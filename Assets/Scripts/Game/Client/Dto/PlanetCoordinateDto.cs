using System;

namespace Game.Client.Dto
{
    [Serializable]
    public record PlanetCoordinateDto
    {
        #region Serialized Fields

        public float radius;
        public float polar;
        public float azimuthal;

        #endregion

        public PlanetCoordinateDto(float radius, float polar, float azimuthal)
        {
            this.radius = radius;
            this.polar = polar;
            this.azimuthal = azimuthal;
        }
    }
}