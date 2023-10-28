#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Common.Utils;
using Game.Planet_.Parts;
using Game.Planet_.Parts.State;
using InGameUi.InputFiller;
using UnityEngine;

namespace InGameUi
{
    public class HudController : MonoBehaviour
    {
        [Serializable]
        private struct HudMapElement
        {
            public ContinentsState continentsState;
            public GameObject gameObject;
        }
        #region Serialized Fields

        [SerializeField] private Canvas canvas = null!; // asserted in Awake
        [SerializeField] private PlanetMonoBehaviour planetMono = null!; // asserted in Awake
        [SerializeField] private List<HudMapElement> hudMap = null!;
        private Dictionary<ContinentsState, GameObject> _hudMap = new();

        #endregion

        private ContinentsState _previousState = ContinentsState.Null;

        #region Event Functions

        private void Awake()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), canvas, planetMono, hudMap);
        }

        private void Start()
        {
            FillHudMap();
            SetAllCanvasesInactive();
            // _hudMap.Values.ForEach(InjectDependencies);
            planetMono.StateChanged.AddListener((_,_) => UpdateInputFields());
            UpdateInputFields();
        }

        #endregion

        private void Update()
        {
            var hudScreen = planetMono.State;
            if (hudScreen == _previousState) return;

            foreach (var s in _hudMap.Values) s.SetActive(false);
            _hudMap[hudScreen].SetActive(true);
            UpdateInputFields();
            _previousState = hudScreen;
        }


        private void UpdateInputFields()
        {
            Debug.Log("Update input fields");
            _hudMap[planetMono.State].GetComponentsInChildren<IInputFiller>().ForEach(t => t.UpdateValue());
        }
        

        #region StartHelpers

        private void FillHudMap()
        {
            _hudMap = hudMap.ToDictionary(e => e.continentsState, e => e.gameObject);
            // _hudMap.Add(ContinentsState.EditPlanet, Prefab.EditPlanetPanel.Instantiate(canvas.transform));
            // _hudMap.Add(ContinentsState.CreatePlanet, Prefab.CreatePlanetPanel.Instantiate(canvas.transform));
            // _hudMap.Add(ContinentsState.ContinentInCreation, Prefab.CreateBoundedPanel.Instantiate(canvas.transform));
            // _hudMap.Add(ContinentsState.RegionInCreation, Prefab.CreateRegionPanel.Instantiate(canvas.transform));
            // _hudMap.Add(ContinentsState.SelectContinent, Prefab.EditContinentPanel.Instantiate(canvas.transform));
            // _hudMap.Add(ContinentsState.SelectedRegion, Prefab.EditRegionPanel.Instantiate(canvas.transform));
            _hudMap.Add(ContinentsState.Null, new GameObject("Null HudScreen"));
        }

        // private void InjectDependencies(GameObject gameObj) => gameObj.InjectToAllChildren(planetMono);

        private void SetAllCanvasesInactive()
        {
            _hudMap.Values.ForEach(p => p.SetActive(false));
        }

        #endregion
    }
}