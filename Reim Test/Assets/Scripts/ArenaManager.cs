﻿using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    public PlayerController Player1;      // reference to player 1
    public PlayerController Player2;      // reference to player 2
    public PlayerController Player3;      // reference to player 3

    bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // check for win condition (one player is left standing)
        if (!gameOver && ((!Player1.isDead && Player2.isDead && Player3.isDead) || (Player1.isDead && !Player2.isDead && Player3.isDead) || (Player1.isDead && Player2.isDead && !Player3.isDead)))
        {
            gameOver = true;

            // player 1 is the winner
            if (!Player1.isDead)
            {
                // play animation and disable script
                Player1.anim.SetBool("Winner", true);
                Player1.enabled = false;

                // make health bar disappear
                Player1.healthBar.gameObject.SetActive(false);
            }

            // player 2 is the winner
            else if (!Player2.isDead)
            {
                // play animation and disable script
                Player2.anim.SetBool("Winner", true);
                Player2.enabled = false;

                // make health bar disappear
                Player2.healthBar.gameObject.SetActive(false);
            }

            // player 3 is the winner
            else
            {
                // play animation and disable script
                Player3.anim.SetBool("Winner", true);
                Player3.enabled = false;

                // make health bar disappear
                Player3.healthBar.gameObject.SetActive(false);
            }
        }
    }
}