using UnityEngine;

public class Stats : MonoBehaviour
{
    [Header("Player Movement")]
    public float speed = 6f;                   // player speed 
    public float turnSmoothTime = 0.1f;        // smooths player rotation

    // static versions
    public static float sSpeed = 6f;
    public static float sSmoothT = 0.1f;

    [Header("Health")]
    public int maxHealth = 100;        // player's maximum amount of health

    // static version
    public static int maxHP = 100;

    [Header("Damage Values")]
    public int punchPower = 10;        // punch hp damage
    public int swordPower = 20;        // sword hp damage
    public int magicPower = 15;        // magic hp damage

    // static versions
    public static int punchP = 10;
    public static int swordP = 20;
    public static int magicP = 15;

    [Header("Attack Speeds")]
    public float punchSpeed = 1.0f;             // punch animation speed
    public float swordSpeed = 1.0f;             // sword animation speed
    public float magicSpeed = 1.0f;             // magic animation speed
    public float projectileSpeed = 50.0f;       // magic projectile speed

    // static versions
    public static float punchS = 1.0f;
    public static float swordS = 1.0f;            
    public static float magicS = 1.0f;          
    public static float projectileS = 50.0f; 

    private void Awake()
    {
        // update all static values with values from inspector

        // player movement
        sSpeed = speed;
        sSmoothT = turnSmoothTime;

        // health
        maxHP = maxHealth;

        // damage
        punchP = punchPower;
        swordP = swordPower;
        magicP = magicPower;

        // speeds
        punchS = punchSpeed;
        swordS = swordSpeed;
        magicS = magicSpeed;
        projectileS = projectileSpeed;
    }
}