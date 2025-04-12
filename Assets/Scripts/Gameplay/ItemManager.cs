using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{

    [SerializeField] private Image[] slotImages;
    [SerializeField] private Sprite emptySlotSprite;
    [SerializeField] private Sprite[] itemSprites;
    [SerializeField] private Image selectionHighlight;
    [SerializeField] private GameObject itemEffectUIPrefab;
    [SerializeField] private Transform itemEffectUIContainer;

    private List<ItemType> items = new List<ItemType>();
    private int selectedIndex = 0;

    public static ItemManager Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        UpdateUI();
    }

    private void Update() {
        if (Time.timeScale == 0f) { return; }
        
        if (KeybindManager.Instance == null) { return; }

        if (Input.GetKeyDown(KeybindManager.Instance.GetKey(KeybindAction.MoveLeft))) {
            selectedIndex = Mathf.Max(selectedIndex - 1, 0);
            UpdateSelectorPosition();
        } else if (Input.GetKeyDown(KeybindManager.Instance.GetKey(KeybindAction.MoveRight))) {
            selectedIndex = Mathf.Min(selectedIndex + 1, items.Count - 1);
            UpdateSelectorPosition();
        }

        if (Input.GetKeyDown(KeybindManager.Instance.GetKey(KeybindAction.UseItem))) {
            UseSelected();
        } else if (Input.GetKeyDown(KeybindManager.Instance.GetKey(KeybindAction.DiscardItem))) {
            DiscardSelected();
        }
    }

    public bool IsFull() {
        return (items.Count >= 3);
    }

    public void AddItem(ItemType item) {
        if (IsFull()) { return; }
        items.Add(item);
        selectedIndex = items.Count - 1;
        UpdateUI();
    }

    private void UseSelected() {
        if (items.Count == 0) { return; }
        ItemType item = items[selectedIndex];

        ApplyItemEffect(item);
        items.RemoveAt(selectedIndex);
        selectedIndex = Mathf.Clamp(selectedIndex, 0, items.Count - 1);
        UpdateUI();
    }

    private void DiscardSelected(){
        if (items.Count == 0) { return; }

        items.RemoveAt(selectedIndex);
        selectedIndex = Mathf.Clamp(selectedIndex, 0, items.Count - 1);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.discardItem);
        UpdateUI();
    }

    private void ApplyItemEffect(ItemType item) {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.useItem);
        switch (item) {
            case ItemType.SlowTime:
                GameManager.Instance.SlowTimeEffect();
                ShowEffectUI("Slow Time", itemSprites[0], 15f);
                break;
            case ItemType.ExtraLife:
                GameManager.Instance.AddLife(1f);
                ShowEffectUI("Extra Life", itemSprites[1], 0f);
                AudioManager.Instance.PlaySFX(AudioManager.Instance.addHealth);
                break;
            case ItemType.DoublePoints:
                GameManager.Instance.DoublePointsEffect();
                ShowEffectUI("Double Points", itemSprites[2], 10f);
                break;
        }
    }

    private void ShowEffectUI(string name, Sprite icon, float duration) {
        if (duration <= 0f) { return; }

        GameObject ui = Instantiate(itemEffectUIPrefab, itemEffectUIContainer);
        ItemTimerUI uiScript = ui.GetComponent<ItemTimerUI>();
        uiScript.Init(icon, name, duration);
    }

    private void UpdateUI() {
        for (int i = 0; i < slotImages.Length; i++){
            if (i < items.Count) {
                slotImages[i].sprite = itemSprites[(int)items[i]];
            } else {
                slotImages[i].sprite = emptySlotSprite;
            }

            slotImages[i].enabled = true;
        }

        UpdateSelectorPosition();
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
