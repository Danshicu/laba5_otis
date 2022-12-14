using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[RequireComponent(typeof(CircleCollider2D))]
public class Node : MonoBehaviour
{
    private Transform _transform;
    public float _radius=20f;
    //private LineRenderer circleRenderer;

    private CapsuleCollider collider;
    // Start is called before the first frame update
    void Awake()
    {
        _transform = GetComponent<Transform>();
        collider = GetComponent<CapsuleCollider>();
        //circleRenderer = GetComponent<LineRenderer>();
    }
    
    void Update()
    {
       // _transform.
    }

    /*public void DrawCircle(int steps, Vector3 position)
    {
        circleRenderer.positionCount = steps;
        for (int currentStep = 0; currentStep < steps; currentStep++)
        {
            float circumferenceProgress = (float)currentStep / steps;
            float currentRadian = circumferenceProgress * 2 * Mathf.PI;

            float x = _radius * Mathf.Cos(currentRadian) + position.x;
            float y = _radius * Mathf.Sin(currentRadian) + position.y;

            Vector3 currentPosition = new Vector3(x, y, 0);
            circleRenderer.SetPosition(currentStep, currentPosition);
        }

        collider.radius = _radius;
    }*/
}
