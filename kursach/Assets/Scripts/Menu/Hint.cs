using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hint : MonoBehaviour
{
    [SerializeField] private bool isShowed = true;
    private string hint;
    [SerializeField] private TMP_Text hintText;
    void Start()
    {
        hint = "LMB, X, M - shoot\nRMB, Z, Space - melee\nShift - dash";
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
