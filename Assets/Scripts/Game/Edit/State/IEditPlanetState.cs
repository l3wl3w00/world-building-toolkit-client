using UnityEngine;

namespace WorldBuilder.Client.Game.Edit.State
{
    public interface IEditPlanetState
    {
        void OnClick(Transform selfTransform);
        void OnRotate(Transform selfTransform);
        void OnCancel();
    }
}