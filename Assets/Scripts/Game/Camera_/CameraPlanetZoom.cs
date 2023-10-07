#nullable enable
using JetBrains.Annotations;
using UnityEngine;

namespace Game.Camera_
{
    public static class CameraPlanetZoom
    {
        public static float EdgeDistanceFrom(this Transform planetTransform, Transform otherTransform)
        {
            var otherPosition = otherTransform.position;
            var planetPosition = planetTransform.position;
            var planetRadius = planetTransform.localScale.x / 2f;

            var distanceFromPlanetCenter = (otherPosition - planetPosition).magnitude;
            return distanceFromPlanetCenter - planetRadius;
        }

        public static float EdgeDistanceFromCamera(this Transform planetTransform, Camera camera) => 
            planetTransform.EdgeDistanceFrom(camera.transform);
        
    }
}