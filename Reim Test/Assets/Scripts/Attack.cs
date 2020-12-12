using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private PlayerController player;    // player reference
    [SerializeField]
    private GameObject sword;           // player's sword
    [SerializeField]
    private GameObject hand;            // player's hand, used for firing projectile

    private GameObject projectile;      // magic projectile

    private int attackId = 0;           // type of attack (punch or sword)

    private bool attacking = false;     // false if attack animation is not playing

    private bool swordOn = false;       // true if sword drawn

    // Start is called before the first frame update
    void Start()
    {
        // set attack speeds
        player.anim.SetFloat("Punch Speed", Stats.punchS);
        player.anim.SetFloat("Sword Speed", Stats.swordS);
        player.anim.SetFloat("Magic Speed", Stats.magicS);

        // start with sword sheathed
        sword.SetActive(false);

        // get magic projectile
        projectile = Resources.Load("magic projectile") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // make sure the player isn't already attacking
        if (player.isActive && !attacking)
        {
            // sheath/draw sword
            if (Input.GetKeyDown(KeyCode.F))
            {
                ToggleSword(!swordOn);
            }

            // punch or swing sword
            if (Input.GetMouseButtonDown(0))
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
                        break;
                    case 1:
                        attackId = 0;
                        break;
                    case 2:
                        sword.GetComponent<Melee>().ResetCollide();
                        break;
                }
            }

            // fire magic
            if (Input.GetKeyDown(KeyCode.Q))
            {
                // set attacking to true
                player.anim.SetBool("Attacking", true);
                attacking = true;

                // set animation
                player.anim.SetInteger("Attack Type", 3);
            }
        }
    }

    public void EndAttack()
    {
        // turn animation and attacking off
        attacking = false;
        player.anim.SetBool("Attacking", false);
    }

    public bool IsAttacking()
    {
        return attacking;
    }

    public bool getSwordToggle()
    {
        return swordOn;
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

    public void FireMagic()
    {
        // spawn magic projectile
        GameObject magic = Instantiate(projectile) as GameObject;
        magic.transform.position = hand.transform.position;

        // fire projectile
        Rigidbody rb = magic.GetComponent<Rigidbody>();
        rb.AddForce(player.transform.forward * Stats.projectileS, ForceMode.Force);
    }
}