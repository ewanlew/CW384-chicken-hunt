using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ItemTimerUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI timerText;

    private float duration;
    private float remaining;
    public void Init(Sprite icon, string itemName, float sec) {
        iconImage.sprite = icon;
        nameText.text = itemName;
        duration = remaining = sec;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer() {
        while (remaining > 0f){
            remaining -= Time.deltaTime;
            timerText.text = FormatTime(remaining);
            yield return null;
        }
        Destroy(gameObject);
    }

    private string FormatTime(float time) {
        int t = Mathf.CeilToInt(time);
        return $"0:{t:00}";
    }

}
