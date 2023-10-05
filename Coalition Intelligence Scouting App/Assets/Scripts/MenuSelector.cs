using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MenuSelector : MonoBehaviour
{
    [SerializeField] private MenuSwapper menuSwapper;
    [SerializeField] private ColorHolder colorHolder;

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
    }

    void ChangeMenu(int ID)
    {
        foreach (MenuButton mb in menuButtons)
        {
            mb.button.interactable = (ID != mb.menuIndex);
        }
        menuSwapper.ChangeMenu(ID);
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

            float x = (mb.menuIndex - Mathf.Round((float)menuButtons.Count / 2));
            mb.buttonObject.transform.localPosition = new Vector3 (x * 100,0,0);

            mb.buttonObject.name = menuSwapper.gameObject.transform.GetChild(i).name;
            mb.name = menuSwapper.gameObject.transform.GetChild(i).name;
            mb.button = mb.buttonObject.GetComponent<Button>();
            mb.menuIndex = i;
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
            mb.button.gameObject.GetComponent<RawImage>().color = colorHolder.buttonColor;
            mb.button.gameObject.GetComponent<RawImage>().texture = mb.icon;
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