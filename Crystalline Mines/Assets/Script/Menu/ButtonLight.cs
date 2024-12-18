using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

[RequireComponent(typeof(Light2D), typeof(Button))]
public class ButtonLight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Light2D _light2D;
    private Button _button;
    private float x;
    private float y;
    
    public bool LightOn = true;

    private void Start()
    {
        _light2D = GetComponent<Light2D>();
        _button = GetComponent<Button>();
        
        x = _button.transform.localScale.x;
        y = _button.transform.localScale.y;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(LightOn)
        {
            _light2D.enabled = true;
        }
        _button.transform.localScale = new Vector3(x + 0.25f, y + 0.25f, 1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(LightOn)
        {
            _light2D.enabled = false;
        }
        _button.transform.localScale = new Vector3(x, y, 1f);

    }
    
    
}
