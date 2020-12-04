using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform cam;           // reference to camera

    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}