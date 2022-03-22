using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrift : MonoBehaviour
{
    [SerializeField] GameObject cameraContainer;
    [SerializeField] float maxXDrift = 1.5f;
    [SerializeField] float maxYDrift = 1.15f;
    [SerializeField] float driftSpeed = 0.5f;
    
    float addedXDrift = 0f;
    float addedYDrift = 0f;

    Vector2 mouseFromCenter;

    // Start is called before the first frame update
    void Start()
    {
        mouseFromCenter = new Vector2(0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        mouseFromCenter = GetMousePosition();
    }
    Vector2 GetMousePosition()
    {
        float mouseXFromCenter = Input.mousePosition.x - Screen.width / 2;
        float mouseYFromCenter = Input.mousePosition.y - Screen.height / 2;

        return new Vector2(mouseXFromCenter, mouseYFromCenter);
    }

    
}
