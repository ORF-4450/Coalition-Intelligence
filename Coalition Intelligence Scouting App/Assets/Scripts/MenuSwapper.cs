using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MenuSwapper : MonoBehaviour
{
    // Allows you to use this script with a dropdown
    [SerializeField] public TMP_Dropdown menuSelectionDropdown;
    // List of all menus, edited in inspector
    [SerializeField] public List<Menu> menus;

    private void Awake()
    {
        // Used to automatically change menus when the dropdown changes to the menu selected in the dropdown
        if (menuSelectionDropdown != null) {
            menuSelectionDropdown.onValueChanged.AddListener(delegate { ChangeMenu(menuSelectionDropdown.value); });
            UpdateDropdown();
        }
    }

    // Updates options of dropdown based on menu list
    //[EditorCools.Button]
    public void UpdateDropdown()
    {
        // Prevents running if there is no dropdown so it shows a warning instead of an error
        if (menuSelectionDropdown == null)
        {
            Debug.LogWarning("menuSelectionDropdown is null");
            return;
        }

        // Clears then adds options to prevent duplicates
        menuSelectionDropdown.ClearOptions();
        foreach (Menu menu in menus)
        { menuSelectionDropdown.options.Add(new TMP_Dropdown.OptionData(menu.name)); }
    }

    public void ChangeMenu(int menuIndex)
    {
        // Toggles every gameobjects active state that matters for the menu
        foreach (MenuPart menuPart in menus[menuIndex].menuParts)
        { menuPart.gameObject.SetActive(menuPart.isEnabled); }

        // Allows invoking the events that you you wanted invoked upon changing to that menu
        menus[menuIndex].onMenuSelect.Invoke();
    }
}

[System.Serializable]
public class Menu
{
    public string name;
    public List<MenuPart> menuParts;
    public UnityEvent onMenuSelect;
}

[System.Serializable]
public class MenuPart
{
    public GameObject gameObject;
    public bool isEnabled;
}