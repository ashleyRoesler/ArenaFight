using TMPro;
using UnityEngine;

public class Waiting : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI waitingText;

    private float time = 4.0f;
    private bool countdown = false;

    private void OnEnable()
    {
        ArenaManager.OnUpdateWait += UpdateText;
    }

    private void OnDisable()
    {
        ArenaManager.OnUpdateWait -= UpdateText;
    }

    private void UpdateText(int current, int needed)
    {
        if (current != needed)
        {
            waitingText.text = "Waiting... " + current + "/" + needed + " players";
        }
        else // switch to fight text
        {
            waitingText.text = "Get Ready!";
            countdown = true;
        }
    }

    private void Update()
    {
        if (countdown)
        {
            if (time <= 3.0f && time > 0.0f)
            {
                waitingText.text = time.ToString("0");
            }
            else if (time <= 0.0f && time >= -1.0f)
            {
                waitingText.text = "Fight!";
                ArenaManager.gameHasStarted = true;
            }
            else if (time < -1.0f)
            {
                gameObject.SetActive(false);
                return;
            }
            time -= Time.deltaTime;
        }
    }
}