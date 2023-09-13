using UnityEngine;

namespace WorldBuilder.Client.UI.Common.Button
{
    public abstract class ButtonControl : MonoBehaviour, IButton
    {
        public string Name => this.name;
        public abstract void OnClicked();
    }
}