using UnityEngine;
using MLAPI;

public class Health : NetworkedBehaviour
{
    [SerializeField]
    private PlayerController _player;    
    [SerializeField]
    private HealthBar _healthBar;        

    [SerializeField]
    private int _maxHealth = 100;
    private int _currentHealth;          

    private bool _dead = false;          // true if current health reaches 0

    #region Initialization and Misc.
    void Start()
    {
        _currentHealth = _maxHealth;
        _healthBar.SetMaxHealth(_currentHealth);
    }

    public bool IsDead()
    {
        return _dead;
    }

    public void DisableHPBar()
    {
        _healthBar.gameObject.SetActive(false);
    }
    #endregion

    #region Take Damage
    public void TakeDamage(int damage)
    {
        // make sure not already dead
        if (_dead)
        {
            return;
        }

        // decrease health
        _currentHealth -= damage;
        _healthBar.SetHealth(_currentHealth);

        // die (play animation and turn off script)
        if (_currentHealth <= 0)
        {
            _dead = true;
            _player.anim.SetBool("IsDead", true);
            _player.enabled = false;

            // make health bar disappear
            DisableHPBar();

            // turn player collision off
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            gameObject.GetComponent<CharacterController>().enabled = false;

            // update arena manager
            ArenaManager.NumAlive--;
        }
    }
    #endregion
}