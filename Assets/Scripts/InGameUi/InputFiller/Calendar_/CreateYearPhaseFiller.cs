#nullable enable
using Common;
using Common.Model;
using Common.Model.Builder;
using Common.Utils;
using InGameUi.Util;
using TMPro;
using UnityEngine;
using Zenject;

namespace InGameUi.InputFiller.Calendar_
{
    public class CreateYearPhaseFiller : MonoBehaviour, IStateObserver
    {
        [SerializeField] private TMP_InputField nameInput = null!; // Asserted in Start
        [SerializeField] private TMP_InputField yearPhaseCountInput = null!; // Asserted in Start
        [Inject] private CalendarBuilderHolder _calendarBuilderHolder = null!; // Asserted in Start

        protected void Start()
        {
            NullChecker.AssertNoneIsNullInType(GetType(), nameInput, yearPhaseCountInput, _calendarBuilderHolder);
        }
        public void UpdateValue()
        {
            if (_calendarBuilderHolder.IsModelNotInCreation)
            {
                return;
            }
            
            var parentDirectlyUnderContentOpt = transform.GetParentDirectlyUnderContentMaybe();
            if (parentDirectlyUnderContentOpt.NoValueOut(out var parentDirectlyUnderContent))
            {
                Debug.LogWarning($"Tried to update a {nameof(CreateYearPhaseFiller)} ('{gameObject.name}') that was not under a 'Content'", gameObject);
                return;
            }

            var index = parentDirectlyUnderContent!.GetSelfIndexInParent().ToInt();
            if (index >= _calendarBuilderHolder.Builder.YearPhases.Count)
            {
                return;
            }
            
            var yearPhase = _calendarBuilderHolder.Builder.YearPhases[index];
            nameInput.text = yearPhase.Name;
            yearPhaseCountInput.text = yearPhase.NumberOfDays.ToString();
        }
    }
}