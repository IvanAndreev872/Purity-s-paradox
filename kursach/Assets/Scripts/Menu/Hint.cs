using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hint : MonoBehaviour
{
    public bool isShowed = true;
    public string hint = "LMB, X, M - shoot\nRMB, Z, Space - melee";
    public TMP_Text hintText;
    void Start()
    {
        hintText = transform.GetComponent<TMP_Text>();
        hintText.text = hint;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            isShowed = !isShowed;
            hintText.text = isShowed ? hint : "";
        }  
    }
}
