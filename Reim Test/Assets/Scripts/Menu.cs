using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField]
    Button ready1;          // reference to player 1's ready button

    [SerializeField]
    Button ready2;          // reference to player 2's ready button

    [SerializeField]
    Button ready3;          // reference to player 3's ready button

    int ready = 0;

    public void BeReady1()
    {
        ready++;
        ready1.interactable = false;

        if (ready == 3)
        {
            SceneManager.LoadScene("Arena");
        }
    }

    public void BeReady2()
    {
        ready++;
        ready2.interactable = false;

        if (ready == 3)
        {
            SceneManager.LoadScene("Arena");
        }
    }

    public void BeReady3()
    {
        ready++;
        ready3.interactable = false;

        if (ready == 3)
        {
            SceneManager.LoadScene("Arena");
        }
    }
}