using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;

    [SerializeField]
    private Image _fill;

    [SerializeField]
    private Gradient _gradient;

    public void SetMaxHealth(int health)
    {
        // set health
        _slider.maxValue = health;
        _slider.value = health;

        // change color
        _fill.color = _gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        // set health
        _slider.value = health;

        // change color
        _fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }
}