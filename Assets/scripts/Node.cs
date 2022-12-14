using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private LineRenderer circleRenderer;
    // Start is called before the first frame update
    void Awake()
    {
        circleRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawCircle(int steps, float radius, Vector3 position)
    {
        circleRenderer.positionCount = steps;
        for (int currentStep = 0; currentStep < steps; currentStep++)
        {
            float circumferenceProgress = (float)currentStep / steps;
            float currentRadian = circumferenceProgress * 2 * Mathf.PI;

            float x = radius * Mathf.Cos(currentRadian) + position.x;
            float y = radius * Mathf.Sin(currentRadian) + position.y;

            Vector3 currentPosition = new Vector3(x, y, 0);
            circleRenderer.SetPosition(currentStep, currentPosition);
        }
    }
}
