using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cam;           // reference to camera location

    private void Start()
    {
        cam = gameObject.transform.parent.gameObject.GetComponent<PlayerController>().GetCamTransform();
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}