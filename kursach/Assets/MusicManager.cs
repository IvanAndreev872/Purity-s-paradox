using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource BackgroundMusic;

    private bool isEnable = true;
    public void Update()
    {
        if (isEnable)
        {
            BackgroundMusic.enabled = true;
        }
        else
        {
            BackgroundMusic.enabled = false;
        }
    }
    public void EnableMusic()
    {
        isEnable = !isEnable;
    }
}
