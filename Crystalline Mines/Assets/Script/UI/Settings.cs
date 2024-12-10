using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _videoPanel;
    [SerializeField] private GameObject _soundPanel;

    [Header("Video Settings")]
    [SerializeField] private Dropdown _resolutionDropdown;
    [SerializeField] private Toggle _fullScreenToggle;

    [Header("Volume Settings")]
    [SerializeField] private AudioSource _masterVolumeSource;
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private TextMeshProUGUI _masterVolumeText;
    [Space(10)]
    [SerializeField] private AudioSource _musicVolumeSource;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private TextMeshProUGUI _musicVolumeText;
    [Space(10)]
    [SerializeField] private AudioSource _uiVolumeSource;
    [SerializeField] private Slider _uiVolumeSlider;
    [SerializeField] private TextMeshProUGUI _uiVolumeText;

    private void Start()
    {
        InitializeSettings();
    }

    private void InitializeSettings()
    {
        _fullScreenToggle.isOn = Screen.fullScreen;
        
        int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
        _resolutionDropdown.value = savedResolutionIndex;
        _resolutionDropdown.RefreshShownValue();

        SetResolution();
        
        InitializeVolume(_masterVolumeSource, _masterVolumeSlider, _masterVolumeText);
        InitializeVolume(_musicVolumeSource, _musicVolumeSlider, _musicVolumeText);
        InitializeVolume(_uiVolumeSource, _uiVolumeSlider, _uiVolumeText);
    }

    private void InitializeVolume(AudioSource source, Slider slider, TextMeshProUGUI volumeText)
    {
        slider.value = source.volume;
        volumeText.text = (source.volume * 100).ToString("0") + "%";
    }

    public void Resume()
    {
        _settingsPanel.SetActive(false);
        _videoPanel.SetActive(false);
        _soundPanel.SetActive(false);
    }

    public void OpenVideoSettings()
    {
        _videoPanel.SetActive(true);
        _soundPanel.SetActive(false);
    }

    public void OpenSoundSettings()
    {
        _soundPanel.SetActive(true);
        _videoPanel.SetActive(false);
    }

    private void SetResolution()
    {
        Resolution[] resolutions = {
            new Resolution { width = 1920, height = 1080 },
            new Resolution { width = 1366, height = 768 },
            new Resolution { width = 1280, height = 720 },
            new Resolution { width = 1024, height = 768 },
            new Resolution { width = 640, height = 480 }
        };

        int selectedIndex = _resolutionDropdown.value;
        if (selectedIndex >= 0 && selectedIndex < resolutions.Length)
        {
            var selected = resolutions[selectedIndex];
            Screen.SetResolution(selected.width, selected.height, _fullScreenToggle.isOn);
            
            PlayerPrefs.SetInt("ResolutionIndex", selectedIndex);
            PlayerPrefs.Save();
        }
    }

    public void ToggleFullScreen()
    {
        Screen.fullScreen = _fullScreenToggle.isOn;
    }

    public void OnMasterVolumeChange()
    {
        AdjustVolume(_masterVolumeSource, _masterVolumeSlider, _masterVolumeText);
    }

    public void OnMusicVolumeChange()
    {
        AdjustVolume(_musicVolumeSource, _musicVolumeSlider, _musicVolumeText);
    }

    public void OnUiVolumeChange()
    {
        AdjustVolume(_uiVolumeSource, _uiVolumeSlider, _uiVolumeText);
    }

    private void AdjustVolume(AudioSource source, Slider slider, TextMeshProUGUI volumeText)
    {
        source.volume = slider.value;
        volumeText.text = (slider.value * 100).ToString("0") + "%";
    }
}
