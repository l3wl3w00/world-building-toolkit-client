using System;
using System.Collections;
using System.Collections.Generic;
using LineUtils;
using UnityEngine;

public class PlanetControl : MonoBehaviour
{
    private const float Speed = 10.0f;
    private Vector3 _lastMousePosition;
    private LineAlongSphere _line;
    
    public float Radius { get; set; }

    private void Start()
    {
        _line = new LineAlongSphere(GetComponent<LineRenderer>(), transform);

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // When the mouse is clicked, record the position
            _lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            // When the mouse is held down, rotate the sphere
            var delta = Input.mousePosition - _lastMousePosition;
            _lastMousePosition = Input.mousePosition;

            var axis = new Vector3(-delta.y, delta.x, 0);
            transform.Rotate(axis * (Speed * Time.deltaTime), Space.World);
        }

        
    }

    private void OnMouseDown()
    {
        if (Camera.main == null)
        {
            Debug.Log("Main camera is null");
            return;
        }
        
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var raycastSuccessful = Physics.Raycast(ray, out var hit);

        if (!raycastSuccessful)  return;

        var hitCollider = hit.collider as SphereCollider;

        if (hitCollider == null) 
        {
            Debug.Log("ray hit something that is not a SphereCollider");
            return;
        }

        _line.ClickedOnSphere(hit.point);
        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
        sphere.transform.position = hit.point;
    }
}