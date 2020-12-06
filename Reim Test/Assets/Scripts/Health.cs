using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    HealthBar healthBar;        // health bar reference

    [SerializeField]
    PlayerController player;    // player reference

    [SerializeField]
    int maxHealth = 100;        // player's maximum amount of health
    int currentHealth;          // player's current health

    bool dead = false;          // true if current health reaches 0

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
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
        }
    }

    public void DisableHPBar()
    {
        healthBar.gameObject.SetActive(false);
    }
}