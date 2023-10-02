#nullable enable
using System;
using UnityEngine;

namespace Game.Client.Response
{
    [Serializable]
    public class ErrorResponse
    {
        #region Serialized Fields

        public string type;
        public string title;
        public long status;
        public string traceId;

        #endregion

        public void LogError()
        {
            Debug.LogError(title);
        }
    }
}