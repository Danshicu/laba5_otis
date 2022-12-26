using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(BoxCollider))]
public class Arrow : MonoBehaviour
{
    public float value = 0;
    public float stemLength;
    public float stemWidth = 8f;
    public float tipLength = 55f;
    public float tipWidth = 20f;

    [System.NonSerialized] public List<Vector3> verticesList;
    [System.NonSerialized] public List<int> trianglesList;

    private Mesh _mesh;
    private Renderer _renderer;
    private Color _color;
    private BoxCollider _collider;
    private Canvas _canvas;
    private Vector3 position;
    private float length;
    
    void OnEnable()
    {
        _collider = GetComponent<BoxCollider>(); 
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        _renderer = GetComponent<Renderer>();
        _color = _renderer.material.color;
    }

    public void SetValue(float value)
    {
        this.value = value;
    }

    public void AddTipWidth(float value)
    {
        this.tipWidth += value;
        RegenerateArrow();
    }
    
    public void AddStemWidth(float value)
    {
        this.stemWidth += value;
        RegenerateArrow();
    }
    
     void Update()
     {
         switch (value)
         {
             case 0:_color=Color.black;
                 break;
             case <0: _color = Color.red;
                 break;
             case>0: _color = Color.green;
                 break;
         }
         SetColor(_color);
     }

     public void RegenerateArrow()
     {
         verticesList = new List<Vector3>();
         trianglesList = new List<int>();

         verticesList.Add(position + (stemWidth/2 * Vector3.down));
         verticesList.Add(position + (stemWidth/2 * Vector3.up));
         verticesList.Add(verticesList[0] + (stemLength * Vector3.right));
         verticesList.Add(verticesList[1] + (stemLength * Vector3.right));
        
         trianglesList.Add(0);
         trianglesList.Add(1);
         trianglesList.Add(3);
        
         trianglesList.Add(0);
         trianglesList.Add(3);
         trianglesList.Add(2);

         Vector3 tipOrigin = position+(stemLength * Vector3.right);

         verticesList.Add(tipOrigin+(tipWidth/2*Vector3.up));
         verticesList.Add(tipOrigin+(tipWidth/2*Vector3.down));
         verticesList.Add(tipOrigin+(tipLength*Vector3.right));
        
         trianglesList.Add(4);
         trianglesList.Add(6);
         trianglesList.Add(5);

         _mesh.vertices = verticesList.ToArray();
         _mesh.triangles = trianglesList.ToArray();
         _collider.size = new Vector3(length, tipWidth, 0.1f);
         _collider.center = new Vector3(length / 2, 0f, 0f);
      
         SetColor(_color);
     }

     public void GenerateArrow(float length, Vector3 startPos)
    {
        position = startPos;
        this.length = length;
        stemLength = length -tipLength;
        verticesList = new List<Vector3>();
        trianglesList = new List<int>();

        verticesList.Add(startPos + (stemWidth/2 * Vector3.down));
        verticesList.Add(startPos + (stemWidth/2 * Vector3.up));
        verticesList.Add(verticesList[0] + (stemLength * Vector3.right));
        verticesList.Add(verticesList[1] + (stemLength * Vector3.right));
        
        trianglesList.Add(0);
        trianglesList.Add(1);
        trianglesList.Add(3);
        
        trianglesList.Add(0);
        trianglesList.Add(3);
        trianglesList.Add(2);

        Vector3 tipOrigin = startPos+(stemLength * Vector3.right);

        verticesList.Add(tipOrigin+(tipWidth/2*Vector3.up));
        verticesList.Add(tipOrigin+(tipWidth/2*Vector3.down));
        verticesList.Add(tipOrigin+(tipLength*Vector3.right));
        
        trianglesList.Add(4);
        trianglesList.Add(6);
        trianglesList.Add(5);

        _mesh.vertices = verticesList.ToArray();
        _mesh.triangles = trianglesList.ToArray();
        _collider.size = new Vector3(length, tipWidth, 0.1f);
        _collider.center = new Vector3(length / 2, 0f, 0f);
      
        SetColor(_color);
    }

    public void SetColor(Color newcolor)
    {
        _renderer.material.color = newcolor;
    }
    
  
}
