using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using WorldBuilder.Client.Common;
using WorldBuilder.Client.Game.Common.Client;
using WorldBuilder.Client.Game.Common.Constants;
using WorldBuilder.Client.Game.Common.Linq;
using WorldBuilder.Client.Game.Edit.State;

namespace WorldBuilder.Client.Game.Edit
{
    public class EditPhaseManager : MonoBehaviour
    {
        [SerializeField] private PlanetControl planetControl;
        [SerializeField] private GameObject lineControlPrefab;
        [SerializeField] private GameObject basePanelPrefab;
        [SerializeField] private GameObject boundedEditPanelPrefab;
        [SerializeField] private Canvas canvas;
        private WorldBuildingApiClient _client;
        
        public void BoundedEditClicked()
        {
            var newPanel = UpdateCanvas(boundedEditPanelPrefab);
            var childrenButtons = newPanel.GetChildren()
                .Select(g => g.GetComponent<Button>())
                .Where(b => b != null)
                .ToList();
            childrenButtons.Where(b => b.name == "OkButton").ForEach(b => b.onClick.AddListener(BoundedEditOkClicked));
            childrenButtons.Where(b => b.name == "CancelButton").ForEach(b => b.onClick.AddListener(BoundedEditCancelClicked));
            planetControl.EditPlanetState = new BoundedLocationEditingState(
                controlPointFactory: (p, r) => Instantiate(lineControlPrefab, p, r),
                planet: planetControl.Planet, 
                mainCamera: planetControl.MainCamera, 
                planetConstants: new PlanetConstants(),
                sphere: planetControl);
        }

        public void BoundedEditOkClicked()
        {
            ToDefaultPanel();
        }

        public void BoundedEditCancelClicked()
        {
            planetControl.EditPlanetState.OnCancel();
            ToDefaultPanel();
        }

        private void ToDefaultPanel()
        {
            var newPanel = UpdateCanvas(basePanelPrefab);
            var childrenButtons = newPanel.GetChildren()
                .Select(g => g.GetComponent<Button>())
                .Where(b => b != null)
                .ToList();
            childrenButtons.Where(b => b.name == "StartEdit").ForEach(b => b.onClick.AddListener(BoundedEditClicked));
            planetControl.EditPlanetState = new DefaultEditPlanetState(planetControl.Planet);
        }

        private GameObject UpdateCanvas(GameObject newPanelPrefab)
        {
            canvas.gameObject.GetChildren().ForEach(Destroy);

            return Instantiate(newPanelPrefab, canvas.transform);
        }
    }
}
