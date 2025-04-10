using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class KeybindButton : MonoBehaviour
{
    [SerializeField] private KeybindAction action;
    [SerializeField] private TextMeshProUGUI label;

    private static List<KeybindButton> allButtons = new();

    private void Start() {
        Refresh();
    }

    private void Awake() {
        allButtons.Add(this);
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnEnable() {
        Refresh();
    }

    private void OnDestroy() {
        allButtons.Remove(this);
    }

    private void OnClick() {
        label.text = "Press any key...";
        KeybindManager.Instance.StartListeningForKey(action);
    }

    public void Refresh() {
        if (label == null && KeybindManager.Instance == null) {
            return;
        }

        KeyCode key = KeybindManager.Instance.GetKey(action);
        label.text = key.ToString();
        bool hasConflict = false;

        foreach (KeybindAction otherAction in System.Enum.GetValues(typeof(KeybindAction))) {
            if (otherAction == action) continue;
            if (KeybindManager.Instance.GetKey(otherAction) == key) {
                hasConflict = true;
                break;
            }
        }

        label.color = hasConflict ? Color.red : Color.black;
        }

    public static void RefreshAll() {
        foreach (var btn in allButtons) {
            btn.Refresh();
        }
    }
}
