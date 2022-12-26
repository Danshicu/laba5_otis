using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameChecker : MonoBehaviour
{

    public TextMeshProUGUI _text;
    public string name;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void TextUpdate(string text)
    {
        name = text;
        _text.text = name;
    }

    public void CheckName()
    {
        _text.text = name;
    }
    
}
