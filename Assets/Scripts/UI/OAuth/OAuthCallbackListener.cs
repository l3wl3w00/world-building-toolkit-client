#nullable enable
using System;
using System.Net;
using System.Threading.Tasks;
using Common;
using Game.Constants;
using Generated;
using UnityEngine;

namespace UI.OAuth
{
    public class OAuthCallbackListener : MonoBehaviour
    {
        private readonly ITimeProvider _timeProvider = new SystemTimeProvider();
        private UnityMainThreadDispatcher _unityMainThreadDispatcher;
        private HttpListener listener;
        private Task listenerTask;

        #region Event Functions

        private void Start()
        {
            _unityMainThreadDispatcher = UnityMainThreadDispatcher.Instance;
            StartServer();
            listenerTask = Task.Run(CheckForRequests); // Start only one task
        }

        private void Update()
        {
            // Handle main-thread-only tasks if necessary
        }

        private void OnDestroy()
        {
            // Stop the listener when the GameObject is destroyed
            if (listener is { IsListening: true }) listener.Stop();
        }

        #endregion

        private void StartServer()
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8080/");
            listener.Start();
        }

        private async Task CheckForRequests()
        {
            try
            {
                while (listener.IsListening)
                {
                    var context = await listener.GetContextAsync();
                    var request = context.Request;

                    // Read the OAuth code or token from the request
                    var token = request.QueryString["token"];
                    var expiration = int.Parse(request.QueryString["expires"]);
                    listener.Stop();
                    if (!string.IsNullOrEmpty(token))
                        // Handle token
                        // Dispatch this back to Unity main thread if needed
                        _unityMainThreadDispatcher.Enqueue(() =>
                        {
                            PlayerPrefs.SetString(AuthConstants.GoogleTokenKey, token);
                            PlayerPrefs.SetInt(AuthConstants.GoogleTokenExpirationKey, expiration);
                            PlayerPrefs.SetString(AuthConstants.GoogleTokenSaveDateKey,
                                _timeProvider.Now.ToBinary().ToString());
                            PlayerPrefs.Save();
                            Scenes.MainMenuScreen.Load();
                        });
                }
            }
            catch (Exception e)
            {
                // Handle exception
                Debug.LogError("An error occurred: " + e.Message);
            }
            finally
            {
                listener.Stop();
            }
        }
    }
}