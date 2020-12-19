using UnityEngine;
using MLAPI;

public class Billboard : NetworkedBehaviour
{
    private static Transform localCam;      // reference to local camera transform

    private void Start()
    {
        if (IsLocalPlayer)
        {
            localCam = gameObject.transform.parent.gameObject.GetComponent<PlayerController>().GetCamTransform();

        }
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + localCam.forward);
    }
}