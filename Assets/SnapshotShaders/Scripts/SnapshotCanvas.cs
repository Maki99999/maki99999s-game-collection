using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class SnapshotCanvas : MonoBehaviour
{
    [SerializeField]
    public Text filterEffectText;

    [SerializeField]
    public List<Graphic> graphics; 

    public void SetFilterProperties(SnapshotFilter filter)
    {
        filterEffectText.text = filter.GetName();

        foreach(var graphic in graphics)
        {
            graphic.color = filter.GetColor();
        }
    }
}
