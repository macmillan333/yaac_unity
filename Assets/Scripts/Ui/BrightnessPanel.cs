using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrightnessPanel : MonoBehaviour
{
    public Image image;
    public Text brightnessText;
    private int brightness;  // [0, 20]

    void Start()
    {
        brightness = 20;
    }
    
    void Update()
    {
        brightnessText.text = brightness.ToString();
        float alpha = brightness * 0.05f;
        image.color = new Color(1f, 1f, 1f, alpha);
    }

    public void OnMinus()
    {
        brightness--;
        if (brightness < 0) brightness = 0;
    }

    public void OnPlus()
    {
        brightness++;
        if (brightness > 20) brightness = 20;
    }
}
