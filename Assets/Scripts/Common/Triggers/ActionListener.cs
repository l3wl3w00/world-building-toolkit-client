#nullable enable
using Common.ButtonBase;
using UnityEngine;

namespace Common.Triggers
{

    namespace GameController
    {
        public interface IActionListener { }

        public interface IControlActionListener<in TParam> : IActionListener
            where TParam : IActionParam
        {
            void OnTriggered(TParam param);
        }
        public abstract class ActionListenerMono<TParam> : MonoBehaviour, IControlActionListener<TParam>
            where TParam : IActionParam
        {
            public abstract void OnTriggered(TParam param);
        }
    }
}