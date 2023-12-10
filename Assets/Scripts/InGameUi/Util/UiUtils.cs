#nullable enable
using System.Linq;
using Client.Response;
using Common;
using Common.Generated;
using Common.Utils;
using UnityEngine;

namespace InGameUi.Util
{
    public static class ContentUtils
    {
        private const string NameOfContent = "Content";

        public static GameObject GetContentInChildren(this GameObject gameObject) =>
            gameObject
                .GetComponentsInChildren<Component>()
                .FirstOrDefault(c => c.name == NameOfContent)
                .ToOption()
                .ExpectNotNull($"No GameObject named '{NameOfContent}' was present under game object '{gameObject.name}'", gameObject)
                .gameObject;

        public static GameObject ContentInDirectChildren(this GameObject gameObject)
        {
            return gameObject
                .GetChild(NameOfContent)
                .ExpectNotNull($"No GameObject named '{NameOfContent}' was present under game object {gameObject.name}",
                    gameObject);
        }

        private static Option<Transform> ContentInParentsMaybe(this Transform transform)
        {
            return transform.GetParentDirectlyUnderContentMaybe().NullOr(p => p.parent);
        }

        public static Option<Transform> GetParentDirectlyUnderContentMaybe(this Transform originalTransform)
        {
            var currentTransform = originalTransform;
            while (true)
            {
                var parentOfCurrentOpt = currentTransform.parent.ToOption();
                if (parentOfCurrentOpt.NoValueOut(out var parent))
                {
                    return Option<Transform>.None;
                }

                var isParentContent = parent!.name == NameOfContent;
                if (isParentContent) return currentTransform.ToOption();

                // step up one level
                currentTransform = parent;
            }
        }

        public static Transform GetParentDirectlyUnderContent(this Transform originalTransform)
        {
            var gameObject = originalTransform.gameObject;
            return originalTransform
                .GetParentDirectlyUnderContentMaybe()
                .ExpectNotNull(
                    $"{gameObject} is not under any game object named {NameOfContent}, but is expected to be",
                    gameObject);
        }

        private static Transform ContentInParents(this Transform transform)
        {
            var gameObject = transform.gameObject;
            return transform
                .ContentInParentsMaybe()
                .ExpectNotNull($"No GameObject named '{NameOfContent}' was present in the parents of {gameObject.name}",
                    gameObject);
        }
    }
    public static class MessageUtils
    {
        public static void DisplayInfo(string message, float secondsToLive = 5) =>
            DisplayMessage(message, MessageType.Info, secondsToLive);
        public static void DisplayWarning(string message, float secondsToLive = 5) => 
            DisplayMessage(message, MessageType.Warning, secondsToLive);
        public static void DisplayError(string message, float secondsToLive = 5) => 
            DisplayMessage(message, MessageType.Error, secondsToLive);
        
        public static void DisplayToUi(this ErrorResponse errorResponse, float secondsToLive = 5) => 
            DisplayError("Error in the response from the server: " + errorResponse.Title + "\nError Code: " + errorResponse.Status, secondsToLive);
        public static void DisplayMessage(string message, MessageType type, float secondsToLive = 5) =>
            Prefab.DisplayShortMessage
                .InstantiateAndExpectComponent<DisplayShortMessageController>()
                .Construct(message, type, secondsToLive);
    }
}