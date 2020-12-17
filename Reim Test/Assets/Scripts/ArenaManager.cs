using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ArenaManager : MonoBehaviour
{
    private readonly List<PlayerController> players;     // list of current players

    private Dictionary<Transform, bool> startSpots;      // places where players can spawn, and if they have already been filled
    
    [Header("Required Player Count")]
    public int requiredPlayerCount = 3;   // number of players needed to start the game

    [SerializeField]
    private GameObject victoryCanvas;     // reference to victory screen

    public static int numAlive = 0;       // number of players left alive

    private bool gameOver = false;        // true if only one player is left
    private bool gameStart = false;       // true if max player count reached (game is ready to start)

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
        var places = GameObject.FindGameObjectsWithTag("Spawn").ToList();
        startSpots = places.ToDictionary(key => key.transform, value => false);

        // reset alive counter
        numAlive = 0;

        // turn cursor off
        Cursor.lockState = CursorLockMode.Locked;
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

        // move player to proper starting place
        foreach(var pair in startSpots)
        {
            if (!pair.Value)
            {
                player.transform.position = pair.Key.position;
                player.transform.rotation = pair.Key.rotation;

                // mark spot as filled
                startSpots[pair.Key] = true;
            }
        }

        // if required number of players has been reached, start the game
        if (numAlive == requiredPlayerCount)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        gameStart = true;
    }

    // Update is called once per frame
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