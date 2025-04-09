using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hint : MonoBehaviour
{
    [SerializeField] private bool isShowed;
    void Start()
    {
        // PlayerPrefs.DeleteKey("Guide");
        int hasShowed = PlayerPrefs.GetInt("Guide", 0);
        if (hasShowed != 0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            isShowed = false;
            Time.timeScale = 1f;
        }
        else
        {
            PlayerPrefs.SetInt("Guide", 1);
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
