#nullable enable
using System;
using System.Collections.Generic;
using Game.Util;
using UnityEngine;

namespace Common
{
    public class UnityMainThreadDispatcher : MonoBehaviour
    {
        private static Option<UnityMainThreadDispatcher> _instance = Option<UnityMainThreadDispatcher>.None;
        private readonly Queue<Action> _actionQueue = new();

        #region Properties

        public static UnityMainThreadDispatcher Instance
        {
            get
            {
                if (_instance.HasValue) return _instance.Value;
            
                var dispatcherGameObject = new GameObject("UnityMainThreadDispatcher");
                DontDestroyOnLoad(dispatcherGameObject);
                _instance = dispatcherGameObject.AddComponent<UnityMainThreadDispatcher>().ToOption();
                
                return _instance.Value;
            }
        }

        #endregion

        #region Event Functions

        private void Update()
        {
            lock (_actionQueue)
            {
                while (_actionQueue.Count > 0) _actionQueue.Dequeue().Invoke();
            }
        }

        #endregion

        public void Enqueue(Action action)
        {
            lock (_actionQueue)
            {
                _actionQueue.Enqueue(action);
            }
        }
    }
}