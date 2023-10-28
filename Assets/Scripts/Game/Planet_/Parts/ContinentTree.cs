#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Geometry.Projection;
using Common.Geometry.Sphere;
using Common.Model;
using Common.Model.Abstractions;
using Game.Continent_;
using Game.Region_;
using ProceduralToolkit;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Planet_.Parts
{
    public class ContinentTree
    {
        private readonly Node _root;

        public IEnumerable<ContinentMonoBehaviour> Continents => _root.Select(r => r.MonoBehaviour);

        private ContinentTree(Node root)
        {
            _root = root;
        }

        public static ContinentTree Create(ICollection<Continent> continents, ISphere sphere, IContinentClickedListener continentClickedListener)
        {
            var root = continents
                .SingleOrDefault(c => c.ParentIdOpt.NoValue)
                .ToOption()
                .ExpectNotNull("No root continent was found");

            var rootMono = ContinentMonoBehaviour.Create(new(sphere, root, continentClickedListener));
            var rootNode = new Node(rootMono, 0);
            BuildTree(rootNode);
            return new ContinentTree(rootNode);

            void BuildTree(Node parentNode)
            {
                var childrenModels = parentNode.MonoBehaviour
                    .Continent
                    .GetChildren(continents);
                foreach (var childModel in childrenModels)
                {
                    var newMono = ContinentMonoBehaviour.Create(new(sphere, childModel, continentClickedListener));
                    var newNode = parentNode.AddChild(newMono);
                    
                    BuildTree(newNode);
                }
            }
        }

        
        internal void UpdatePlanetMeshes(MeshDraft planetDraft)
        {
            var sortedContinentsWithPolygons = _root
                .PostOrderTraversal()
                .Select(node => new ContinentInfoForMeshUpdate(node.MonoBehaviour, node.Depth))
                .ToArray();
            var lastIndex = sortedContinentsWithPolygons.Length - 1;
            sortedContinentsWithPolygons[lastIndex] = new ContinentInfoForMeshUpdate(_root.MonoBehaviour, 0, true);

            var regions = Object
                .FindObjectsOfType<RegionMonoBehaviour>()
                .Select(r => new RegionInfoForMeshUpdate(r))
                .ToArray();
            // must use traditional for loop or else this is very slow
            for (var i = 0; i < planetDraft.triangles.Count; i += 3)
            {
                var v1Index = planetDraft.triangles[i + 0];
                var v2Index = planetDraft.triangles[i + 1];
                var v3Index = planetDraft.triangles[i + 2];

                var v1 = planetDraft.vertices[v1Index];
                var v2 = planetDraft.vertices[v2Index];
                var v3 = planetDraft.vertices[v3Index];
                var vertices = (v1, v2, v3);
                
                foreach (var info in sortedContinentsWithPolygons)
                {

                    bool polygonContains;
                    unsafe
                    {
                        polygonContains = (info.MonoBehaviour.Inverted)
                            ? !info.PolygonOnSphere.ContainsUnsafe(&vertices)
                            : info.PolygonOnSphere.ContainsUnsafe(&vertices);
                    }

                    if (polygonContains)
                    {
                        info.Draft.AddTriangle(v1, v2, v3, true);
                        break;
                    }
                }

                foreach (var region in regions)
                {
                    bool polygonContains;
                    unsafe
                    {
                        polygonContains = (region.MonoBehaviour.Inverted)
                            ? !region.PolygonOnSphere.ContainsUnsafe(&vertices)
                            : region.PolygonOnSphere.ContainsUnsafe(&vertices);
                    }
                    if (polygonContains)
                    {
                        region.Draft.AddTriangle(v1, v2, v3, true);
                    }
                }
                // Debug.LogError($"None of the continents contains the triangle {i}, {i + 1}, {i + 2}");
            }
            
            foreach (var info in sortedContinentsWithPolygons)
            {
                info.PolygonOnSphere.Dispose();
                info.MonoBehaviour.UpdateMesh(info.Draft, info.Depth);
            }
            
            foreach (var region in regions)
            {
                region.PolygonOnSphere.Dispose();
                region.MonoBehaviour.UpdateMesh(region.Draft);
                region.MonoBehaviour.transform.localScale *= 1.0035f;
            }
        }

        internal ContinentMonoBehaviour CreateContinentWithParent(ContinentWithParent continent, ISphere sphere, IContinentClickedListener clickedListener)
        {
            var parentId = continent.ParentId;
            var parentNode = FindNodeById(parentId);
            var newMono = ContinentMonoBehaviour.Create(new(sphere, continent.ToContinent(), clickedListener));
            parentNode.AddChild(newMono);
            return newMono;
        }

        internal Node FindNodeById(IdOf<Continent> id)
        {
            return _root
                .SingleOrDefault(c => c.Id == id)
                .ToOption()
                .ExpectNotNull($"Continent was not found with id: {id}");
        }
        internal record Node(ContinentMonoBehaviour MonoBehaviour, int Depth) : IEnumerable<Node>
        {
            private readonly ICollection<Node> _children = new HashSet<Node>();

            internal ICollection<Node> Children => _children;

            internal IdOf<Continent> Id => MonoBehaviour.Continent.Id;

            internal Node AddChild(ContinentMonoBehaviour continentMonoBehaviour)
            {
                var newNode = new Node(continentMonoBehaviour, Depth + 1);
                Children.Add(newNode);
                return newNode;
            }

            internal Option<Node> FindFirst(Func<Node, bool> predicate)
            {
                if (predicate(this)) return this.ToOption();
                foreach (var child in Children)
                {
                    return child.FindFirst(predicate);
                }
                return Option<Node>.None;
            }
            
            public IEnumerator<Node> GetEnumerator()
            {
                return PostOrderTraversal().GetEnumerator();
            }

            internal IEnumerable<Node> PostOrderTraversal()
            {
                foreach (var child in Children)
                {
                    using var childEnumerator = child.GetEnumerator();
                    while (childEnumerator.MoveNext())
                    {
                        yield return childEnumerator.Current.ToOption().ExpectNotNull("Current Child enumerator was null");
                    }
                }

                yield return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        struct ContinentInfoForMeshUpdate
        {
            internal readonly ContinentMonoBehaviour MonoBehaviour;
            internal readonly PolygonOnSphere<StereographicProjector> PolygonOnSphere;
            internal readonly int Depth;
            internal readonly MeshDraft Draft;

            internal ContinentInfoForMeshUpdate(ContinentMonoBehaviour monoBehaviour, int depth, bool infinitePolygon = false)
            {
                this.MonoBehaviour = monoBehaviour;
                this.Depth = depth;
                if (infinitePolygon)
                {
                    PolygonOnSphere = PolygonOnSphere<StereographicProjector>.Infinite();
                }
                else
                {
                    PolygonOnSphere = MonoBehaviour.CreatePolygonOnSphere();
                }
                Draft = new MeshDraft();
            }
        }
        
        struct RegionInfoForMeshUpdate
        {
            internal readonly RegionMonoBehaviour MonoBehaviour;
            internal readonly PolygonOnSphere<StereographicProjector> PolygonOnSphere;
            internal readonly MeshDraft Draft;

            internal RegionInfoForMeshUpdate(RegionMonoBehaviour monoBehaviour)
            {
                MonoBehaviour = monoBehaviour;
                PolygonOnSphere = MonoBehaviour.CreatePolygonOnSphere();
                Draft = new MeshDraft();
            }
        }
    }
}