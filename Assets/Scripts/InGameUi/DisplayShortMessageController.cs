#nullable enable
using System;
using Common.Utils;
using TMPro;
using UnityEngine;

namespace InGameUi
{
    public class DisplayShortMessageController : MonoBehaviour
    {
        [SerializeField] private TMP_Text text = null!; //Asserted in Construct
        
        private double _timeSinceCreation;

        private string _message = "";
        private MessageType _type;
        private float _secondsToLive;

        public void Construct(string message, MessageType type, float secondsToLive)
        {
            NullChecker.AssertNoneIsNullInType(GetType(), text);
            _message = message;
            _type = type;
            _secondsToLive = secondsToLive;
        }

        public void Start()
        {
            text.text = _message;
            text.color = _type switch
            {
                MessageType.Error => Color.red,
                MessageType.Info => Color.cyan,
                MessageType.Warning => Color.yellow,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void Update()
        {
            _timeSinceCreation += Time.deltaTime;
            if (_timeSinceCreation >= _secondsToLive)
            {
                Destroy(gameObject);
            }
        }
    }

    public enum MessageType
    {
        Error, Info, Warning
    }
}