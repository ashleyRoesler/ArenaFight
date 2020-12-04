using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public Gradient gradient;

    public void SetMaxHealth(int health)
    {
        // set health
        slider.maxValue = health;
        slider.value = health;

        // change color
        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        // set health
        slider.value = health;

        // change color
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
