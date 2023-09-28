using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MenuSelector : MonoBehaviour
{
    [SerializeField] private MenuSwapper menuSwapper;
    [SerializeField] private ColorHolder colorHolder;

    [SerializeField] public List<MenuButton> menuButtons = new();

    // Start is called before the first frame update
    void Start()
    {
        foreach (MenuButton mb in menuButtons)
        {
            mb.button.onClick.AddListener(() => ChangeMenu(mb.menuIndex));
        }

        FixColors();
    }

    void ChangeMenu(int ID)
    {
        foreach (MenuButton mb in menuButtons)
        {
            mb.button.interactable = (ID != mb.menuIndex);
        }
        menuSwapper.ChangeMenu(ID);
    }

    void Destroy()
    {
        foreach (MenuButton mb in menuButtons)
        {
            mb.button.onClick.RemoveListener(() => ChangeMenu(mb.menuIndex));
        }
    }

    /*void AddChildren()
    {

    }*/

    [EditorCools.Button]
    void FixColors()
    {
        
        foreach (MenuButton mb in menuButtons)
        {
            mb.button.gameObject.GetComponent<RawImage>().color = colorHolder.buttonColor;
        }
    }
}

[Serializable]
public class MenuButton
{
    public Button button;
    public int menuIndex;
}