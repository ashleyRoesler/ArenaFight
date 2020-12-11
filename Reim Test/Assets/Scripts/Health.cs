using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;    // player reference

    [SerializeField]
    private HealthBar healthBar;        // health bar reference

    private int currentHealth;          // player's current health

    private bool dead = false;          // true if current health reaches 0

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = Stats.maxHP;
        healthBar.SetMaxHealth(Stats.maxHP);
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