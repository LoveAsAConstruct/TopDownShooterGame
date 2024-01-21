using Mirror;
using UnityEngine;

public class PlayerCameraController : NetworkBehaviour
{
    public Camera playerCamera;
    public GameObject playerLight;

    void Start()
    {
        if (isLocalPlayer)
        {
            playerCamera.gameObject.SetActive(true);
            playerLight.SetActive(true);
        }
        else
        {
            playerCamera.gameObject.SetActive(false);
            playerLight.SetActive(false);
        }
    }
}
