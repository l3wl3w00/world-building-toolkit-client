#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Common.Model;
using Common.Model.Query;
using Common.Triggers.GameController;
using UnityEngine;
using Zenject;

namespace Common
{
    public class NamespaceBasedCommandInstaller : MonoInstaller
    {
        [SerializeField] private List<string> namespaces = new();
        private IEnumerable<Type> GetComponentTypesToInstall()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => typeof(IActionListener).IsAssignableFrom(t) || typeof(IQuery).IsAssignableFrom(t))
                .Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericType)
                .Where(t => namespaces.Any(n => t.Namespace.StartsWith(n)));

        }
        public override void InstallBindings()
        {
            var commandContainer = new GameObject("Command Container");
            
            foreach (var type in GetComponentTypesToInstall())
            {
                Container.Bind(type).FromNewComponentOn(commandContainer).AsSingle().NonLazy();
            }

            AfterComponentsInstalled();
        }

        protected virtual void AfterComponentsInstalled()
        {
            
        }
    }
}