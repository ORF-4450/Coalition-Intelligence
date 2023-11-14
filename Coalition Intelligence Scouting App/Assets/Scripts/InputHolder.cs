using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHolder : MonoBehaviour
{
    public RectTransform rt;
    public int height;
    // Start is called before the first frame update
    void Start()
    {
        rt = gameObject.GetComponent<RectTransform>();
        ChangeSize();
    }

    [EditorCools.Button]
    void ChangeSize()
    {
        rt.sizeDelta = new Vector2(0, height); //2nd value is height of scouting menu
        gameObject.transform.localPosition = new Vector3(0,-1 * rt.sizeDelta.y/2,0);
    }
}
