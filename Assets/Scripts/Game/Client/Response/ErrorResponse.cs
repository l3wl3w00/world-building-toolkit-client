#nullable enable
using System;
using Game.Client.Dto;
using UnityEngine;

namespace Game.Client.Response
{
    public record ErrorResponse : JsonSerializable<ErrorResponse>
    {
        public string Type { get; set; } = "";
        public string Title { get; set; } = "";
        public long Status { get; set; }
        public string TraceId { get; set; } = "";
        public void LogError()
        {
            Debug.LogError("Error in the response from the server: " + Title);
        }
    }
}