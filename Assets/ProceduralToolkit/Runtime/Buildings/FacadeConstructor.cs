#nullable enable
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Buildings
{
    public abstract class FacadeConstructor : ScriptableObject, IFacadeConstructor
    {
        #region IFacadeConstructor Members

        public abstract void Construct(List<Vector2> foundationPolygon, List<ILayout> layouts,
            Transform parentTransform);

        #endregion
    }
}