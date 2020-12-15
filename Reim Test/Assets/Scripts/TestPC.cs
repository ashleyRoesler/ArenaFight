using UnityEngine;

public class TestPC : MonoBehaviour
{
    CharacterController C;      // player controller reference  
    private float turnSmoothVelocity;           // current smooth velocity

    // Start is called before the first frame update
    void Start()
    {
        // get reference to animator component
        C = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        /*=====================================================
                              MOVEMENT
        =====================================================*/

        // gather movement input information
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {

            C.Move(direction * Stats.sSpeed * Time.deltaTime);
        }
    }
}