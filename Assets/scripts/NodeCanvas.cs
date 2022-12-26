using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NodeCanvas : MonoBehaviour
{
    public Node _node;

    public Text startText;
    public Text editingText;
    
    public Button minusButton;
    public Button plusButton;
    
    void Awake()
    {
        Button plus = plusButton.GetComponent<Button>();
        plus.onClick.AddListener(AddRadius);
        Button minus = minusButton.GetComponent<Button>();
        minus.onClick.AddListener(MinusRadius);
    }

    public void SetNode(Node node)
    {
        this._node = node;
        Setname();
    }

    void AddRadius()
    {
        _node.AddRadius(5f);
    }
    
    void MinusRadius()
    {
        _node.AddRadius(-5f);
    }

    public void Setname()
    {
        startText.text = _node.name;
    }
    
    private void OnDisable()
    {
        if (editingText.GetComponent<Text>().text != "")
        {
            _node.SetName(editingText.GetComponent<Text>().text);
        }
    }
}
