using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private PlayerController player;    // player reference

    [SerializeField]
    private HealthBar healthBar;        // health bar reference

    [Header("Health Values")]
    [SerializeField]
    private int maxHealth = 100;        // player's maximum amount of health
    private int currentHealth;          // player's current health

    private bool dead = false;          // true if current health reaches 0

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        // test health
        if (!player.isActive && !dead)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
               TakeDamage(10);
            }
        }

        if (!player.isActive || dead)
        {
            return;
        }
    }

    public bool IsDead()
    {
        return dead;
    }

    public void TakeDamage(int damage)
    {
        // decrease health
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        // die (play animation and turn off script)
        if (currentHealth <= 0)
        {
            dead = true;
            player.anim.SetBool("IsDead", true);
            player.enabled = false;

            // make health bar disappear
            DisableHPBar();

            // update arena manager
            ArenaManager.numAlive--;
        }
    }

    public void DisableHPBar()
    {
        healthBar.gameObject.SetActive(false);
    }
}