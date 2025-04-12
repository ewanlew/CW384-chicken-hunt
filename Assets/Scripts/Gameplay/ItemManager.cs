using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image[] slotImages; // visual item slots
    [SerializeField] private Sprite emptySlotSprite; // blank slot sprite
    [SerializeField] private Sprite[] itemSprites; // icon for each item type
    [SerializeField] private Image selectionHighlight; // UI highlight for current slot
    [SerializeField] private GameObject itemEffectUIPrefab; // prefab for active item display
    [SerializeField] private Transform itemEffectUIContainer; // where to spawn effect UI

    private List<ItemType> items = new List<ItemType>(); // stored items
    private int selectedIndex = 0; // current slot selected

    public static ItemManager Instance; // singleton reference

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject); // prevent duplicates
        }
    }

    private void Start() {
        UpdateUI(); // reflect inventory on start
    }

    private void Update() {
        if (Time.timeScale == 0f) { return; } // pause guard
        if (KeybindManager.Instance == null) { return; } // keybinds not ready yet

        // left movement
        if (Input.GetKeyDown(KeybindManager.Instance.GetKey(KeybindAction.MoveLeft))) {
            selectedIndex = Mathf.Max(selectedIndex - 1, 0);
            UpdateSelectorPosition();
        } 
        // right movement
        else if (Input.GetKeyDown(KeybindManager.Instance.GetKey(KeybindAction.MoveRight))) {
            selectedIndex = Mathf.Min(selectedIndex + 1, items.Count - 1);
            UpdateSelectorPosition();
        }

        // use item
        if (Input.GetKeyDown(KeybindManager.Instance.GetKey(KeybindAction.UseItem))) {
            UseSelected();
        } 
        // discard item
        else if (Input.GetKeyDown(KeybindManager.Instance.GetKey(KeybindAction.DiscardItem))) {
            DiscardSelected();
        }
    }

    public bool IsFull() {
        return (items.Count >= 3); // max inventory size
    }

    public void AddItem(ItemType item) {
        if (IsFull()) { return; }
        items.Add(item);
        selectedIndex = items.Count - 1; // always select newest
        UpdateUI();
    }

    private void UseSelected() {
        if (items.Count == 0) { return; }

        ItemType item = items[selectedIndex];
        ApplyItemEffect(item); // run item logic
        items.RemoveAt(selectedIndex); // remove it
        selectedIndex = Mathf.Clamp(selectedIndex, 0, items.Count - 1);
        UpdateUI();
    }

    private void DiscardSelected(){
        if (items.Count == 0) { return; }

        items.RemoveAt(selectedIndex);
        selectedIndex = Mathf.Clamp(selectedIndex, 0, items.Count - 1);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.discardItem); // toss sound
        UpdateUI();
    }

    private void ApplyItemEffect(ItemType item) {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.useItem); // common use sound

        switch (item) {
            case ItemType.SlowTime:
                GameManager.Instance.SlowTimeEffect();
                ShowEffectUI("Slow Time", itemSprites[0], 15f);
                break;

            case ItemType.ExtraLife:
                GameManager.Instance.AddLife(1f);
                ShowEffectUI("Extra Life", itemSprites[1], 0f); // no timer needed
                AudioManager.Instance.PlaySFX(AudioManager.Instance.addHealth);
                break;

            case ItemType.DoublePoints:
                GameManager.Instance.DoublePointsEffect();
                ShowEffectUI("Double Points", itemSprites[2], 10f);
                break;
        }
    }

    private void ShowEffectUI(string name, Sprite icon, float duration) {
        if (duration <= 0f) { return; } // don't show instant effects

        GameObject ui = Instantiate(itemEffectUIPrefab, itemEffectUIContainer);
        ItemTimerUI uiScript = ui.GetComponent<ItemTimerUI>();
        uiScript.Init(icon, name, duration); // start the countdown
    }

    private void UpdateUI() {
        for (int i = 0; i < slotImages.Length; i++){
            if (i < items.Count) {
                slotImages[i].sprite = itemSprites[(int)items[i]]; // show item
            } else {
                slotImages[i].sprite = emptySlotSprite; // empty it
            }

            slotImages[i].enabled = true;
        }

        UpdateSelectorPosition(); // update highlight
    }

    private void UpdateSelectorPosition() {
        if (items.Count == 0) {
            selectionHighlight.enabled = false;
        } else {
            selectionHighlight.enabled = true;
            selectionHighlight.rectTransform.position = slotImages[selectedIndex].rectTransform.position;
        }
    }
}
