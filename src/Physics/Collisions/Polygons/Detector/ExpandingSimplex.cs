using System.Collections.Generic;
using Common;

namespace Physics.Collisions.Polygons.Detector
{
    public class ExpandingSimplex
    {
        private readonly int _winding;
        private readonly PriorityQueue<double, ExpandingSimplexEdge> _queue;

        public ExpandingSimplex(List<Vector2> simplex)
        {
            _winding = GetWinding(simplex);
            _queue = new PriorityQueue<double, ExpandingSimplexEdge>();

            for (var i = 0; i < simplex.Count; i++)
            {
                var a = simplex[i];
                var b = simplex[i + 1 == simplex.Count ? 0 : i + 1];
                var edge = new ExpandingSimplexEdge(a, b, _winding);
                _queue.Add(new KeyValuePair<double, ExpandingSimplexEdge>(edge.Distance, edge));
            }
        }

        public ExpandingSimplexEdge GetClosestEdge()
        {
            return _queue.Peek().Value;
        }

        public void Expand(Vector2 point)
        {
            var edge = _queue.Dequeue().Value;

            var edge1 = new ExpandingSimplexEdge(edge.Point1, point, _winding);
            var edge2 = new ExpandingSimplexEdge(point, edge.Point2, _winding);

            _queue.Add(new KeyValuePair<double, ExpandingSimplexEdge>(edge1.Distance, edge1));
            _queue.Add(new KeyValuePair<double, ExpandingSimplexEdge>(edge2.Distance, edge2));
        }

        private int GetWinding(List<Vector2> simplex)
        {
            for (var i = 0; i < simplex.Count; i++)
            {
                var a = simplex[i];
                var b = simplex[i + 1 == simplex.Count ? 0 : i + 1];

                var cross = Vector2.Cross(a, b);

                if (cross > 0)
                    return 1;
                
                if (cross < 0)
                    return -1;
            }

            return 0;
        }
    }
}
