using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MenuSwapper : MonoBehaviour
{
    [SerializeField] public TMP_Dropdown menuSelectionDropdown;
    [SerializeField] public List<Menu> menus = new();

    private void Awake()
    {
        if (menuSelectionDropdown != null)
        {
            // Used to automatically change menus when the dropdown changes to the menu selected in the dropdown
            menuSelectionDropdown.onValueChanged.AddListener(delegate { ChangeMenu(menuSelectionDropdown.value); });

            UpdateDropdown();
        }
    }

    // Updates options of dropdown based on menu list
    [EditorCools.Button]
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

    [EditorCools.Button]
    public void AddChildren()
    {
        int initialSize = menus.Count;

        for (int a = 0; a < transform.childCount; a++)
        {
            menus.Add(new Menu());
            // Sets it's name to the gameObject to more easily discern which one it is
            menus[a + initialSize].name = transform.GetChild(a).name;

            for (int b = 0; b < transform.childCount; b++)
            {
                GameObject child = transform.GetChild(b).gameObject;
                // Adds the MenuParts to disable the other child menus and enable this one
                menus[a + initialSize].menuParts.Add(new MenuPart()
                { gameObject = child, isEnabled = child.name == menus[a + initialSize].name });
            }
        }

        UpdateDropdown();
    }
}

[System.Serializable]
public class Menu
{
    public string name;
    public List<MenuPart> menuParts = new();
    public UnityEvent onMenuSelect;
}

[System.Serializable]
public class MenuPart
{
    public GameObject gameObject;
    public bool isEnabled;
}