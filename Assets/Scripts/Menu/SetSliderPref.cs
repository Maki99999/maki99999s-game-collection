using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSliderPref : MonoBehaviour
{
    public string volumeName;

    void OnEnable()
    {
        GetComponent<Slider>().value = PlayerPrefs.GetFloat(volumeName, 0);
    }
}
