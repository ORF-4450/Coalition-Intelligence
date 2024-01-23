using TMPro;
using UnityEngine;

public class GeneralVariable : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameVisualizer;
    public string infoName { get => nameVisualizer.text; set => nameVisualizer.text = value; }
    public string infoKey;
}

