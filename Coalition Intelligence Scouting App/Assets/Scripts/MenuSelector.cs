using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MenuSelector : MonoBehaviour
{
    [SerializeField] private MenuSwapper menuSwapper;
    [SerializeField] public ColorHolder CH;

    [SerializeField] private GameObject prefab;

    [SerializeField] public List<MenuButton> menuButtons = new();

    // Start is called before the first frame update
    void Start()
    {
        ReloadIcons();
        foreach (MenuButton mb in menuButtons)
        {
            mb.button.onClick.AddListener(() => ChangeMenu(mb.menuIndex));
        }
        ChangeMenu(menuSwapper.defaultMenu,false);
    }

    void ChangeMenu(int ID)
    {
        foreach (MenuButton mb in menuButtons)
        {
            mb.button.interactable = (ID != mb.menuIndex);
        }
        menuSwapper.ChangeMenu(ID);
    }

    void ChangeMenu(int ID, bool changeMenu)
    {
        foreach (MenuButton mb in menuButtons)
        {
            mb.button.interactable = (ID != mb.menuIndex);
        }
        if (changeMenu) menuSwapper.ChangeMenu(ID);
    }

    [EditorCools.Button]
    void CreateButtons()
    {
        for (var i = gameObject.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(gameObject.transform.GetChild(i).gameObject);
        }

        menuButtons.Clear();

        for (var i = 0; i <= menuSwapper.gameObject.transform.childCount - 1; i++)
        {
            MenuButton mb = new();
            menuButtons.Add(mb);
            mb.buttonObject = Instantiate(prefab, gameObject.transform);
            
            mb.menuIndex = i;
            mb.buttonObject.transform.localPosition = new Vector3 (((menuSwapper.gameObject.transform.childCount - 1) * -50) + (i * 100),0,0);

            mb.buttonObject.name = menuSwapper.gameObject.transform.GetChild(i).name;
            mb.name = menuSwapper.gameObject.transform.GetChild(i).name;
            mb.button = mb.buttonObject.GetComponent<Button>();
        }

        ReloadIcons();
        Debug.Log("Created Buttons");
    }

    void Destroy()
    {
        foreach (MenuButton mb in menuButtons)
        {
            mb.button.onClick.RemoveListener(() => ChangeMenu(mb.menuIndex));
        }
    }

    [EditorCools.Button]
    void ReloadIcons()
    {
        
        foreach (MenuButton mb in menuButtons)
        {
            mb.button.gameObject.GetComponent<ColorApplier>().colorIndex = 1;
            mb.button.gameObject.GetComponent<RawImage>().texture = mb.icon;
            mb.buttonObject.SetActive(true);
        }
    }
}

[Serializable]
public class MenuButton
{
    public string name;
    [HideInInspector] public Button button;
    [HideInInspector] public GameObject buttonObject;
    public int menuIndex;

    public Texture icon;
}