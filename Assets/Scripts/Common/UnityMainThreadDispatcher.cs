#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class UnityMainThreadDispatcher : MonoBehaviour
    {
        private static UnityMainThreadDispatcher _instance;
        private readonly Queue<Action> actionQueue = new();

        #region Properties

        public static UnityMainThreadDispatcher Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("UnityMainThreadDispatcher");
                    DontDestroyOnLoad(go);
                    _instance = go.AddComponent<UnityMainThreadDispatcher>();
                }

                return _instance;
            }
        }

        #endregion

        #region Event Functions

        private void Update()
        {
            lock (actionQueue)
            {
                while (actionQueue.Count > 0) actionQueue.Dequeue().Invoke();
            }
        }

        #endregion

        public void Enqueue(Action action)
        {
            lock (actionQueue)
            {
                actionQueue.Enqueue(action);
            }
        }
    }
}