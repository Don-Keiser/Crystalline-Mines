using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTutoPanel : MonoBehaviour
{
    private bool _isActive;
    private GameObject _currentGo;

    [SerializeField] private TextMeshProUGUI _canvasText;
    [SerializeField] private Image _canvasImage;
    [SerializeField] private GameObject _panel;

    private Interact_TutoPanel _tutoPanel;

    private void Awake()
    {
        GetComponent();
        EventManager.ActiveTutoPanel += ShowPanel;
        EventManager.DisableTutoPanel += HidePanel;
    }
    private void GetComponent()
    {
        if (_canvasText != null && _panel != null && _canvasImage != null) { return; }

        _panel = gameObject.transform.GetChild(0).gameObject;

        _canvasText = _panel.GetComponentInChildren<TextMeshProUGUI>();
        _canvasImage = _panel.GetComponentInChildren<Image>();
    }
    private void ShowPanel(GameObject go)
    {
        if (_currentGo == go || _currentGo == null || (!_isActive && _currentGo != go))
        {
            _isActive = !_isActive;
            _panel.SetActive(_isActive);
        }
        _currentGo = go;
        _tutoPanel = _currentGo.GetComponent<Interact_TutoPanel>();

        ShowAndUpdateRightComponent(go);
        UpdatePanelPosition(go);
    }

    private void HidePanel(GameObject go)
    {
        if (!_isActive) { return; }

        _isActive = false;
        _panel.SetActive(false);
    }
    private void UpdatePanelPosition(GameObject go)
    {
        transform.position = go.transform.position;
    }
    private void ShowAndUpdateRightComponent(GameObject go)
    {
        if (_tutoPanel.ExplanatoryText != string.Empty) //set Text
        {
            _canvasImage.gameObject.SetActive(false);
            _canvasText.gameObject.SetActive(true);
            UpdatePanelText(go);

        }
        else if (_tutoPanel.ExplanatoryImage != null) // set Image
        {
            _canvasText.gameObject.SetActive(false);
            _canvasImage.gameObject.SetActive(true);
            UpdatePanelImage(go);
        }
    }
    private void UpdatePanelText(GameObject go)
    {
        _canvasText.text = _tutoPanel.ExplanatoryText;
    }
    private void UpdatePanelImage(GameObject go)
    {
        _canvasImage.sprite = _tutoPanel.ExplanatoryImage;
    }
}

