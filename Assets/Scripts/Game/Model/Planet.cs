#nullable enable
using System;
using UnityEngine;

namespace Game.Model
{
    public delegate LineRenderer LineRendererFactory();

    public struct Planet
    {
        public Planet(string id, string name = "", string description = "") : this(Guid.Parse(id), name, description)
        {
        }

        public Planet(Guid id, string name = "", string description = "")
        {
            Name = name;
            Description = description;
            Id = id;
        }

        public string Name { get; }
        public string Description { get; }
        public Guid Id { get; }
    }
}