﻿using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class Attack : NetworkedBehaviour
{
    [Header("References")]
    [SerializeField]
    private PlayerController player;    // player reference
    [SerializeField]
    private GameObject sword;           // player's sword
    [SerializeField]
    private GameObject hand;            // player's hand, used for firing projectile
    [SerializeField]
    private GameObject punch;           // player's punch volume

    private GameObject projectile;      // magic projectile

    private int attackId = 0;           // type of attack (punch or sword)

    private bool attacking = false;     // false if attack animation is not playing

    private bool swordOn = false;       // true if sword drawn

    #region Initialization
    private void Start()
    {
        // set attack speeds
        player.anim.SetFloat("Punch Speed", Stats.instance.punchSpeed);
        player.anim.SetFloat("Sword Speed", Stats.instance.swordSpeed);
        player.anim.SetFloat("Magic Speed", Stats.instance.magicSpeed);

        // get magic projectile
        projectile = Resources.Load("magic projectile") as GameObject;
    }
    #endregion

    #region Do Attack
    void Update()
    {
        // don't attack if you are already attacking, not the local player, or if the game hasn't started
        if (attacking || !IsLocalPlayer || !ArenaManager.gameHasStarted)
        {
            return;
        }

        // sheath/draw sword
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleSword(!swordOn);

            // make sure sword is toggled across both host and clients
            if (IsHost)
            {
                // apply client side
                InvokeClientRpcOnEveryone(SendToggleToClient, swordOn);
            }
            else
            {
                // apply host side
                InvokeServerRpc(SendToggleToHost, swordOn);
            }
        }

        // punch or swing sword
        if (Input.GetMouseButtonDown(0) && !attacking)
        {
            // set attacking to true
            player.anim.SetBool("Attacking", true);
            attacking = true;

            // set animation and attack type
            player.anim.SetInteger("Attack Type", attackId);

            // switch punches or reset sword
            switch (attackId)
            {
                case 0:
                    attackId = 1;
                    punch.GetComponent<Melee>().ResetCollide();
                    break;
                case 1:
                    attackId = 0;
                    punch.GetComponent<Melee>().ResetCollide();
                    break;
                case 2:
                    sword.GetComponent<Melee>().ResetCollide();
                    break;
            }
        }

        // fire magic
        if (Input.GetKeyDown(KeyCode.Q) && !attacking)
        {
            // set attacking to true
            player.anim.SetBool("Attacking", true);
            attacking = true;

            // set animation
            player.anim.SetInteger("Attack Type", 3);
        }
    }

    public void FireMagic()
    {
        if (IsHost)
        {
            // spawn magic projectile
            GameObject magic = Instantiate(projectile, hand.transform.position, Quaternion.identity) as GameObject;
            magic.GetComponent<NetworkedObject>().Spawn();

            // fire projectile
            Rigidbody rb = magic.GetComponent<Rigidbody>();
            rb.AddForce(player.transform.forward * Stats.instance.projectileSpeed, ForceMode.Force);
        }
    }
    #endregion

    #region Toggle Attack
    public void EndAttack()
    {
        // turn animation and attacking off
        attacking = false;
        player.anim.SetBool("Attacking", false);

        // only turn off attack collision for melee, not magic
        if (player.anim.GetInteger("Attack Type") == 3)
        {
            return;
        }

        // turn attack collision off
        if (attackId < 2)
        {
            TogglePunchCollision();
        }
        else if (attackId == 2)
        {
            ToggleSwordCollision();
        }
    }

    public void ToggleSword(bool onf)
    {
        sword.SetActive(onf);
        swordOn = onf;

        if (onf)    // turn sword on
        {
            attackId = 2;
        }
        else       // turn sword off
        {
            attackId = 0;
        }
    }

    [ServerRPC]
    private void SendToggleToHost(bool onf)
    {
        ToggleSword(onf);
    }

    [ClientRPC]
    private void SendToggleToClient(bool onf)
    {
        ToggleSword(onf);
    }

    public void ToggleSwordCollision()
    {
        sword.GetComponent<BoxCollider>().enabled = !sword.GetComponent<BoxCollider>().enabled;
    }

    public void TogglePunchCollision()
    {
        punch.GetComponent<BoxCollider>().enabled = !punch.GetComponent<BoxCollider>().enabled;
    }
    #endregion

    #region Status Check
    public bool IsAttacking()
    {
        return attacking;
    }

    public bool GetSwordToggle()
    {
        return swordOn;
    }
    #endregion
}