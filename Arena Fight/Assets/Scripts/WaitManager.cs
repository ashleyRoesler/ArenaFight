using TMPro;
using UnityEngine;

public class WaitManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _waitingText;        // waiting for players/countdown timer text

    private float _time = 4.0f;                  // timer length in seconds
    private bool _countdown = false;             // true if countdown has begun

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
            _waitingText.text = "Waiting... " + current + "/" + needed + " players";
        }
        else // switch to fight text
        {
            _waitingText.text = "Get Ready!";
            _countdown = true;
        }
    }

    private void Update()
    {
        if (_countdown)
        {
            if (_time <= 3.0f && _time > 1.0f)
            {
                _waitingText.text = _time.ToString("0");
            }
            else if (_time <= 1.0f && _time >= 0.0f)
            {
                _waitingText.text = "Fight!";
                ArenaManager.GameHasStarted = true;
            }
            else if (_time < 0.0f)
            {
                gameObject.SetActive(false);
                return;
            }
            _time -= Time.deltaTime;
        }
    }
}