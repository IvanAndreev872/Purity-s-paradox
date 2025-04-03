using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Fountain : MonoBehaviour
{
    public TMP_Text pressButtonText;
    public bool inTrigger = false;
    public float part = 0.75f;
    public void Update()
    {
        if (inTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameObject.FindGameObjectWithTag("Character").GetComponent<PlayerHealth>().PartHeal(part);
            }
        }
    }
    public void Awake()
    {
        pressButtonText = GameObject.FindGameObjectWithTag("Hints").transform.GetChild(0).GetComponent<TMP_Text>();
    }
    private void ShowPressButtonText()
    {
        pressButtonText.text = "Press E To Heal";
        pressButtonText.gameObject.SetActive(true);
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            inTrigger = true;
            ShowPressButtonText();
        }
    }
    public void HidePressButtonText()
    {
        pressButtonText.text = "";
        pressButtonText.gameObject.SetActive(false);
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            inTrigger = false;
            HidePressButtonText();
        }
    }
}
