using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorApplier : MonoBehaviour
{
    [SerializeField] public int colorIndex;

    void OnEnable()
    {
        ApplyColors(ColorHolder.instance.ColorOptions[ColorHolder.instance.currentColorIndex]);
    }

    void ApplyColors(ColorOption colorOption)
    {
        Color bufferColor;
        switch(colorIndex)
        {
            case 1:
            bufferColor = colorOption.primaryColor;
            break;

            case 2:
            bufferColor = colorOption.secondaryColor;
            break;

            case 3:
            bufferColor = colorOption.backgroundColor;
            break;

            default:
            Debug.Log("Color index " + colorIndex.ToString() + " is out of range.");
            bufferColor = Color.black;
            break;
        }

        if (gameObject.GetComponent<RawImage>() != null) gameObject.GetComponent<RawImage>().color = bufferColor;
        if (gameObject.GetComponent<Image>() != null) gameObject.GetComponent<Image>().color = bufferColor;

        if (gameObject.GetComponent<Camera>() != null) gameObject.GetComponent<Camera>().backgroundColor = bufferColor;
    }
}