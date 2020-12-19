using UnityEngine;
using UnityEngine.SceneManagement;
using MLAPI;
using System.Collections.Generic;

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
        Debug.Log("Thanks for playing :)");     // needed for testing since you cannot quit the editor
        Application.Quit();
    }

    private void Disconnect()
    {
        if (IsHost)
        {
            // if the host disconnects, make sure to disconnect all clients
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
}