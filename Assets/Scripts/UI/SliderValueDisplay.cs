using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderValueDisplay : MonoBehaviour {
    public Slider slider; // reference to the slider component
    public TextMeshProUGUI valueLabel; // text to show value
    public bool showAsPercent = true; // toggle between percent or raw value
    public string suffix = "%"; // extra character to append

    private void Start() {
        if (slider != null) {
            UpdateText(slider.value); // set initial display
            slider.onValueChanged.AddListener(UpdateText); // hook up listener
        }
    }

    private void UpdateText(float value) {
        string display;

        if (showAsPercent) {
            int percent = Mathf.RoundToInt(value * 100f); // convert to percentage
            display = percent + suffix;
        } else {
            display = Mathf.RoundToInt(value) + suffix; // use raw value
        }

        valueLabel.text = display; // update label
    }
}
