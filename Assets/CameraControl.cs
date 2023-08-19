using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private const float ZoomSpeedMultiplier = 50f;
    public PlanetControl planet;

    private void Update()
    {
        var selfPosition = transform.position;
        var planetTransform = planet.transform;
        var planetPosition = planetTransform.position;
        var planetRadius = planetTransform.localScale.x / 2f;
        
        var distanceFromPlanetCenter = (selfPosition - planetPosition).magnitude;
        var distanceFromPlanetEdge = distanceFromPlanetCenter - planetRadius;
        
        var zoomSpeed = Mathf.Sqrt(distanceFromPlanetEdge * ZoomSpeedMultiplier);

        var scrollValue = Input.GetAxis("Mouse ScrollWheel");

        selfPosition += new Vector3(0, 0, scrollValue * Time.deltaTime) * zoomSpeed;
        transform.position = selfPosition;
    }
}
