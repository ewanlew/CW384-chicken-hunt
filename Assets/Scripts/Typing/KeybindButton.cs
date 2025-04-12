using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class KeybindButton : MonoBehaviour
{
    [SerializeField] private KeybindAction action; // which input this button changes
    [SerializeField] private TextMeshProUGUI label; // shows current key

    private static List<KeybindButton> allButtons = new(); // track all keybind buttons

    private void Start() {
        Refresh(); // update label when loaded
    }

    private void Awake() {
        allButtons.Add(this); // register self
        GetComponent<Button>().onClick.AddListener(OnClick); // hook up button event
    }

    private void OnEnable() {
        Refresh(); // update label if re-enabled
    }

    private void OnDestroy() {
        allButtons.Remove(this); // clean up if destroyed
    }

    private void OnClick() {
        label.text = "Press any key..."; // show prompt
        KeybindManager.Instance.StartListeningForKey(action); // tell manager to wait for key
    }

    public void Refresh() {
        if (label == null && KeybindManager.Instance == null) {
            return; // skip if not ready
        }

        KeyCode key = KeybindManager.Instance.GetKey(action); // get current key
        label.text = key.ToString(); // update text

        bool hasConflict = false;

        // check for duplicates
        foreach (KeybindAction otherAction in System.Enum.GetValues(typeof(KeybindAction))) {
            if (otherAction == action) continue;
            if (KeybindManager.Instance.GetKey(otherAction) == key) {
                hasConflict = true;
                break;
            }
        }

        // colour red if used elsewhere
        label.color = hasConflict ? Color.red : Color.black;
    }

    public static void RefreshAll() {
        // force all buttons to refresh their labels
        foreach (var btn in allButtons) {
            btn.Refresh();
        }
    }
}
