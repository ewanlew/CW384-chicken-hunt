using UnityEngine;
using System.Collections.Generic;

public class KeybindManager : MonoBehaviour
{
    public static KeybindManager Instance;

    private Dictionary<KeybindAction, KeyCode> keybinds = new Dictionary<KeybindAction, KeyCode>();
    private KeybindAction? listeningFor = null;

    private void Awake() {
        if (Instance == null) { 
            Instance = this; 
        } else { 
            Destroy(gameObject);
        }

        LoadKeybinds();
    }

    void Update() {
        if (listeningFor != null) {
            foreach (KeyCode code in System.Enum.GetValues(typeof(KeyCode))) {
                if (Input.GetKeyDown(code)) {
                Debug.Log($"[KeybindManager] Detected key: {code}");
                    SetKey((KeybindAction) listeningFor, code);
                    listeningFor = null;
                    break;
                }
            }
        }
    }

    public void StartListeningForKey(KeybindAction action) {
        listeningFor = action;
    }

    public KeyCode GetKey(KeybindAction action) {
        return keybinds[action]; // THIS ONE
    }

    public void SetKey(KeybindAction action, KeyCode key) {
        Debug.Log($"[KeybindManager] Setting {action} to {key}");
        keybinds[action] = key;
        PlayerPrefs.SetString(action.ToString(), key.ToString());
        PlayerPrefs.Save();

        KeybindButton.RefreshAll();
    }

    private void LoadKeybinds() {
        foreach (KeybindAction action in System.Enum.GetValues(typeof(KeybindAction))) {
            string saved = PlayerPrefs.GetString(action.ToString(), GetDefaultKey(action).ToString());
            keybinds[action] = (KeyCode)System.Enum.Parse(typeof(KeyCode), saved);
        }
    }

    private KeyCode GetDefaultKey(KeybindAction action) {
        return action switch {
            KeybindAction.MoveLeft => KeyCode.A,
            KeybindAction.MoveRight => KeyCode.D,
            KeybindAction.UseItem => KeyCode.Space,
            KeybindAction.DiscardItem => KeyCode.F,
            _ => KeyCode.None
        };
    }
}
