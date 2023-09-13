using System;
using System.Threading.Tasks;
using UnityEngine;
using System.Net;
using UnityEngine.SceneManagement;
using WorldBuilder.Client.Common;

namespace WorldBuilder.Client.UI.OAuth
{
    public class OAuthCallbackListener : MonoBehaviour
    {
        private HttpListener listener;
        private Task listenerTask;
        private UnityMainThreadDispatcher _unityMainThreadDispatcher;
        void Start()
        {
            _unityMainThreadDispatcher = UnityMainThreadDispatcher.Instance;
            StartServer();
            listenerTask = Task.Run(CheckForRequests);  // Start only one task
        }

        void Update()
        {
            // Handle main-thread-only tasks if necessary
        }

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
                    HttpListenerContext context = await listener.GetContextAsync();
                    HttpListenerRequest request = context.Request;

                    // Read the OAuth code or token from the request
                    string token = request.QueryString["token"];
                    listener.Stop();
                    if (!string.IsNullOrEmpty(token))
                    {
                        // Handle token
                        // Dispatch this back to Unity main thread if needed
                        _unityMainThreadDispatcher.Enqueue(() =>
                        {
                            PlayerPrefs.SetString("google-token", token);
                            PlayerPrefs.Save();
                            SceneManager.LoadScene(SceneNames.WorldListScreen);
                        });
                    }
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

        void OnDestroy()
        {
            // Stop the listener when the GameObject is destroyed
            if (listener is { IsListening: true })
            {
                listener.Stop();
            }
        }
    }
}
