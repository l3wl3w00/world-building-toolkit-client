using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetControl : MonoBehaviour
{
    public float speed = 10.0f;
    private Vector3 lastMousePosition;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // When the mouse is clicked, record the position
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            // When the mouse is held down, rotate the sphere
            Vector3 delta = Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;

            Vector3 axis = new Vector3(-delta.y, delta.x, 0);
            transform.Rotate(axis * speed * Time.deltaTime, Space.World);
        }
    }
}
