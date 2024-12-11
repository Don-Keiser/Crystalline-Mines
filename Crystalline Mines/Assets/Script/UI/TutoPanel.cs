using TMPro;
using UnityEngine;

public class UpdateTutoPanel : MonoBehaviour
{
    private bool _isActive;
    private GameObject _currentGo;
    private TextMeshProUGUI _canvasText;
    private GameObject _panel;

    private void Awake()
    {
        GetComponent();
        EventManager.ActiveTutoPanel += ShowPanel;
    }
    private void GetComponent()
    {
        _canvasText = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        _panel = gameObject.transform.GetChild(0).gameObject;
    }
    private void ShowPanel(GameObject go)
    {
        if(_currentGo == go || _currentGo == null || (!_isActive && _currentGo != go))
        {
            _isActive = !_isActive;
            _panel.SetActive(_isActive);
        }
        _currentGo = go;
        UpdatePanelPosition(go);
        UpdatePanelText(go);
    }
    private void UpdatePanelPosition(GameObject go)
    {
        transform.position = go.transform.position;
    }
    private void UpdatePanelText(GameObject go)
    {
        _canvasText.text = go.GetComponent<Interact_TutoPanel>().ExplanatoryText;
    }
}
