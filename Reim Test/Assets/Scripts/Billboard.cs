using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField]
    private Transform cam;           // reference to camera

    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}