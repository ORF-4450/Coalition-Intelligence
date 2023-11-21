using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorApplier : MonoBehaviour
{
    [SerializeField] public ColorHolder CH;
    [SerializeField] public int colorIndex;

    // Start is called before the first frame update
    void Awake()
    {
        ApplyColors();
    }

[EditorCools.Button]
    public void ApplyColors()
    {
        if (gameObject.GetComponent<RawImage>() != null) gameObject.GetComponent<RawImage>().color = SelectedColor(colorIndex);

        if (gameObject.GetComponent<Camera>() != null) gameObject.GetComponent<Camera>().backgroundColor = SelectedColor(colorIndex);
    }

    Color SelectedColor(int Index)
    {
        Color bufferColor;
        switch(Index)
        {
            case 1:
            bufferColor = CH.currentColor.primaryColor;
            break;

            case 2:
            bufferColor = CH.currentColor.secondaryColor;
            break;

            case 3:
            bufferColor = CH.currentColor.backgroundColor;
            break;

            default:
            Debug.Log("Color index " + Index.ToString() + " is out of range.");
            bufferColor = Color.black;
            break;
        }
        return bufferColor;
    }
}
