using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

[RequireComponent(typeof(Light2D), typeof(Button))]
public class ButtonLight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    private Light2D _light2D;
    private Button _button;
    private float x;
    private float y;
    
    public bool LightOn = true;
    private IPointerUpHandler _pointerUpHandlerImplementation;

    [Header("Color tint when cursor over Button")]
    [SerializeField] private Color _colorTint;

    private void Start()
    {
        _light2D = GetComponent<Light2D>();
        _button = GetComponent<Button>();
        
        x = _button.transform.localScale.x;
        y = _button.transform.localScale.y;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnter();
    }

    public void PointerEnter()
    {
        if (LightOn)
        {
            _light2D.enabled = true;
        }
        _button.transform.localScale = new Vector3(x + 0.25f, y + 0.25f, 1f);
        _light2D.color = _colorTint;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExit();
    }
    public void PointerExit()
    {
        if (LightOn)
        {
            _light2D.enabled = false;
        }
        _button.transform.localScale = new Vector3(x, y, 1f);
        _light2D.color = Color.white;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(LightOn)
        {
            _light2D.enabled = false;
        }
        _button.transform.localScale = new Vector3(x, y, 1f);
    }
}
