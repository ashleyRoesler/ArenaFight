using UnityEngine;
using UnityEngine.SceneManagement;
using MLAPI;
using System.Collections.Generic;
using MLAPI.Messaging;

public class MainMenu : NetworkedBehaviour
{
    public void ReplayGame()
    {
        Disconnect();
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Disconnect();
        Debug.Log("Thanks for playing :)");
        Application.Quit();
    }

    private void Disconnect()
    {
        if (IsHost)
        {
            // if the host disconnects, make sure to disconnect all clients
            InvokeClientRpcOnEveryone(SendClientToMenu);

            int cCount = NetworkingManager.Singleton.ConnectedClients.Count;
            var clientIdList = new List<ulong>(NetworkingManager.Singleton.ConnectedClients.Keys);

            for (int i = 0; i < cCount; i++)
            {
                NetworkingManager.Singleton.DisconnectClient(clientIdList[i]);
            }

            NetworkingManager.Singleton.StopHost();
        }
        else if (IsClient)
        {
            NetworkingManager.Singleton.StopClient();
        }
    }

    [ClientRPC]
    private void SendClientToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}