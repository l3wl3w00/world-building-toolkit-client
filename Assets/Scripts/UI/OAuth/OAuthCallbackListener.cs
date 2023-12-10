#nullable enable
using System;
using System.Net;
using System.Threading.Tasks;
using CodiceApp.EventTracking.Plastic;
using Common;
using Common.Constants;
using Common.Generated;
using Common.Utils;
using UnityEngine;

namespace UI.OAuth
{
    public class OAuthCallbackListener : MonoBehaviour
    {
        private readonly ITimeProvider _timeProvider = new SystemTimeProvider();
        private Option<UnityMainThreadDispatcher> _unityMainThreadDispatcher = Option<UnityMainThreadDispatcher>.None;
        private Option<HttpListener> _listener = Option<HttpListener>.None;

        #region Event Functions

        private void Start()
        {
            StartServer();
            _unityMainThreadDispatcher = UnityMainThreadDispatcher.Instance.ToOption();
            Task.Run(CheckForRequests);
        }
        private void OnDestroy()
        {
            _listener.DoIfNotNull(l => { if (l.IsListening) l.Stop(); });
        }

        #endregion

        private void StartServer()
        {
            _listener = new HttpListener().ToOption()
                .DoIfNotNull(l => l.Prefixes.Add("http://localhost:8080/"))
                .DoIfNotNull(l => l.Start());
        }

        private async Task CheckForRequests()
        {
            await _listener.DoIfNotNullAsync(async listener =>
            {
                try
                {
                    await ListeningLoop(listener);
                }
                catch (Exception e)
                {
                    Debug.LogError("An error occurred: " + e.Message);
                }
                finally
                {
                    listener.Stop();
                }
            });

        }

        private async Task ListeningLoop(HttpListener listener)
        {
            while (listener.IsListening)
            {
                var context = await listener.GetContextAsync();
                var request = context.Request;

                var token = request.QueryString["token"];
                var expiration = int.Parse(request.QueryString["expires"]);
                listener.Stop();
                if (string.IsNullOrEmpty(token)) return;
                
                _unityMainThreadDispatcher
                    .ExpectNotNull(nameof(_unityMainThreadDispatcher), (Func<HttpListener, Task>) ListeningLoop)
                    .Enqueue(() =>
                    {
                        PlayerPrefs.SetString(AuthConstants.GoogleTokenKey, token);
                        PlayerPrefs.SetInt(AuthConstants.GoogleTokenExpirationKey, expiration);
                        PlayerPrefs.SetString(AuthConstants.GoogleTokenSaveDateKey,
                            _timeProvider.Now.ToBinary().ToString());
                        PlayerPrefs.Save();
                        Scene.MainMenuScreen.Load();
                    });
            }
        }
    }
}