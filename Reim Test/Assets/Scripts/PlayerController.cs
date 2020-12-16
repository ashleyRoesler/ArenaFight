using UnityEngine;
using MLAPI;
using Cinemachine;

public class PlayerController : NetworkedBehaviour
{
    [Header("References")]
    public CharacterController controller;      // player controller reference
    public Animator anim;                       // animator reference 
    [SerializeField]
    private GameObject localCam;                // player camera reference
    [SerializeField]
    private Transform lookAt;                   // player camera's focus

    private float turnSmoothVelocity;           // current smooth velocity

    [Header("Player Health")]
    public Health HP;                           // player's health information

    [Header("Player Attack")]
    public Attack attack;                       // player's attack information

    // Start is called before the first frame update
    void Start()
    {
        // spawn a camera for the local player
     /*   if (IsLocalPlayer)
        {
            Instantiate(localCam, new Vector3(0f, 1.8f, -5.6f), Quaternion.identity);

            // force camera to look at player
            localCam.GetComponentInChildren<CinemachineFreeLook>().m_LookAt = lookAt;
            localCam.GetComponentInChildren<CinemachineFreeLook>().m_Follow = lookAt;
        }*/

        if (!IsLocalPlayer)
        {
            localCam.SetActive(false);
        }

        // get reference to animator component
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attack.IsAttacking())
        {
            return;
        }

        /*=====================================================
                              MOVEMENT
        =====================================================*/

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
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, Stats.sSmoothT);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // move player based on input and camera rotation
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * Stats.sSpeed * Time.deltaTime);
        }
        else
        {
            // set animation to idle
            anim.SetBool("IsRunning", false);
        }
    }

    private void OnDisable()
    {
        // make sure sword disappears on death or victory
        attack.ToggleSword(false);
    }

    public Transform GetCamTransform()
    {
        return localCam.transform;
    }
}