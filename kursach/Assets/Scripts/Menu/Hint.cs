using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hint : MonoBehaviour
{
    [SerializeField] private bool isShowed;
    void Start()
    {
        ConfigData configData = ConfigManager.LoadConfig();
        bool hasShowed = configData.hasGuideShowed;
        if (hasShowed)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            isShowed = false;
            Time.timeScale = 1f;
        }
        else
        {
            configData.hasGuideShowed = !hasShowed;
            ConfigManager.SaveConfig(configData);
            isShowed = true;
            Time.timeScale = 0f;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            isShowed = !isShowed;
            if (!isShowed)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(true);
                Time.timeScale = 0f;
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isShowed)
            {
                isShowed = false;
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}
