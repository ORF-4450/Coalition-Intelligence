using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FontApplier : MonoBehaviour
{
    private TextMeshProUGUI tmp;
    public void Awake()
    {
        tmp = gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void OnEnable()
    {
        ApplyFont();
    }
    public void ApplyFont()
    {
        tmp.font = FontHolder.instance.FontOptions[FontHolder.instance.currentIndex];
    }
}
