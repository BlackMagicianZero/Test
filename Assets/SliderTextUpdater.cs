using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderTextUpdater : MonoBehaviour
{
    public Slider slider;
    public TMP_Text text;
    
    void Start()
    {
        slider.onValueChanged.AddListener(delegate { UpdateText(); });

        UpdateText();    
    }

    // Update is called once per frame
    void UpdateText()
    {
        text.text = (slider.value * 1f).ToString("P0");
    }
}
