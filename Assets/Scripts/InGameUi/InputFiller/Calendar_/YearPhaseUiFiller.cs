#nullable enable
using Common.Model;
using Common.Model.Abstractions;
using Common.Utils;
using Game.Common;
using Game.Common.Holder;
using InGameUi.Util;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace InGameUi.InputFiller.Calendar_
{
    public class YearPhaseUiFiller : MonoBehaviour, IStateObserver
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text dayCountText;
        private ModelIdHolder<Calendar> _calendarModelIdHolder;
        [Inject] private ModelCollection<Calendar> _calendars;

        public void Construct(ModelIdHolder<Calendar> calendarModelIdHolder)
        {
            _calendarModelIdHolder = calendarModelIdHolder;
        }
        public void UpdateValue()
        {
            
            var parentDirectlyUnderContentOpt = transform.GetParentDirectlyUnderContentMaybe();
            if (parentDirectlyUnderContentOpt.NoValueOut(out var parentDirectlyUnderContent))
            {
                Debug.LogWarning($"Tried to update a {nameof(YearPhaseUiFiller)} ('{gameObject.name}') that was not under a 'Content'", gameObject);
                return;
            }

            var calendar = _calendarModelIdHolder.Value.GetModel(_calendars);
            var index = parentDirectlyUnderContent!.GetSelfIndexInParent().ToInt();
            if (index >= calendar.YearPhases.Count)
            {
                return;
            }
            
            var yearPhase = calendar.YearPhases[index];
            nameText.text = yearPhase.Name;
            dayCountText.text = "Number of days: " + yearPhase.NumberOfDays.ToString();
        }
    }
}