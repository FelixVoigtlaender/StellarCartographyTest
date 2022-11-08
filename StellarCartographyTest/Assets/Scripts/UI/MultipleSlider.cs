using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultipleSlider : MonoBehaviour
{
    public Slider templateSlider;

    private List<Slider> _sliders = new List<Slider>();

    public void Start()
    {
        templateSlider.gameObject.SetActive(false);
    }

    public Slider AddSlider(Color color)
    {
        GameObject sliderObj = Instantiate(templateSlider.gameObject, templateSlider.transform.parent);
        sliderObj.SetActive(true);
        Slider slider = sliderObj.GetComponent<Slider>();
        slider.fillRect.GetComponent<Image>().color = color;
        _sliders.Add(slider);
        
        slider.transform.SetAsFirstSibling();
        return slider;
    }

}
