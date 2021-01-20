using UnityEngine;
using MLAPI;

public class Billboard : NetworkedBehaviour
{
    private static Transform _localCam;      

    private void Start()
    {
        if (IsLocalPlayer)
        {
            // get the local player's camera transform
            _localCam = gameObject.transform.parent.gameObject.GetComponent<PlayerController>().GetCamTransform();

        }
    }

    void LateUpdate()
    {
        // point the billboard at the camera
        transform.LookAt(transform.position + _localCam.forward);
    }
}