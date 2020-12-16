using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public void SetCamLook(Transform location)
    {
        CinemachineFreeLook cam = gameObject.GetComponentInChildren<CinemachineFreeLook>();

        cam.LookAt = location;
        cam.Follow = location;
    }
}