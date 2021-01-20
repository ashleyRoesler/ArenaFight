using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class AttackController : NetworkedBehaviour
{
    [Header("References")]
    [SerializeField]
    private PlayerController player;    
    [SerializeField]
    private Attack sword;           
    [SerializeField]
    private GameObject hand;            // player's hand, used for firing projectile
    [SerializeField]
    private Attack punch;          

    private Attack projectile;      

    private int attackId = 0;           // type of attack (punch or sword)

    private bool attacking = false;     // false if attack animation is not playing

    private bool swordOn = false;       // true if sword drawn

    [Header("Damage Values")]
    [SerializeField]
    private int punchPower = 10;        // punch hp damage
    [SerializeField]
    private int swordPower = 20;        // sword hp damage
    [SerializeField]
    private int magicPower = 15;        // magic hp damage

    [Header("Attack Speeds")]
    [SerializeField]
    private float punchSpeed = 1.0f;             // punch animation speed
    [SerializeField]
    private float swordSpeed = 1.0f;             // sword animation speed
    [SerializeField]
    private float magicSpeed = 1.0f;             // magic animation speed
    [SerializeField]
    private float projectileSpeed = 50.0f;       // magic projectile speed


    #region Initialization
    private void Start()
    {
        // set attack speeds
        player.anim.SetFloat("Punch Speed", punchSpeed);
        player.anim.SetFloat("Sword Speed", swordSpeed);
        player.anim.SetFloat("Magic Speed", magicSpeed);

        // set melee values
        sword.SetPlayer(this);
        sword.SetPower(swordPower);
        punch.SetPlayer(this);
        punch.SetPower(punchPower);

        // get magic projectile
        projectile = Resources.Load("magic projectile") as Attack;
        projectile.SetPlayer(this);
        projectile.SetPower(magicPower);
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
                    punch.GetComponent<Attack>().ResetCollide();
                    break;
                case 1:
                    attackId = 0;
                    punch.GetComponent<Attack>().ResetCollide();
                    break;
                case 2:
                    sword.GetComponent<Attack>().ResetCollide();
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
            Attack magic = Instantiate(projectile, hand.transform.position, Quaternion.identity) as Attack;
            magic.GetComponent<NetworkedObject>().Spawn();

            // fire projectile
            Rigidbody rb = magic.GetComponent<Rigidbody>();
            rb.AddForce(player.transform.forward * projectileSpeed, ForceMode.Force);
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
        sword.gameObject.SetActive(onf);
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

    #region Getters
    public int GetPunchPower()
    {
        return punchPower;
    }

    public int GetSwordPower()
    {
        return swordPower;
    }

    public int GetMagicPower()
    {
        return magicPower;
    }

    #endregion
}