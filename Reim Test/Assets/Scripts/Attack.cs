using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private PlayerController player;

    private int attackId = 0;

    [Header("Attack Values")]
    [SerializeField]
    private int punchPower = 10;        // punch hp damage

    [SerializeField]
    private int swordPower = 20;        // sword hp damage

    [SerializeField]
    private int magicPower = 30;        // magic hp damage

    // Update is called once per frame
    void Update()
    {
        if (player.isActive && Input.GetMouseButtonDown(0))
        {
            // set attacking to true
            player.anim.SetBool("Attacking", true);
            player.ToggleAttack();

            // set animation and attack type
            player.anim.SetInteger("Attack Type", attackId);

            switch (attackId)
            {
                case 0:
                    Debug.Log("Left Punch");
                    break;
                case 1:
                    Debug.Log("Right Punch");
                    break;
                case 2:
                    Debug.Log("Sword Slice");
                    break;
                case 3:
                    Debug.Log("Pew Pew Magic");
                    break;
                default:
                    Debug.LogWarning("Bro, your attack id is wack.");
                    break;
            }

            if (attackId == 0)
            {
                attackId = 1;
            }
            else if (attackId == 1)
            {
                attackId = 0;
            }

            // turn attacking off
            player.ToggleAttack();
        }
    }
}