using UnityEngine;
using UnityEngine.SceneManagement;
using MLAPI;

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
            NetworkingManager.Singleton.StopHost();
        }
        else if (IsClient)
        {
            NetworkingManager.Singleton.StopClient();
        }
    }
}