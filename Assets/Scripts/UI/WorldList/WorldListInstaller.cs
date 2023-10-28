#nullable enable
using Common;
using Common.Utils;
using Generated;
using UnityEngine;

namespace UI.WorldList
{
    public class WorldListInstaller : NamespaceBasedCommandInstaller
    {
        protected override void AfterComponentsInstalled()
        {
            base.AfterComponentsInstalled();
            Container.Bind<GameObject>().FromComponentInNewPrefab(Prefab.WorldUiItem.Load()).AsSingle();
        }
    }
}