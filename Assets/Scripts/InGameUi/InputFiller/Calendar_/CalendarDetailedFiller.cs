#nullable enable
using Common;
using Common.Model;
using Common.Model.Abstractions;
using Common.Model.Query;
using Common.Utils;
using Game.Common;
using InGameUi.Factory;
using InGameUi.Util;
using TMPro;
using UnityEngine;
using Zenject;

namespace InGameUi.InputFiller.Calendar_
{
    public class CalendarDetailedFiller : MonoBehaviour, IStateObserver
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Text firstYearText;
        [Inject] private ModelCollection<Calendar> _calendars;
        [Inject] private YearPhaseUiItemFactory _factory;

        public void UpdateValue()
        { 
            var calendar = gameObject
                .GetIdOf<Calendar>()
                .GetModel(_calendars);
            nameText.text = calendar.Name;
            descriptionText.text = calendar.Description;
            firstYearText.text = "First year: " + calendar.FirstYear.ToString();

            var content = gameObject.GetContentInChildren();
            if (_calendars.Count != content.transform.childCount)
            {
                content.GetChildren().ForEach(Destroy);
                content.transform.DetachChildren();
                calendar.YearPhases.ForEach(c => _factory.Create(gameObject.GetModelIdHolder<Calendar>(), content.transform));
            }
            content.UpdateChildInputFillers();
        }
    }
}