using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource BackgroundMusic;
    [SerializeField] private Slider slider;
    [SerializeField] private Sprite musicOn;
    [SerializeField] private Sprite musicOff;
    [SerializeField] private GameObject musicButton;
    private float previousVolume = 1;
    void Awake()
    {
        ConfigData configData = ConfigManager.LoadConfig();
        slider.value = configData.volume;
    }
    void Start()
    {
        UpdateValue();   
    }
    public void UpdateValue()
    {
        AudioListener.volume = slider.value;
        if (slider.value > 0)
        {
            previousVolume = slider.value;
        }
        if (AudioListener.volume == 0) 
        {
            musicButton.GetComponent<Image>().sprite = musicOff;
        }
        else
        {
            musicButton.GetComponent<Image>().sprite = musicOn;
        }
    }
    public void EnableMusic()
    {
        if (AudioListener.volume == 0) {
            AudioListener.volume = previousVolume;
            slider.value = previousVolume;
        }
        else
        {
            AudioListener.volume = 0;
            slider.value = 0;
        }
    }
    public void SaveVolume()
    {
        ConfigData configData = ConfigManager.LoadConfig();
        configData.volume = AudioListener.volume;
        ConfigManager.SaveConfig(configData);
    }
}
