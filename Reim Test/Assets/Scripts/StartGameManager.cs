using UnityEngine.SceneManagement;
using MLAPI;

public class StartGameManager : NetworkedBehaviour
{
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