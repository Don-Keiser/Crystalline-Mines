using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ButtonLight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Light2D _light2D;
    [SerializeField] private Button _button;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _light2D.enabled = true;
        _button.transform.localScale = new Vector3(1.25f, 1.25f, 1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _light2D.enabled = false;
        _button.transform.localScale = new Vector3(1.0f, 1.0f, 1f);

    }
}
