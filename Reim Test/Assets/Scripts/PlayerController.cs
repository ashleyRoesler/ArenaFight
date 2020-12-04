using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public CharacterController controller;      // player controller reference
    public Transform cam;                       // camera reference
    public Animator anim;                       // animator reference

    [Header("Player Movement")]
    public float speed = 6f;                    // player speed

    public float turnSmoothTime = 0.1f;         // smooths player rotation
    float turnSmoothVelocity;                   // current smooth velocity

    [Header("Player Health")]
    public int maxHealth = 100;                 // player's maximum amount of health
    int currentHealth;                          // player's current health

    public bool isDead = false;                 // true if current health reaches 0

    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // test health
        if (!isActive)
        {
            if (currentHealth == 0 && !isDead)
            {
                isDead = true;
                anim.SetBool("IsDead", true);
                enabled = false;
            }
            else
            {
                //currentHealth--;
            }
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