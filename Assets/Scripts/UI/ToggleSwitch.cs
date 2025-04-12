using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour, IPointerClickHandler
{
    [Header("Slider setup")] 
    [SerializeField, Range(0, 1f)]
    protected float sliderValue; // actual position of the slider knob
    public bool CurrentValue { get; private set; } // true = on, false = off

    private bool _previousValue;
    private Slider _slider;

    [Header("Animation")] 
    [SerializeField, Range(0, 1f)] private float animationDuration = 0.5f; // time to animate toggle
    [SerializeField] private AnimationCurve slideEase =
        AnimationCurve.EaseInOut(0, 0, 1, 1); // easing for smooth movement

    private Coroutine _animateSliderCoroutine;

    [Header("Events")] 
    [SerializeField] private UnityEvent onToggleOn; // called when toggled on
    [SerializeField] private UnityEvent onToggleOff; // called when toggled off

    private ToggleSwitchGroupManager _toggleSwitchGroupManager; // optional group manager

    protected Action transitionEffect; // extra thing to do during movement

    protected virtual void OnValidate()
    {
        SetupToggleComponents(); // rebind slider in editor
        _slider.value = sliderValue; // apply initial value
    }

    private void SetupToggleComponents()
    {
        if (_slider != null)
            return;

        SetupSliderComponent(); // do setup once
    }

    private void SetupSliderComponent()
    {
        _slider = GetComponent<Slider>(); // get attached slider

        if (_slider == null)
        {
            Debug.Log("No slider found!", this); // warn if missing
            return;
        }

        _slider.interactable = false; // block manual interaction
        var sliderColors = _slider.colors;
        sliderColors.disabledColor = Color.white; // make sure it's visible
        _slider.colors = sliderColors;
        _slider.transition = Selectable.Transition.None; // no transition effects
    }

    public void SetupForManager(ToggleSwitchGroupManager manager)
    {
        _toggleSwitchGroupManager = manager; // allow manager to control
    }

    protected virtual void Awake()
    {
        SetupSliderComponent(); // make sure it's all hooked up
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Toggle(); // user clicked
    }

    private void Toggle()
    {
        if (_toggleSwitchGroupManager != null)
            _toggleSwitchGroupManager.ToggleGroup(this); // use manager logic
        else
            SetStateAndStartAnimation(!CurrentValue); // otherwise just flip
    }

    public void ToggleByGroupManager(bool valueToSetTo)
    {
        SetStateAndStartAnimation(valueToSetTo); // called by manager to control value
    }

    public void SetStateAndStartAnimation(bool state)
    {
        _previousValue = CurrentValue;
        CurrentValue = state;

        // invoke correct event
        if (_previousValue != CurrentValue)
        {
            if (CurrentValue)
                onToggleOn?.Invoke();
            else
                onToggleOff?.Invoke();
        }

        // restart animation
        if (_animateSliderCoroutine != null)
            StopCoroutine(_animateSliderCoroutine);

        _animateSliderCoroutine = StartCoroutine(AnimateSlider());
    }

    private IEnumerator AnimateSlider()
    {
        float startValue = _slider.value;
        float endValue = CurrentValue ? 1 : 0;

        float time = 0;
        if (animationDuration > 0)
        {
            while (time < animationDuration)
            {
                time += Time.deltaTime;

                float lerpFactor = slideEase.Evaluate(time / animationDuration); // curve based easing
                _slider.value = sliderValue = Mathf.Lerp(startValue, endValue, lerpFactor);

                transitionEffect?.Invoke(); // call transition event
                        
                yield return null;
            }
        }

        _slider.value = endValue; // snap to end at finish
    }
}
