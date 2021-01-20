using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using MLAPI;
using MLAPI.Spawning;
using TMPro;

public class ArenaManager : NetworkedBehaviour
{
    private static bool _isGameHost = false;                        // true if local player is game host

    private List<PlayerController> _players;                        // list of current players
    private List<GameObject> _spawnAreas;                           // list of player spawn areas
    
    private static int _requiredPlayerCount;                             

    [SerializeField]
    private GameObject _victoryCanvas;                               

    public static int NumAlive = 0;                                 // number of players left alive

    private bool _gameOver = false;                                 // true if only one player is left
    public static bool GameHasStarted = false;                      // true if number of players equals the required amount

    public delegate void UpdateWait(int current, int needed);       
    public static event UpdateWait OnUpdateWait;

    #region Pre-Game Networking
    public static void BeHost(bool yes, int PC)
    {
        _isGameHost = yes;
        _requiredPlayerCount = PC;
    }

    private void StartHost()
    {
        NetworkingManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkingManager.Singleton.StartHost(_spawnAreas[0].transform.position, _spawnAreas[0].transform.rotation);

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
        if (NumAlive < _requiredPlayerCount && !GameHasStarted)
        {
            approve = true;
            createPlayerObject = true;

            // spawning different player prefabs at different locations
            if (NumAlive < _spawnAreas.Count)
            {
                prefabHash = SpawnManager.GetPrefabHashFromGenerator("PlayerCam " + (NumAlive + 1));
                spawn = _spawnAreas[NumAlive].transform;
            }
            else
            {
                Debug.LogError("Not enough spawn areas");
            }
        }

        // reject or accept connection, spawning desired player at desired location
        callback(createPlayerObject, prefabHash, approve, spawn.position, spawn.rotation);
    }

    private void RemoveCallback()
    {
        NetworkingManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
    }

    #endregion

    #region Game Initialization
    private void OnEnable()
    {
        PlayerController.OnJoin += AddPlayer;
        MainMenu.OnRestartCallback += RemoveCallback;
    }

    private void OnDisable()
    {
        PlayerController.OnJoin -= AddPlayer;
        MainMenu.OnRestartCallback -= RemoveCallback;
        _spawnAreas.Clear();
    }

    private void Awake()
    {
        // get all spawn locations and initialize start spots
        _spawnAreas = GameObject.FindGameObjectsWithTag("Spawn").ToList();

        // initialize players list
        _players = new List<PlayerController>();

        // reset static variables
        NumAlive = 0;
        GameHasStarted = false;

        // start game
        if (_isGameHost)
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
        if (GameHasStarted)
        {
            return;
        }

        _players.Add(player);
        NumAlive++;

        // update waiting text (waiting script starts game)
        OnUpdateWait?.Invoke(NumAlive, _requiredPlayerCount);
    }

    #endregion

    #region Win Condition
    /*=====================================================
                         WIN CONDITION
    =====================================================*/

    void Update()
    {
        // check for win condition (one player is left standing)
        if (GameHasStarted && !_gameOver && NumAlive == 1)
        {
            _gameOver = true;

            // determine who is the winner
            foreach (PlayerController pc in _players)
            {
                if (!pc.HP.IsDead())
                {
                    // play animation and disable script
                    pc.anim.SetBool("Winner", true);
                    pc.enabled = false;

                    // make health bar disappear
                    pc.HP.DisableHPBar();

                    // display victory screen
                    _victoryCanvas.SetActive(true);

                    string pcName = pc.transform.parent.name;
                    pcName = pcName.Replace("(Clone)", string.Empty);
                    pcName = pcName.Replace("Cam", string.Empty);

                    _victoryCanvas.GetComponentInChildren<TextMeshProUGUI>().text = pcName + " Wins!";

                    // turn cursor back on
                    Cursor.lockState = CursorLockMode.None;

                    break;
                }
            }
        }
    }
    #endregion
}