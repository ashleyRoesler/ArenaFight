using UnityEngine;
using MLAPI;

public class Billboard : NetworkedBehaviour
{
    private Transform cam;           // reference to camera location

    private static Transform localCam;

    private void Start()
    {
        cam = gameObject.transform.parent.gameObject.GetComponent<PlayerController>().GetCamTransform();

        if (IsLocalPlayer)
        {
            localCam = gameObject.transform.parent.gameObject.GetComponent<PlayerController>().GetCamTransform();

        }
    }

    void LateUpdate()
    {
        //    transform.LookAt(transform.position + cam.forward);
        transform.LookAt(transform.position + localCam.forward);
    }
}