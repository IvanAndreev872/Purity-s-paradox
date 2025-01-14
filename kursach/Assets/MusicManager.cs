using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public AudioSource BackgroundMusic;
    public Slider slider;
    public Sprite musicOn;
    public Sprite musicOff;
    public GameObject musicButton;
    public void Update()
    {
        AudioListener.volume = slider.value;
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
            AudioListener.volume = 1;
            slider.value = 1;
        }
        else
        {
            AudioListener.volume = 0;
            slider.value = 0;
        }
    }
    public void PlayMusic() {
        BackgroundMusic.enabled = true;
    }
}
