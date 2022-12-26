using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowCanvas : MonoBehaviour
{
   
    private Arrow _arrow;

    public Text startText;
    public Text editingText;

    public Button minusStemButton;
    public Button plusStemButton;
    
    public Button minusTipButton;
    public Button plusTipButton;
    
    public void SetArrow(Arrow arrow)
    {
        this._arrow = arrow;
        SetVeigth();
    }

    public void SetVeigth()
    {
        startText.text = _arrow.value.ToString();
    }

    private void Awake()
    {
        Button _minusStemButton = minusStemButton.GetComponent<Button>();
        _minusStemButton.onClick.AddListener(MinusStemWidth);
        Button _minusTipButton = minusTipButton.GetComponent<Button>();
        _minusTipButton.onClick.AddListener(MinusTipWidth);
        Button _plusStemButton = plusStemButton.GetComponent<Button>();
        _plusStemButton.onClick.AddListener(AddStemWidth);
        Button _plusTipButton = plusTipButton.GetComponent<Button>();
        _plusTipButton.onClick.AddListener(AddTipWidth);
    }
    

    void AddTipWidth()
    {
        _arrow.AddTipWidth(3f);
    }
    
    void MinusTipWidth()
    {
        _arrow.AddTipWidth(-3f);
    }

    void AddStemWidth()
    {
        _arrow.AddStemWidth(3f);
    }
    
    void MinusStemWidth()
    {
        _arrow.AddStemWidth(-3f);
    }
    
    private void OnDisable()
    {
        if (editingText.GetComponent<Text>().text != "")
        {
            _arrow.SetValue(float.Parse(editingText.GetComponent<Text>().text));
        }
    }
}
