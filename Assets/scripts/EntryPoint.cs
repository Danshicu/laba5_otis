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
    [SerializeField] private Camera mainCamera;
    private float clickCount = 0;
    [SerializeField] private Arrow arrowPref;
    [SerializeField] private SpriteRenderer squarePref;
    private List<Arrow> arrows;
    public Vector3 firstClickPos;
    public Vector3 secondClickPos;
    [SerializeField] private Canvas nodeCanvas;
    [SerializeField] private Canvas arrowCanvas;
    [SerializeField] private Node _node;
    
    
    void Awake()
    {
        arrows = new List<Arrow>();
        firstClickPos = new Vector3(0,0, 0);
        secondClickPos = new Vector3(0,0, 0);
    }

    private void Start()
    {
        screenSize = new Vector2(Screen.width, Screen.height);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 MouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 SpawnPos = MouseWorldPosition;
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
            SpawnPos.z = 0;
            Instantiate(nodeCanvas, SpawnPos, Quaternion.identity);
            Debug.Log(screenSize);
            Debug.Log(SpawnPos);
            Debug.Log(MouseWorldPosition);
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
                if (firstClickPos == secondClickPos)
                {
                    Debug.Log($"squarePosition: {secondClickPos}");
                    var square = Instantiate(squarePref, MouseWorldPosition, Quaternion.identity);
                    var node = Instantiate(_node, MouseWorldPosition, Quaternion.identity);
                    node.DrawCircle(100, 3, MouseWorldPosition);
                }
                clickCount = 0;
                if (secondClickPos != firstClickPos)
                {
                    Vector3 angleVector = new Vector3((secondClickPos.x - firstClickPos.x),
                        (secondClickPos.y - firstClickPos.y), 0f);
                    float length = Vector3.Distance(firstClickPos, secondClickPos);
                    float angle = Vector3.Angle(Vector3.right, angleVector);
                    if (angleVector.y < 0)
                    {
                        angle *= -1;
                    }
                    
                    Debug.Log($"angle: {angle}");
                    Debug.Log($"length: {length}");
                    Debug.Log($"firstClickPos: {firstClickPos}");
                    Debug.Log($"secondClickPos: {secondClickPos}");
                    var arrow = Instantiate(arrowPref, firstClickPos, Quaternion.identity);
                    arrow.GenerateArrow(length, Vector3.zero);
                    arrow.transform.Rotate(Vector3.forward*angle, Space.Self);
                }
                firstClickPos = secondClickPos = Vector3.zero;
            } 
        }
    }
}
