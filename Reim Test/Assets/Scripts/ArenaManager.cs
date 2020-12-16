using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ArenaManager : MonoBehaviour
{
    private List<PlayerController> players;

    [SerializeField]
    private GameObject victoryCanvas;     // reference to victory screen

    public static int numAlive;           // number of players left alive

    private bool gameOver = false;        // true if only one player is left

    void Awake()
    {
        // get list of players
        players = Object.FindObjectsOfType<PlayerController>().ToList();

        // reset alive counter
        numAlive = players.Count;

        // turn cursor off
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // check for win condition (one player is left standing)
        if (!gameOver && numAlive == 1)
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