using UnityEngine;
using System.Collections.Generic;

public class KeybindManager : MonoBehaviour
{
    public static KeybindManager Instance; // singleton ref

    private Dictionary<KeybindAction, KeyCode> keybinds = new Dictionary<KeybindAction, KeyCode>(); // action → key
    private KeybindAction? listeningFor = null; // which key is being reassigned

    private void Awake() {
        if (Instance == null) { 
            Instance = this; 
        } else { 
            Destroy(gameObject); // kill extras
        }

        LoadKeybinds(); // pull saved or default bindings
    }

    void Update() {
        if (listeningFor != null) {
            // loop through all possible keycodes
            foreach (KeyCode code in System.Enum.GetValues(typeof(KeyCode))) {
                if (Input.GetKeyDown(code)) {
                    Debug.Log($"[KeybindManager] Detected key: {code}");
                    SetKey((KeybindAction) listeningFor, code); // bind the new key
                    listeningFor = null; // stop listening
                    break;
                }
            }
        }
    }

    public void StartListeningForKey(KeybindAction action) {
        listeningFor = action; // set which action we’re rebinding
    }

    public KeyCode GetKey(KeybindAction action) {
        return keybinds[action]; // get the current key for this action
    }

    public void SetKey(KeybindAction action, KeyCode key) {
        Debug.Log($"[KeybindManager] Setting {action} to {key}");
        keybinds[action] = key; // update in memory
        PlayerPrefs.SetString(action.ToString(), key.ToString()); // save
        PlayerPrefs.Save();

        KeybindButton.RefreshAll(); // update UI
    }

    private void LoadKeybinds() {
        foreach (KeybindAction action in System.Enum.GetValues(typeof(KeybindAction))) {
            string saved = PlayerPrefs.GetString(action.ToString(), GetDefaultKey(action).ToString()); // try get saved
            keybinds[action] = (KeyCode)System.Enum.Parse(typeof(KeyCode), saved); // apply
        }
    }

    private KeyCode GetDefaultKey(KeybindAction action) {
        // fallback default keys
        return action switch {
            KeybindAction.MoveLeft => KeyCode.A,
            KeybindAction.MoveRight => KeyCode.D,
            KeybindAction.UseItem => KeyCode.Space,
            KeybindAction.DiscardItem => KeyCode.F,
            _ => KeyCode.None
        };
    }
}
