using UnityEngine;
using System.Collections.Generic;

public class ToggleSwitchGroupManager : MonoBehaviour
{
    [Header("Start Value")]
    [SerializeField] private ToggleSwitch initialToggleSwitch; // optional toggle to default on

    [Header("Toggle Options")]
    [SerializeField] private bool allCanBeToggledOff; // if true, allows all toggles to be off

    private List<ToggleSwitch> _toggleSwitches = new List<ToggleSwitch>(); // list of toggles in this group

    private void Awake()
    {
        // find all toggle switches under this object
        ToggleSwitch[] toggleSwitches = GetComponentsInChildren<ToggleSwitch>();
        foreach (var toggleSwitch in toggleSwitches)
        {
            RegisterToggleButtonToGroup(toggleSwitch); // hook them up
        }
    }

    private void RegisterToggleButtonToGroup(ToggleSwitch toggleSwitch)
    {
        if (_toggleSwitches.Contains(toggleSwitch))
            return;

        _toggleSwitches.Add(toggleSwitch); // store reference
        toggleSwitch.SetupForManager(this); // link back to this manager
    }

    private void Start()
    {
        bool areAllToggledOff = true;

        // check if any toggle is currently on
        foreach (var button in _toggleSwitches)
        {
            if (!button.CurrentValue) 
                continue;

            areAllToggledOff = false;
            break;
        }

        // if something is on or toggle-off is allowed, don’t touch anything
        if (!areAllToggledOff || allCanBeToggledOff) 
            return;

        // otherwise, force one toggle on
        if (initialToggleSwitch != null)
            initialToggleSwitch.ToggleByGroupManager(true);
        else
            _toggleSwitches[0].ToggleByGroupManager(true);
    }

    public void ToggleGroup(ToggleSwitch toggleSwitch)
    {
        if (_toggleSwitches.Count <= 1)
            return;

        // allow all off if enabled and clicked toggle is already on
        if (allCanBeToggledOff && toggleSwitch.CurrentValue)
        {
            foreach (var button in _toggleSwitches)
            {
                if (button == null)
                    continue;

                button.ToggleByGroupManager(false); // turn them all off
            }
        }
        else
        {
            // turn this one on, others off
            foreach (var button in _toggleSwitches)
            {
                if (button == null)
                    continue;

                if (button == toggleSwitch)
                    button.ToggleByGroupManager(true);
                else
                    button.ToggleByGroupManager(false);
            }
        }
    }
}
