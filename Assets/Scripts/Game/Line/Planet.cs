using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TMPro;
using UnityEngine;

namespace WorldBuilder.Client.Game.Line
{
    internal class LineList : List<(LineAlongSphere line, LineRenderer renderer)> { }

    public delegate LineRenderer LineRendererFactory();

    public class Planet
    {
        private LineRendererFactory _lineRendererFactory;
        private readonly ICollection<(LineAlongSphere line, LineRenderer renderer)> _lines = new LineList();
        
        public Planet(LineRendererFactory lineRendererFactory)
        {
            _lineRendererFactory = lineRendererFactory;
        }

        public Planet(ICollection<(LineAlongSphere line, LineRenderer renderer)> lines)
        {
            _lines = lines;
        }

        public IEnumerable<LineRenderer> LineRenderers => _lines.Select(l => l.renderer);

        public LineRendererFactory RendererFactory
        {
            get => _lineRendererFactory;
            set => _lineRendererFactory = value;
        }

        public void AddLine(LineAlongSphere line)
        {
            _lines.Add((line, _lineRendererFactory.Invoke()));
        }

        public void RenderUpdatedLines()
        {
            foreach (var (line, renderer) in _lines)
            {
                line.Update();
                line.Render(renderer);
            }
        }

        public void DeleteLine(LineAlongSphere line)
        {
            var lineToRemove = _lines.Single(pair => pair.line == line);
            _lines.Remove(lineToRemove);
            Object.Destroy(lineToRemove.renderer.gameObject);
        }
    }
}