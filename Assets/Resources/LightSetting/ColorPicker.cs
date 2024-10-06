using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    public Image colorDisplay;
    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;

    void Start()
    {
        UpdateColor();
        redSlider.onValueChanged.AddListener(delegate { UpdateColor(); });
        greenSlider.onValueChanged.AddListener(delegate { UpdateColor(); });
        blueSlider.onValueChanged.AddListener(delegate { UpdateColor(); });
    }

    void UpdateColor()
    {
        Color color = new Color(redSlider.value, greenSlider.value, blueSlider.value);
        colorDisplay.color = color;
    }
}
