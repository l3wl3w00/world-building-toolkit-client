#nullable enable
using System;
using Common;
using Common.Constants;
using Common.Generated;
using Common.Utils;
using UnityEngine;

namespace EntryPoint
{
    public class InitialScreenChooser : MonoBehaviour
    {
        private readonly ITimeProvider _timeProvider = new SystemTimeProvider();

        #region Event Functions

        private void Start()
        {
            var anyKeyMissing = !(PlayerPrefs.HasKey(AuthConstants.GoogleTokenKey) &&
                                  PlayerPrefs.HasKey(AuthConstants.GoogleTokenExpirationKey) &&
                                  PlayerPrefs.HasKey(AuthConstants.GoogleTokenSaveDateKey));
            if (anyKeyMissing)
            {
                Scene.UnauthorizedScreen.Load();
                return;
            }

            var token = PlayerPrefs.GetString(AuthConstants.GoogleTokenKey);
            var expiration = PlayerPrefs.GetInt(AuthConstants.GoogleTokenExpirationKey);
            var saveDateString = PlayerPrefs.GetString(AuthConstants.GoogleTokenSaveDateKey);

            var saveDate = DateTime.FromBinary(long.Parse(saveDateString));

            const int secondsOverHead = 60;
            var isExpired = _timeProvider.Now >= saveDate.AddSeconds(expiration - secondsOverHead);
            if (isExpired)
            {
                Scene.UnauthorizedScreen.Load();
                return;
            }

            Scene.MainMenuScreen.Load();
        }

        #endregion
    }
}