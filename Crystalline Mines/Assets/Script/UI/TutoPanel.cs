using System.Collections;
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

    [Header("Type writer effect")]
    [SerializeField] private float _delay;
    [SerializeField] private string _fullText;
    [SerializeField] private string _currentText = "";

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
        ResetText();
        if (_currentGo == go || _currentGo == null || (!_isActive && _currentGo != go))
        {
            _isActive = !_isActive;
            _panel.SetActive(_isActive);
        }
        _currentGo = go;
        _tutoPanel = _currentGo.GetComponent<Interact_TutoPanel>();

        ShowAndUpdateRightComponent();
    }

    private void HidePanel(GameObject go)
    {
        if (!_isActive) { return; }

        _isActive = false;
        _panel.SetActive(false);
        ResetText();
    }
    private void ShowAndUpdateRightComponent()
    {
        if (_tutoPanel.ExplanatoryText != string.Empty) //set Text
        {
            _canvasImage.gameObject.SetActive(false);
            _canvasText.gameObject.SetActive(true);
            UpdatePanelText();

        }
        else if (_tutoPanel.ExplanatoryImage != null) // set Image
        {
            _canvasText.gameObject.SetActive(false);
            _canvasImage.gameObject.SetActive(true);
            UpdatePanelImage();
        }
    }
    private void UpdatePanelText()
    {
        _fullText = _tutoPanel.ExplanatoryText;
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        for (int i = 0; 0 < _fullText.Length; i++)
        {
            if (!_panel.activeSelf) { ResetText(); break; }
            if(_currentText.Length == _fullText.Length) { break; }

            _currentText = _fullText.Substring(0, i);
            _canvasText.text = _currentText;
            yield return new WaitForSeconds(_delay);
        }
    }

    private void ResetText()
    {
        _fullText = string.Empty;
        _currentText = string.Empty;
        _canvasText.text = string.Empty;
    }
    private void UpdatePanelImage()
    {
        _canvasImage.sprite = _tutoPanel.ExplanatoryImage;
    }
}

