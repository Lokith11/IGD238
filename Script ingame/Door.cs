using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    private UIManager uIManager;

    public bool isExit;
    private bool isPlayerInRange = false;


    void Start()
    {
        uIManager = FindObjectOfType<UIManager>(); 
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E)) // ��Ǩ�ͺ��á����� E
        {
            uIManager.ShowChoices(); // �ʴ� UI
            uIManager.pressDoorText.SetActive(false);
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isExit)
            {
                uIManager.winButton.SetActive(true);
                Time.timeScale = 0;
            }
            else 
            {
                isPlayerInRange = true;
                uIManager.pressDoorText.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            uIManager.HideChoices(); // ��͹ UI ����ͼ������͡�ҡ����
            uIManager.pressDoorText.SetActive(false);
        }
    }
}
