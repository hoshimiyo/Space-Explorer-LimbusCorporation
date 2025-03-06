using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxValue(float maxValue)
    {
        slider.maxValue = maxValue;
        slider.value = 0;
    }

    public void SetValue(float value)
    {
        slider.value = value;
    }
}