#nullable enable
using UnityEngine;

namespace ProceduralToolkit.Buildings
{
    public abstract class RoofConstructor : ScriptableObject, IRoofConstructor
    {
        #region IRoofConstructor Members

        public abstract void Construct(IConstructible<MeshDraft> constructible, Transform parentTransform);

        #endregion
    }
}