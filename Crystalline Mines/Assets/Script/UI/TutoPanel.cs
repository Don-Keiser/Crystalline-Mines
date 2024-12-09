using TMPro;
using UnityEngine;

public class UpdateTutoPanel : MonoBehaviour
{
    private bool _isActive;
    private GameObject _currentGo;
    private string _canvasText;

    private void OnEnable()
    {
        EventManager.ActiveTutoPanel += UpdatePanelPos;
        EventManager.ActiveTutoPanel += UpdatePanelText;
        EventManager.ActiveTutoPanel += ShowPanel;

        GetTextComponent();
    }
    private void OnDisable()
    {
        EventManager.ActiveTutoPanel -= UpdatePanelPos;
        EventManager.ActiveTutoPanel -= UpdatePanelText;
        EventManager.ActiveTutoPanel -= ShowPanel;
    }

    private void GetTextComponent()
    {
        _canvasText = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text;
    }
    private void ShowPanel(GameObject go)
    {
        if(_currentGo == go || _currentGo is null || (!_isActive && _currentGo != go) )
        {
            _isActive = !_isActive;
            gameObject.transform.GetChild(0).gameObject.SetActive(_isActive);
        }
        _currentGo = go;
    }
    private void UpdatePanelPos(GameObject go)
    {
        transform.position = go.transform.position;
    }
    private void UpdatePanelText(GameObject go)
    {
        _canvasText = go.GetComponent<Interact_TutoPanel>().ExplanatoryText;
    }
}
