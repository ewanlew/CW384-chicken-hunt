using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))] // make sure a button is on this object
public class UIAudio : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData) {
        // play a click sound when any button with this script is clicked
        AudioManager.Instance?.PlaySFX(AudioManager.Instance.menuClickClip);
    }
}
