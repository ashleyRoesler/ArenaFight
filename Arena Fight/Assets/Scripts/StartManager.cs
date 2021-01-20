using UnityEngine.SceneManagement;
using MLAPI;
using UnityEngine;

public class StartManager : NetworkedBehaviour
{
    public void HostGame(int PC)
    {
        SceneManager.LoadScene("Arena");
        ArenaManager.BeHost(true, PC);
    }

    public void JoinGame(int PC)
    {
        SceneManager.LoadScene("Arena");
        ArenaManager.BeHost(false, PC);
    }
}