using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;    // player reference
    [SerializeField]
    private HealthBar healthBar;        // health bar reference

    private int currentHealth;          // player's current health

    private bool dead = false;          // true if current health reaches 0

    void Start()
    {
        currentHealth = Stats.instance.maxHealth;
        healthBar.SetMaxHealth(currentHealth);
    }

    public bool IsDead()
    {
        return dead;
    }

    public void TakeDamage(int damage)
    {
        // make sure not already dead
        if (dead)
        {
            return;
        }

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

            // turn player collision off
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            gameObject.GetComponent<CharacterController>().enabled = false;

            // update arena manager
            ArenaManager.numAlive--;
        }
    }

    public void DisableHPBar()
    {
        healthBar.gameObject.SetActive(false);
    }
}