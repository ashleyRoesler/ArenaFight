using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using MLAPI;
using MLAPI.Spawning;

public class ArenaManager : NetworkedBehaviour
{
    private static bool isGameHost = false;                         // true if local player is game host

    private List<PlayerController> players;                         // list of current players
    private List<GameObject> spawnAreas;                            // list of player spawn areas
    
    [Header("Required Player Count")]
    public int requiredPlayerCount = 3;                             // number of players needed to start the game

    [SerializeField]
    private GameObject victoryCanvas;                               // reference to victory screen

    public static int numAlive = 0;                                 // number of players left alive

    private bool gameOver = false;                                  // true if only one player is left
    public static bool gameHasStarted = false;                      // true if number of players equals the required amount

    public delegate void UpdateWait(int current, int needed);       // used to update the waiting counter text
    public static event UpdateWait OnUpdateWait;

    #region Networking
    public static void BeHost(bool yes)
    {
        isGameHost = yes;
    }

    private void StartHost()
    {
        NetworkingManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkingManager.Singleton.StartHost(spawnAreas[0].transform.position, spawnAreas[0].transform.rotation);

        // turn cursor off
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void StartClient()
    {
        NetworkingManager.Singleton.StartClient();

        // turn cursor off
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void ApprovalCheck(byte[] connectionData, ulong clientId, MLAPI.NetworkingManager.ConnectionApprovedDelegate callback)
    {
        bool approve = false;
        bool createPlayerObject = false;
        ulong? prefabHash = null;
        Transform spawn = new GameObject().transform;

        // if players are still needed, accept the connection
        // note: numAlive starts at 1 because the host has already spawned
        if (numAlive < requiredPlayerCount)
        {
            approve = true;
            createPlayerObject = true;

            // spawning different player prefabs at different locations
            if (numAlive < spawnAreas.Count)
            {
                prefabHash = SpawnManager.GetPrefabHashFromGenerator("PlayerCam " + (numAlive + 1));
                spawn = spawnAreas[numAlive].transform;
            }
            else
            {
                Debug.LogError("Not enough spawn areas");
            }
        }

        // reject or accept connection, spawning desired player at desired location
        callback(createPlayerObject, prefabHash, approve, spawn.position, spawn.rotation);
    }
    #endregion

    #region Game Initialization
    private void OnEnable()
    {
        PlayerController.OnJoin += AddPlayer;
    }

    private void OnDisable()
    {
        PlayerController.OnJoin -= AddPlayer;
    }

    private void Awake()
    {
        // get all spawn locations and initialize start spots
        spawnAreas = GameObject.FindGameObjectsWithTag("Spawn").ToList();

        // initialize players list
        players = new List<PlayerController>();

        // reset static variables
        numAlive = 0;
        gameHasStarted = false;

        // start game
        if (isGameHost)
        {
            StartHost();
        }
        else
        {
            StartClient();
        }
    }

    public void AddPlayer(PlayerController player)
    {
        // don't add any players if the game has already started
        if (gameHasStarted)
        {
            return;
        }

        players.Add(player);
        numAlive++;

        // update waiting text (waiting script starts game)
        OnUpdateWait?.Invoke(numAlive, requiredPlayerCount);
    }
    #endregion

    #region Win Condition
    /*=====================================================
                         WIN CONDITION
    =====================================================*/

    void Update()
    {
        // check for win condition (one player is left standing)
        if (gameHasStarted && !gameOver && numAlive == 1)
        {
            gameOver = true;

            // display victory screen and turn cursor back on
            victoryCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;

            // determine who is the winner
            foreach (PlayerController pc in players)
            {
                if (!pc.HP.IsDead())
                {
                    // play animation and disable script
                    pc.anim.SetBool("Winner", true);
                    pc.enabled = false;

                    // make health bar disappear
                    pc.HP.DisableHPBar();
                }
            }
        }
    }
    #endregion
}