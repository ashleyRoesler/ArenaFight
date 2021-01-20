using UnityEngine;
using MLAPI;

public class Health : NetworkedBehaviour
{
    [SerializeField]
    private PlayerController player;    // player reference
    [SerializeField]
    private HealthBar healthBar;        // health bar reference

    [SerializeField]
    private int maxHealth = 100;
    private int currentHealth;          // player's current health

    private bool dead = false;          // true if current health reaches 0

    #region Initialization and Misc.
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(currentHealth);
    }

    public bool IsDead()
    {
        return dead;
    }

    public void DisableHPBar()
    {
        healthBar.gameObject.SetActive(false);
    }
    #endregion

    #region Take Damage
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
    #endregion
}