#nullable enable
using System;
using System.Collections.Generic;
using Common;
using Game.Continent;
using Game.Hud.Panel;
using Game.Linq;
using Game.Planet;
using Game.SceneChange;
using Game.Util;
using Generated;
using UnityEngine;

namespace Game.Hud
{
    public enum HudScreen
    {
        PlanetEdit,
        PlanetCreate,
        ContinentCreate,
        ContinentEdit,
        Null
    }

    public class HudController : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private Canvas canvas = null!; // asserted in Awake
        [SerializeField] private PlanetControl planetControl = null!; // asserted in Awake

        #endregion

        private readonly Dictionary<HudScreen, GameObject> _hudMap = new();
        private HudScreen _currentHudScreen = HudScreen.Null;
        private HudScreen _previousHudScreen = HudScreen.Null;

        #region Properties

        public HudScreen CurrentHudScreen
        {
            get => _currentHudScreen;
            set
            {
                if (value == _currentHudScreen) return;

                _hudMap[_currentHudScreen].SetActive(false);
                var newHud = _hudMap[value];
                newHud.SetActive(true);
                _previousHudScreen = _currentHudScreen;
                _currentHudScreen = value;
            }
        }

        #endregion

        #region Event Functions

        private void Awake()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), canvas, planetControl);
        }

        private void Start()
        {
            FillHudMap();
            SetAllCanvasesInactive();

            _hudMap.Values.ForEach(InjectDependencies);

            planetControl.SelectedContinentChanged.AddListener(OnSelectedContinentChanged);

            CurrentHudScreen = ISceneChangeParameters.Instance.GetNonNullable<HudScreen>(SceneParamKey.InitialScreen);
            UpdateInputFields();
        }

        #endregion

        private void OnSelectedContinentChanged(
            Option<ContinentHandler> oldContinent,
            Option<ContinentHandler> newContinent)
        {
            newContinent
                .DoIfNotNull(_ => CurrentHudScreen = HudScreen.ContinentEdit)
                .DoIfNull(() => CurrentHudScreen = HudScreen.PlanetEdit);

            UpdateInputFields();
        }

        public void UpdateInputFields()
        {
            Debug.Log("Update input fields");
            _hudMap[_currentHudScreen].GetComponentsInChildren<IInputFiller>().ForEach(t => t.UpdateValue());
        }

        public bool HasPreviousScreen()
        {
            return _previousHudScreen != HudScreen.Null;
        }

        public void ToPreviousScreen()
        {
            if (_previousHudScreen == HudScreen.Null)
            {
                Debug.LogError("Tried to go back, when there were no previous HudScreens");
                return;
            }

            CurrentHudScreen = _previousHudScreen;
        }

        public void ToDefaultPanel()
        {
            CurrentHudScreen = HudScreen.PlanetEdit;
            planetControl.ContinentInCreation = Option<ContinentHandler>.None;
        }

        #region StartHelpers

        private void FillHudMap()
        {
            _hudMap.Add(HudScreen.PlanetEdit, Prefab.EditPlanetPanel.Instantiate(canvas.transform));
            _hudMap.Add(HudScreen.PlanetCreate, Prefab.CreatePlanetPanel.Instantiate(canvas.transform));
            _hudMap.Add(HudScreen.ContinentCreate, Prefab.CreateBoundedPanel.Instantiate(canvas.transform));
            _hudMap.Add(HudScreen.ContinentEdit, Prefab.EditContinentPanel.Instantiate(canvas.transform));
            _hudMap.Add(HudScreen.Null, new GameObject("Null HudScreen"));
        }

        private void InjectDependencies(GameObject gameObj)
        {
            gameObj.GetComponentsInChildren<IInputFiller>()
                .ForEach(t => t.PlanetControl = planetControl);
            gameObj.ForEachHudButtonControl(b =>
            {
                b.HudController = this;
                b.PlanetControl = planetControl;
            });
        }

        private void SetAllCanvasesInactive()
        {
            _hudMap.Values.ForEach(p => p.SetActive(false));
        }

        #endregion
    }
}