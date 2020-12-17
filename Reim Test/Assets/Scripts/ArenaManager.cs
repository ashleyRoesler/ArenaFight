using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using MLAPI;
using MLAPI.Spawning;

public class ArenaManager : NetworkedBehaviour
{
    private List<PlayerController> players;     // list of current players
    private List<GameObject> spawnAreas;        // list of player spawn areas
    
    [Header("Required Player Count")]
    public int requiredPlayerCount = 3;   // number of players needed to start the game

    [SerializeField]
    private GameObject victoryCanvas;     // reference to victory screen

    public static int numAlive = 0;       // number of players left alive

    private bool gameOver = false;        // true if only one player is left
    private bool gameStart = false;       // true if max player count reached (game is ready to start)

    /*=====================================================
                    NETWORK INITIALIZATION
    =====================================================*/
    private bool hasClicked = false;

    private void OnGUI()
    {
        if (!hasClicked && GUI.Button(new Rect(20, 20, 100, 20), "Start Host"))
        {
            NetworkingManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
            NetworkingManager.Singleton.StartHost(spawnAreas[0].transform.position, spawnAreas[0].transform.rotation);

            // turn cursor off
            Cursor.lockState = CursorLockMode.Locked;

            // turn button off
            hasClicked = true;
        }

        if (!hasClicked && GUI.Button(new Rect(20, 120, 100, 20), "Start Client"))
        {
            NetworkingManager.Singleton.StartClient();

            // turn cursor off
            Cursor.lockState = CursorLockMode.Locked;

            // turn button off
            hasClicked = true;
        }
    }

    private void ApprovalCheck(byte[] connectionData, ulong clientId, MLAPI.NetworkingManager.ConnectionApprovedDelegate callback)
    {
        bool approve = false;
        bool createPlayerObject = false;
        ulong? prefabHash = null;
        Transform spawn = new GameObject().transform;

        // if players are still needed, accept the connection
        if (numAlive < requiredPlayerCount)
        {
            approve = true;
            createPlayerObject = true;

            Debug.Log("YEYEYEY");

            // for spawning different player prefabs at different locations
            switch (numAlive)
            {
                case 1: // player 2
                    prefabHash = SpawnManager.GetPrefabHashFromGenerator("PlayerCam 2");
                    spawn = spawnAreas[1].transform;
                    break;
                case 2: // player 3
                    prefabHash = SpawnManager.GetPrefabHashFromGenerator("PlayerCam 3");
                    spawn = spawnAreas[2].transform;
                    break;
                default:
                    prefabHash = null;
                    break;
            }
        }

        // reject or accept connection, spawning desired player at desired location
        callback(createPlayerObject, prefabHash, approve, spawn.position, spawn.rotation);
    }

    /*=====================================================
                      GAME INITIALIZATION
    =====================================================*/

    private void OnEnable()
    {
        PlayerController.OnJoin += AddPlayer;
    }

    private void OnDisable()
    {
        PlayerController.OnJoin -= AddPlayer;
    }

    void Awake()
    {
        // get all spawn locations and initialize start spots
        spawnAreas = GameObject.FindGameObjectsWithTag("Spawn").ToList();

        // initialize players list
        players = new List<PlayerController>();

        // reset alive counter
        numAlive = 0;
    }

    public void AddPlayer(PlayerController player)
    {
        // don't add any players if the game has already started
        if (gameStart)
        {
            return;
        }

        players.Add(player);
        numAlive++;

        // if required number of players has been reached, start the game
        if (numAlive == requiredPlayerCount)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        // enable all players
        foreach (PlayerController P in players)
        {
            P.enabled = true;
        }

        gameStart = true;
    }

    /*=====================================================
                         WIN CONDITION
    =====================================================*/

    void Update()
    {
        // check for win condition (one player is left standing)
        if (gameStart && !gameOver && numAlive == 1)
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
}