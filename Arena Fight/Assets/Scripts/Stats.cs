using UnityEngine;

public class Stats : MonoBehaviour
{
    public static Stats instance = null;        // singleton instance

    [Header("Player Movement")]
    public float speed = 6f;                   // player speed 
    public float turnSmoothTime = 0.1f;        // smooths player rotation

    [Header("Health")]
    public int maxHealth = 100;        // player's maximum amount of health

    [Header("Damage Values")]
    public int punchPower = 10;        // punch hp damage
    public int swordPower = 20;        // sword hp damage
    public int magicPower = 15;        // magic hp damage

    [Header("Attack Speeds")]
    public float punchSpeed = 1.0f;             // punch animation speed
    public float swordSpeed = 1.0f;             // sword animation speed
    public float magicSpeed = 1.0f;             // magic animation speed
    public float projectileSpeed = 50.0f;       // magic projectile speed

    private void Awake()
    {
        // get singleton instance
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}