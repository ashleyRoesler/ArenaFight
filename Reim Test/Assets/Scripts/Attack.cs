using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private GameObject sword;

    GameObject projectile;              // magic projectile
    [SerializeField]
    private GameObject hand;            // player's hand, used for firing projectile

    private int attackId = 0;           // type of attack (punch or sword)

    [Header("Damage Values")]
    [SerializeField]
    private int punchPower = 10;        // punch hp damage
    [SerializeField]
    private int swordPower = 20;        // sword hp damage
    [SerializeField]
    private int magicPower = 15;        // magic hp damage

    [Header("Attack Speeds")]
    [SerializeField]
    private float punchSpeed = 1.0f;    // punch animation speed
    [SerializeField]
    private float swordSpeed = 1.0f;    // sword animation speed
    [SerializeField]
    private float magicSpeed = 1.0f;    // magic animation speed
    [SerializeField]
    private float projectileSpeed = 20.0f;      // magic projectile speed

    private bool attacking = false;     // false if attack animation is not playing

    private bool swordOn = false;       // true if sword drawn

    // Start is called before the first frame update
    void Start()
    {
        // set attack speeds
        player.anim.SetFloat("Punch Speed", punchSpeed);
        player.anim.SetFloat("Sword Speed", swordSpeed);
        player.anim.SetFloat("Magic Speed", magicSpeed);

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
                ToggleSword();
            }

            // punch or swing sword
            if (Input.GetMouseButtonDown(0))
            {
                // set attacking to true
                player.anim.SetBool("Attacking", true);
                attacking = true;

                // set animation and attack type
                player.anim.SetInteger("Attack Type", attackId);

                // switch punch
                if (attackId == 0)
                {
                    attackId = 1;
                }
                else if (attackId == 1)
                {
                    attackId = 0;
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

    public void ToggleSword()
    {
        // sheath sword
        if (swordOn)
        {
            sword.SetActive(false);
            swordOn = false;
            attackId = 0;
        }
        else
        {
            sword.SetActive(true);
            swordOn = true;
            attackId = 2;
        }
    }

    public void FireMagic()
    {
        // spawn magic projectile
        GameObject magic = Instantiate(projectile) as GameObject;
        magic.transform.position = hand.transform.position;

        // fire projectile
        Rigidbody rb = magic.GetComponent<Rigidbody>();
        rb.velocity = player.transform.forward * projectileSpeed;
    }
}