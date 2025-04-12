using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ItemTimerUI : MonoBehaviour
{
    [SerializeField] private Image iconImage; // icon for the item
    [SerializeField] private TextMeshProUGUI nameText; // label for item name
    [SerializeField] private TextMeshProUGUI timerText; // time remaining display

    private float duration; // total time
    private float remaining; // time left

    public void Init(Sprite icon, string itemName, float sec) {
        iconImage.sprite = icon; // set icon image
        nameText.text = itemName; // set label
        duration = remaining = sec; // start from full duration
        StartCoroutine(UpdateTimer()); // begin countdown
    }

    private IEnumerator UpdateTimer() {
        while (remaining > 0f){
            if (!PauseState.IsGamePaused && !TypingState.IsUserTyping){
                remaining -= Time.unscaledDeltaTime; // tick timer in realtime
                timerText.text = FormatTime(remaining); // update display
            }
            yield return null; // wait a frame
        }
        Destroy(gameObject); // auto-remove when done
    }

    private string FormatTime(float time) {
        int t = Mathf.CeilToInt(time); // round up
        return $"0:{t:00}"; // format as mm:ss (ish)
    }
}
