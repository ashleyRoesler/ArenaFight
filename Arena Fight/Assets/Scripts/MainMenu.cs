using UnityEngine;
using UnityEngine.SceneManagement;
using MLAPI;
using System.Collections.Generic;

public class MainMenu : NetworkedBehaviour
{
    public delegate void RestartCallback();         
    public static event RestartCallback OnRestartCallback;

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
        // remove approval check callback from networking manager
        OnRestartCallback?.Invoke();

        if (IsHost)
        {
            // if the host disconnects, make sure to disconnect all clients
            int cCount = NetworkingManager.Singleton.ConnectedClients.Count;
            var clientIdList = new List<ulong>(NetworkingManager.Singleton.ConnectedClients.Keys);

            for (int i = 0; i < cCount; i++)
            {
                // make sure the client hasn't already disconnected
                if (NetworkingManager.Singleton.ConnectedClients.ContainsKey(clientIdList[i]))
                {
                    NetworkingManager.Singleton.DisconnectClient(clientIdList[i]);
                }
            }
          
            NetworkingManager.Singleton.StopHost();
        }
        else if (IsClient)
        {
            NetworkingManager.Singleton.StopClient();
        }
    }
}