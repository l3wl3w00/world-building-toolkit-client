using Common.Constants;
using UnityEditor;
using UnityEngine;

namespace WorldBuilder.Client.Editor
{
    public static class RemoveToken
    {
        [MenuItem("Tools/Unauthorize")]
        public static void Generate()
        {
            PlayerPrefs.DeleteKey(AuthConstants.GoogleTokenKey);
            PlayerPrefs.DeleteKey(AuthConstants.GoogleTokenExpirationKey);
            PlayerPrefs.DeleteKey(AuthConstants.GoogleTokenSaveDateKey);
        }
    }
}