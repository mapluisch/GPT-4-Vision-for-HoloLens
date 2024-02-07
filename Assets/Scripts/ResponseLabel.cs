using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseLabel : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshPro label;

    private void OnEnable()
    {
        Application.logMessageReceived += ShowText;
        OpenAIWrapper.OnOpenAIResponse += ShowText;
    }

    void ShowText(string log, string _, LogType __) => ShowText(log);
    void ShowText(string s) => label.text = s;
}
