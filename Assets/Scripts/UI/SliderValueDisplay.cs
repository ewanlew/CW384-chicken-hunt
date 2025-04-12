using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderValueDisplay : MonoBehaviour {
    public Slider slider;
    public TextMeshProUGUI valueLabel;
    public bool showAsPercent = true;
    public string suffix = "%";

    private void Start() {
        if (slider != null) {
            UpdateText(slider.value);
            slider.onValueChanged.AddListener(UpdateText);
        }
    }

    private void UpdateText(float value) {
        string display;
        if (showAsPercent) {
            int percent = Mathf.RoundToInt(value * 100f);
            display = percent + suffix;
        } else {
            display = Mathf.RoundToInt(value) + suffix;
        }

        valueLabel.text = display;
    }
}
