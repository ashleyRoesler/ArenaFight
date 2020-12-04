using UnityEngine;

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
        // check for win condition
        if (!gameOver && (Player1.isDead ^ Player2.isDead ^ Player3.isDead))
        {
            gameOver = true;

            if (!Player1.isDead)
            {
                Player1.anim.SetBool("Winner", true);
                Player1.enabled = false;
            }
            else if (!Player2.isDead)
            {
                Player2.anim.SetBool("Winner", true);
                Player2.enabled = false;
            }
            else
            {
                Player3.anim.SetBool("Winner", true);
                Player3.enabled = false;
            }
        }
    }
}
