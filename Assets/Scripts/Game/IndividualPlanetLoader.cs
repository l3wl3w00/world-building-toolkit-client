using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WorldBuilder.Client.Game.Edit;
using WorldBuilder.Client.Game.Line;

namespace WorldBuilder.Client
{
    public class IndividualPlanetLoader : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var planetStateHolder = FindObjectOfType<PlanetStateHolder>();
            var lines = new List<(LineAlongSphere line, LineRenderer renderer)>
            {
                //(new LineAlongSphere(new ), new LineRenderer()),
            };
            
            planetStateHolder.Planet = new Planet(lines);
            
            DontDestroyOnLoad(planetStateHolder);
            SceneManager.LoadScene(SceneNames.PlanetEditingScene);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
