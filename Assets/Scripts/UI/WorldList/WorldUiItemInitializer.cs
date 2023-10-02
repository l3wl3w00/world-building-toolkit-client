using System;
using TMPro;
using UnityEngine;

namespace UI.WorldList
{
    public class WorldUiItemInitializer : MonoBehaviour
    {
        public void Initialize(string worldName, Guid guid)
        {
            GetComponentInChildren<DeleteWorldButton>().Id = guid;
            GetComponentInChildren<EditWorldButton>().Id = guid;
            transform.Find("WorldName").GetComponent<TextMeshProUGUI>().text = worldName;
        }
    }
}