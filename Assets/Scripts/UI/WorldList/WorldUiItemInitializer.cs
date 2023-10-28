#nullable enable
using System;
using Common.Model;
using Common.Model.Abstractions;
using TMPro;
using UnityEngine;

namespace UI.WorldList
{
    public class WorldUiItemInitializer : MonoBehaviour
    {
        public void Initialize(string worldName, IdOf<Planet> guid)
        {
            GetComponentInChildren<DeleteWorldButton>().Id = guid;
            GetComponentInChildren<EditWorldButton>().Id = guid;
            transform.Find("WorldName").GetComponent<TextMeshProUGUI>().text = worldName;
        }
    }
}