using Mirror;
using UnityEngine;

public class MainMenuNetworkManager : MonoBehaviour
{
    public void StartHost()
    {
        NetworkManager.singleton.StartHost();
    }

    public void JoinGame()
    {
        NetworkManager.singleton.networkAddress = "localhost"; // Change as needed
        NetworkManager.singleton.StartClient();
    }

    public void Disconnect()
    {
        NetworkManager.singleton.StopHost();
    }
}
