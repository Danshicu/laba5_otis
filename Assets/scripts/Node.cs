using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    public float radius=30f;
    public string name = "Node name";
    public NameChecker nameCanvasText;

    private CapsuleCollider _collider;
    
    void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
    }
    
    public void SetName(string name)
    {
        this.name = name;
        nameCanvasText.TextUpdate(name);
    }

    public void AddRadius(float value)
    {
        this.radius += value;
        this.SetScale();
    }

    private void SetScale()
    {
        this.GetComponent<Transform>().localScale = new Vector3(radius, radius, 0.1f);
    }
    
    
    
    

}
