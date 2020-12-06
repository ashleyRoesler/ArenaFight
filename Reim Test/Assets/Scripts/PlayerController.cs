using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public CharacterController controller;      // player controller reference
    public Transform cam;                       // camera reference
    public Animator anim;                       // animator reference

    [Header("Player Movement")]
    [SerializeField]
    float speed = 6f;                           // player speed

    [SerializeField]
    float turnSmoothTime = 0.1f;                // smooths player rotation
    float turnSmoothVelocity;                   // current smooth velocity

    [Header("Player Health")]
    public Health HP;                           // player's health information

    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*=====================================================
                               HEALTH
        =====================================================*/

        // test health
        if (!isActive && !HP.IsDead())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                HP.TakeDamage(10);
            }
        }

        if (!isActive || HP.IsDead())
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
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
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
}