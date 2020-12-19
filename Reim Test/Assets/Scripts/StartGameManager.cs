using UnityEngine.SceneManagement;
using MLAPI;
using UnityEngine;

public class StartGameManager : NetworkedBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void HostGame()
    {
        SceneManager.LoadScene("Arena");
        ArenaManager.BeHost(true);
    }

    public void JoinGame()
    {
        SceneManager.LoadScene("Arena");
        ArenaManager.BeHost(false);
    }
}