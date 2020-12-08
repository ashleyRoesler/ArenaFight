using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReadyMenu : MonoBehaviour
{
    [SerializeField]
    private Button ready1;          // reference to player 1's ready button

    [SerializeField]
    private Button ready2;          // reference to player 2's ready button

    [SerializeField]
    private Button ready3;          // reference to player 3's ready button

    private int ready = 0;

    public void OnEnable()
    {
        ready = 0;
        ready1.interactable = true;
        ready2.interactable = true;
        ready3.interactable = true;
    }

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