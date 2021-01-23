using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class AttackController : NetworkedBehaviour
{
    [Header("References")]
    [SerializeField]
    private PlayerController _player;    
    [SerializeField]
    private Attack _sword;           
    [SerializeField]
    private GameObject _hand;            // player's left hand, used for firing projectile
    [SerializeField]
    private Attack _punch;          
    [SerializeField]
    private Attack _magic;

    private int _attackId = 0;           // type of attack (punch or sword)

    private bool _attacking = false;     // false if attack animation is not playing

    private bool _swordOn = false;       // true if sword drawn

    #region Initialization
    private void Start()
    {
        // set attack speeds
        _player.anim.SetFloat("Punch Speed", _punch.Skill.Speed);
        _player.anim.SetFloat("Sword Speed", _sword.Skill.Speed);
        _player.anim.SetFloat("Magic Speed", _magic.Skill.Speed);

        // set players
        _sword.SetPlayer(this);
        _punch.SetPlayer(this);
        _magic.SetPlayer(this);
    }
    #endregion

    #region Do Attack
    void Update()
    {
        // don't attack if you are already attacking, not the local player, or if the game hasn't started
        if (_attacking || !IsLocalPlayer || !ArenaManager.GameHasStarted)
        {
            return;
        }

        // sheath/draw sword
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleSword(!_swordOn);

            // make sure sword is toggled across both host and clients
            if (IsHost)
            {
                // apply client side
                InvokeClientRpcOnEveryone(SendToggleToClient, _swordOn);
            }
            else
            {
                // apply host side
                InvokeServerRpc(SendToggleToHost, _swordOn);
            }
        }

        // punch or swing sword
        if (Input.GetMouseButtonDown(0) && !_attacking)
        {
            // set attacking to true
            _player.anim.SetBool("Attacking", true);
            _attacking = true;

            // set animation and attack type
            _player.anim.SetInteger("Attack Type", _attackId);

            // switch punches or reset sword
            switch (_attackId)
            {
                case 0:
                    _attackId = 1;
                    _punch.GetComponent<Attack>().ResetCollide();
                    break;
                case 1:
                    _attackId = 0;
                    _punch.GetComponent<Attack>().ResetCollide();
                    break;
                case 2:
                    _sword.GetComponent<Attack>().ResetCollide();
                    break;
            }
        }

        // fire magic
        if (Input.GetKeyDown(KeyCode.Q) && !_attacking)
        {
            // set attacking to true
            _player.anim.SetBool("Attacking", true);
            _attacking = true;

            // set animation
            _player.anim.SetInteger("Attack Type", 3);
        }
    }

    public void FireMagic()
    {
        if (IsHost)
        {
            // spawn magic projectile
            GameObject magic = Instantiate(_magic.gameObject, _hand.transform.position, Quaternion.identity) as GameObject;
            magic.GetComponent<NetworkedObject>().Spawn();
            magic.GetComponent<Attack>().SetPlayer(this);

            // fire projectile
            Rigidbody rb = magic.GetComponent<Rigidbody>();
            rb.AddForce(_player.transform.forward * ((Skill_Projectile)_magic.Skill).ProjectileSpeed, ForceMode.Force);
        }
    }
    #endregion

    #region Toggle Attack
    public void EndAttack()
    {
        // turn animation and attacking off
        _attacking = false;
        _player.anim.SetBool("Attacking", false);

        // only turn off attack collision for melee, not magic
        if (_player.anim.GetInteger("Attack Type") == 3)
        {
            return;
        }

        // turn attack collision off
        if (_attackId < 2)
        {
            TogglePunchCollision();
        }
        else if (_attackId == 2)
        {
            ToggleSwordCollision();
        }
    }

    public void ToggleSword(bool onf)
    {
        _sword.gameObject.SetActive(onf);
        _swordOn = onf;

        if (onf)    // turn sword on
        {
            _attackId = 2;
        }
        else       // turn sword off
        {
            _attackId = 0;
        }
    }

    [ServerRPC]
    private void SendToggleToHost(bool onf)
    {
        ToggleSword(onf);
    }

    [ClientRPC]
    private void SendToggleToClient(bool onf)
    {
        ToggleSword(onf);
    }

    public void ToggleSwordCollision()
    {
        _sword.GetComponent<BoxCollider>().enabled = !_sword.GetComponent<BoxCollider>().enabled;
    }

    public void TogglePunchCollision()
    {
        _punch.GetComponent<BoxCollider>().enabled = !_punch.GetComponent<BoxCollider>().enabled;
    }
    #endregion

    #region Status Check
    public bool IsAttacking()
    {
        return _attacking;
    }

    public bool GetSwordToggle()
    {
        return _swordOn;
    }
    #endregion
}