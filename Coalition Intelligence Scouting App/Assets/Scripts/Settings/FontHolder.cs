using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class FontHolder : MonoBehaviour
{
    public static FontHolder instance;

    [SerializeField] public List<TMP_FontAsset> FontOptions;

    [SerializeField] public int currentIndex = 1;
    [SerializeField] public GameObject canvas;
    [SerializeField] public TMP_Dropdown fontSelector;

    void Awake()
    {
        instance = this;
        // SetFont();
    }

    public void SetFont()
    {
        instance.currentIndex = fontSelector.value;
        canvas.BroadcastMessage("ApplyFont",SendMessageOptions.DontRequireReceiver);
    }

    public void SetFont(int fontIndex)
    {
        instance.currentIndex = fontIndex;
        canvas.BroadcastMessage("ApplyFont",SendMessageOptions.DontRequireReceiver);
    }
}
