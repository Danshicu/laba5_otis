using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class EntryPoint : MonoBehaviour
{
    private Vector2 screenSize;
    private float clickCount = 0;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Arrow arrowPref;
    [SerializeField] private Node nodepref;
    
    //[SerializeField] private SpriteRenderer squarePref;
   
    private List<Arrow> arrows;
    private List<Node> nodes;
    private Canvas myCanvas;
    public Vector3 firstClickPos;
    public Vector3 secondClickPos;
    [SerializeField] private List<Canvas> canvases;
    [SerializeField] private float minDistance = 20f;
    
    //[SerializeField] private Canvas arrowCanvas;
    
    [SerializeField] private LayerMask nodes_mask;
    [SerializeField] private LayerMask arrows_mask;
    
    
    void Awake()
    {
        nodes = new List<Node>();
        arrows = new List<Arrow>();
        firstClickPos = new Vector3(0,0, 0);
        secondClickPos = new Vector3(0,0, 0);
    }

    private void Start()
    {
        // nodes_mask = ~nodes_mask;
        // arrows_mask = ~arrows_mask;
        screenSize = new Vector2(Screen.width, Screen.height);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 MouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 SpawnPos = MouseWorldPosition;

            RaycastHit hit;
            bool attacked = Physics.Raycast(SpawnPos, Vector3.forward, out hit, 200f);
            Debug.DrawRay(SpawnPos, Vector3.forward * hit.distance, Color.yellow, 10f);
            Debug.Log(hit.collider);
            SpawnPos.z = -1f;
            
            if (MouseWorldPosition.x > screenSize.x / 2)
            {
                SpawnPos.x -= 350f*0.22f;
            }
            else
            {
                SpawnPos.x += 350f*0.22f;
            }
            if (MouseWorldPosition.y > screenSize.y / 2)
            {
                SpawnPos.y -= 145f*0.22f;
            }
            else
            {
                SpawnPos.y += 145f*0.22f;
            }

            if (attacked)
            {
                if (hit.collider.gameObject.CompareTag("Nodetag"))
                {
                    myCanvas = canvases[0];
                    attacked = false;
                    Instantiate(myCanvas, SpawnPos, Quaternion.identity);
                }

                if (hit.collider.gameObject.CompareTag("Arrowtag"))
                {
                    myCanvas = canvases[1];
                    attacked = false;
                    Instantiate(myCanvas, SpawnPos, Quaternion.identity);
                }
            }

            // Debug.Log(screenSize);
            // Debug.Log(SpawnPos);
            // Debug.Log(MouseWorldPosition);
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 MouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            MouseWorldPosition.z = 0f;
            clickCount++;
            if (clickCount==2)
            {
                secondClickPos = MouseWorldPosition;
            }
            else
            {
                firstClickPos =  MouseWorldPosition;
            }

            if (clickCount == 2)
            {
                float distance = Vector3.Distance(secondClickPos, firstClickPos);
                if (distance<minDistance)
                {
                    Debug.Log($"nodePosition: {secondClickPos}");
                    var node = Instantiate(nodepref, MouseWorldPosition, Quaternion.identity);
                    nodes.Add(node);
                    //node.DrawCircle(100,  MouseWorldPosition);
                }
                clickCount = 0;
                if (distance>minDistance)
                {
                    Vector3 angleVector = new Vector3((secondClickPos.x - firstClickPos.x),
                        (secondClickPos.y - firstClickPos.y), 0f);
                    float length = Vector3.Distance(firstClickPos, secondClickPos);
                    float angle = Vector3.Angle(Vector3.right, angleVector);
                    if (angleVector.y < 0)
                    {
                        angle *= -1;
                    }
                    
                    // Debug.Log($"angle: {angle}");
                    // Debug.Log($"length: {length}");
                    // Debug.Log($"firstClickPos: {firstClickPos}");
                    // Debug.Log($"secondClickPos: {secondClickPos}");
                    var arrow = Instantiate(arrowPref, firstClickPos, Quaternion.identity);
                    arrow.GenerateArrow(length, Vector3.zero);
                    arrow.transform.Rotate(Vector3.forward*angle, Space.Self);
                    arrows.Add(arrow);
                }
                firstClickPos = secondClickPos = Vector3.zero;
            } 
        }
    }
}
