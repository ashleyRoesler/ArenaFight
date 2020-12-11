﻿using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    public PlayerController Player1;      // reference to player 1
    public PlayerController Player2;      // reference to player 2
    public PlayerController Player3;      // reference to player 3

    public static int numAlive = 3;       // number of players left alive

    private bool gameOver = false;        // true if only one player is left

    // Update is called once per frame
    void Update()
    {
        // check for win condition (one player is left standing)
        if (!gameOver && numAlive == 1)
        {
            gameOver = true;

            // player 1 is the winner
            if (!Player1.HP.IsDead())
            {
                // play animation and disable script
                Player1.anim.SetBool("Winner", true);
                Player1.enabled = false;

                // make health bar disappear
                Player1.HP.DisableHPBar();
            }

            // player 2 is the winner
            else if (!Player2.HP.IsDead())
            {
                // play animation and disable script
                Player2.anim.SetBool("Winner", true);
                Player2.enabled = false;

                // make health bar disappear
                Player2.HP.DisableHPBar();
            }

            // player 3 is the winner
            else
            {
                // play animation and disable script
                Player3.anim.SetBool("Winner", true);
                Player3.enabled = false;

                // make health bar disappear
                Player3.HP.DisableHPBar();
            }
        }
    }
}