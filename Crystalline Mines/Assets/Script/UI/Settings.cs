using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public static Settings Instance;
    
    public bool settingsActive;
    public AudioSource audioSource;
    public bool isInOnDestroyOnLoad;

    [Header("UI Panels")]
    [SerializeField] public GameObject _settingsPanel;
    [SerializeField] private GameObject _videoPanel;
    [SerializeField] private GameObject _soundPanel;

    [Header("Video Settings")]
    [SerializeField] private Dropdown _resolutionDropdown;
    [SerializeField] private Toggle _fullScreenToggle;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer _audioMixer;

    [Header("Volume Settings")]
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private TextMeshProUGUI _masterVolumeText;
    [Space(10)]
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private TextMeshProUGUI _musicVolumeText;
    [Space(10)]
    [SerializeField] private Slider _uiVolumeSlider;
    [SerializeField] private TextMeshProUGUI _uiVolumeText;

    private void Awake()
    {
        Instance = Instantiator.ReturnInstance(this, Instantiator.InstanceConflictResolutions.DestructionOfTheSecondOneParent);
    }

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
        
        InitializeVolume("MasterVolume", _masterVolumeSlider, _masterVolumeText, "MasterVolumeValue");
        InitializeVolume("MusicVolume", _musicVolumeSlider, _musicVolumeText, "MusicVolumeValue");
        InitializeVolume("UIVolume", _uiVolumeSlider, _uiVolumeText, "UIVolumeValue");
    }

    private void InitializeVolume(string parameterName, Slider slider, TextMeshProUGUI volumeText, string playerPrefKey)
    {
        float savedVolume = PlayerPrefs.GetFloat(playerPrefKey, 0.5f);
        slider.value = savedVolume;
        AdjustVolume(parameterName, slider, volumeText);
    }

    public void Resume()
    {
        audioSource.Play();
        _settingsPanel.SetActive(false);
        _videoPanel.SetActive(false);
        _soundPanel.SetActive(false);
        settingsActive = false;
    }

    public void OpenVideoSettings()
    {
        audioSource.Play();
        _videoPanel.SetActive(true);
        _soundPanel.SetActive(false);
    }

    public void OpenSoundSettings()
    {
        audioSource.Play();
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
        AdjustVolume("MasterVolume", _masterVolumeSlider, _masterVolumeText, "MasterVolumeValue");
    }

    public void OnMusicVolumeChange()
    {
        AdjustVolume("MusicVolume", _musicVolumeSlider, _musicVolumeText, "MusicVolumeValue");
    }

    public void OnUiVolumeChange()
    {
        AdjustVolume("UIVolume", _uiVolumeSlider, _uiVolumeText, "UIVolumeValue");
    }

    private void AdjustVolume(string parameterName, Slider slider, TextMeshProUGUI volumeText, string playerPrefKey = "")
    {
        float volumeInDb = Mathf.Log10(slider.value) * 40 + 12;
        _audioMixer.SetFloat(parameterName, volumeInDb);
        volumeText.text = (slider.value * 100).ToString("0") + "%";
        
        if (!string.IsNullOrEmpty(playerPrefKey))
        {
            PlayerPrefs.SetFloat(playerPrefKey, slider.value);
            PlayerPrefs.Save();
        }
    }
    
        public void Default()
        {
            const float defaultMasterVolume = 0.5f;
            const float defaultMusicVolume = 0.5f;
            const float defaultUiVolume = 0.5f;
            const int defaultResolutionIndex = 0;
            const bool defaultFullScreen = true;
            
            _masterVolumeSlider.value = defaultMasterVolume;
            AdjustVolume("MasterVolume", _masterVolumeSlider, _masterVolumeText, "MasterVolumeValue");

            _musicVolumeSlider.value = defaultMusicVolume;
            AdjustVolume("MusicVolume", _musicVolumeSlider, _musicVolumeText, "MusicVolumeValue");

            _uiVolumeSlider.value = defaultUiVolume;
            AdjustVolume("UIVolume", _uiVolumeSlider, _uiVolumeText, "UIVolumeValue");
            
            _resolutionDropdown.value = defaultResolutionIndex;
            _resolutionDropdown.RefreshShownValue();
            SetResolution();
            
            _fullScreenToggle.isOn = defaultFullScreen;
            ToggleFullScreen();
            
            PlayerPrefs.SetFloat("MasterVolumeValue", defaultMasterVolume);
            PlayerPrefs.SetFloat("MusicVolumeValue", defaultMusicVolume);
            PlayerPrefs.SetFloat("UIVolumeValue", defaultUiVolume);
            PlayerPrefs.SetInt("ResolutionIndex", defaultResolutionIndex);
            PlayerPrefs.SetInt("FullScreen", defaultFullScreen ? 1 : 0);
            PlayerPrefs.Save();
            
        }
}
