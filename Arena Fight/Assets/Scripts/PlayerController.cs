using UnityEngine;
using MLAPI;

public class PlayerController : NetworkedBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject localCam;                               
    [SerializeField]
    private Transform lookAt;                                   // player camera's focus

    private CharacterController controller;                     

    [HideInInspector]
    public Health HP;                                           

    [HideInInspector]
    public Animator anim;                                      

    [HideInInspector]
    public AttackController attack;                                       

    private float turnSmoothVelocity;                           

    [Header("Movement Stats")]

    [SerializeField]
    private float speed = 6f;
    [SerializeField]
    private float turnSmoothTime = 0.1f;

    public delegate void JoinGame(PlayerController player);     // used to add player to arena manager
    public static event JoinGame OnJoin;

    #region Initialization
    private void Start()
    {
        // tell the Arena Manager that you have joined the game
        OnJoin?.Invoke(this);

        // get reference to components
        anim = gameObject.GetComponent<Animator>();
        controller = gameObject.GetComponent<CharacterController>();
        HP = gameObject.GetComponent<Health>();
        attack = gameObject.GetComponent<AttackController>();

        // turn off camera and component control for non-local players
        if (!IsLocalPlayer)
        {
            localCam.SetActive(false);
            controller.enabled = false;
        }
    }
    #endregion

    #region Movement
    void Update()
    {
        // don't move if you are attacking, not the local player, or if the game hasn't started
        if (attack.IsAttacking() || !IsLocalPlayer || !ArenaManager.gameHasStarted)
        {
            return;
        }

        // gather movement input information
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // set animation to running
            anim.SetBool("IsRunning", true);

            // rotate player to make them look where they are going (based on camera rotation as well)
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + localCam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // move player based on input and camera rotation
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        else
        {
            // set animation to idle
            anim.SetBool("IsRunning", false);
        }
    }
    #endregion

    #region Misc. Utility
    private void OnDisable()
    {
        // make sure sword disappears on death or victory
        attack.ToggleSword(false);
    }

    public Transform GetCamTransform()
    {
        return localCam.transform;
    }
    #endregion
}