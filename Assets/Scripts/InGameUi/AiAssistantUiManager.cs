#nullable enable
using System;
using Common.ButtonBase;
using Common.Model;
using GluonGui.WorkspaceWindow.Views.WorkspaceExplorer;
using PlasticGui.WorkspaceWindow.CodeReview.Summary;
using TMPro;
using UnityEngine;
using Zenject;

namespace InGameUi
{
    public class AiAssistantUiManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField promptInput;
        [SerializeField] private TMP_Text answerText;

        private bool _updateQueued = false;

        public AiAssistant AiAssistant { get; private set; } = AiAssistant.Default();

        public void SetAnswer(string answer)
        {
            AiAssistant = AiAssistant with { Answer = answer };
            _updateQueued = true;
        }

        public void SetPrompt(string prompt)
        {
            AiAssistant = AiAssistant with { Question = prompt };
            _updateQueued = true;
        }

        public string Prompt => AiAssistant.Question;

        private void Start()
        {
            promptInput.onValueChanged.AddListener(SetPrompt);
        }

        private void Update()
        {
            if (!_updateQueued) return;
            _updateQueued = false;
            promptInput.SetTextWithoutNotify(AiAssistant.Question);
            AiAssistant.Answer.DoIfNotNull(a => answerText.text = a);
        }
    }
}