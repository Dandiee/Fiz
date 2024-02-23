using System.Collections.Generic;
using Physics;
using Physics.Bodies;
using Physics.Collisions.Manifolds;
using Physics.Joints;
using SharpDX;
using TestBed.WinForms.DrawableShapes.Solids;
using TestBed.WinForms.DrawableShapes.Wireframes;
using Circle = Physics.Bodies.Circle;
using Vector2 = Common.Vector2;

namespace TestBed.WinForms
{
    public partial class MyGame
    {
        public bool ShowContactPoints = false;
        public bool ShowBoundingCircles = false;
        public bool ShowStaticBodyMarkers = false;
        public bool DrawTriangles = false;
        public bool ShowOnlyVisibleConstraings = false;
        public bool ShowBoundingBoxes = true;
        public bool ShowTree = true;
        public bool FillTree = true;

        private static readonly System.Random _random = new System.Random();
        public static readonly Color PolygonColor = Color.White;
        public static readonly Color CircleColor = Color.White;
        public static readonly Color HoveredBodyColor = Color.Green;
        public static readonly Color RevoluteJointColor = Color.Orange;
        public static readonly Color PinJointColor = Color.Orange;
        public static readonly Color MouseJointColor = Color.LightGreen;
        public static readonly Color MotorJointColor = Color.Purple;
        public static readonly Color DistanceJointColor = Color.Brown;
        public static readonly Color WeldJointColor = Color.Yellow;
        public static readonly Color AxisJointColor = Color.Violet;
        public static readonly Color BoundingCircleColor = Color.DarkGray;
        public static readonly Color ContactPointColor = Color.Red;
        public static readonly Color StaticBodyMarkerColor = new Color(.2f);
        public static readonly Color TriangleColor = Color.Chocolate;
        public static readonly Color PrimitiveColor = Color.White;
        public static readonly Color TreeColor = Color.YellowGreen;

        private List<Line> _lines;
        private List<Cross> _crosses;
        private List<Triangle> _triangles;
        private List<Arrow> _arrows;
        private List<DrawableShapes.Wireframes.Circle> _circles;

        private void InitializeDrawings()
        {
            _lines = new List<Line>();
            _triangles = new List<Triangle>();
            _crosses = new List<Cross>();
            _arrows = new List<Arrow>();
            _circles = new List<DrawableShapes.Wireframes.Circle>();
        }

        private void ClearDrawings()
        {
            _lines.ForEach(e => e.Dispose());
            _crosses.ForEach(e => e.Dispose());
            _triangles.ForEach(e => e.Dispose());
            _arrows.ForEach(e => e.Dispose());
            _circles.ForEach(e => e.Dispose());

            _lines.Clear();
            _crosses.Clear();
            _triangles.Clear();
            _arrows.Clear();
            _circles.Clear();
        }

        private void DrawBox(Vector2 min, Vector2 max, Color color)
        {
            _lines.Add(new Line(new Vector2(min.X, min.Y), new Vector2(min.X, max.Y), color));
            _lines.Add(new Line(new Vector2(min.X, max.Y), new Vector2(max.X, max.Y), color));
            _lines.Add(new Line(new Vector2(max.X, max.Y), new Vector2(max.X, min.Y), color));
            _lines.Add(new Line(new Vector2(max.X, min.Y), new Vector2(min.X, min.Y), color));
        }



        public void CreateDrawingPrimitives()
        {
            _crosses.Add(new Cross(Vector2.Zero, Color.Red, 0.1f));


            //DrawAabb(Color.Yellow, World._tree.Root.Children[0].Children[0]);
            //DrawAabb(Color.Red, World._tree.Root.Children[0].Children[1]);
            //DrawAabb(Color.Green, World._tree.Root.Children[0].Children[1].Children[0]);
            foreach (var body in World.GetBodies())
            {
                if (body is Polygon)
                    DrawPolygon(body as Polygon);
                else if (body is Circle)
                    DrawCircle(body as Circle);
                else if(body is Segment)
                    DrawSegment(body as Segment);

                if (ShowBoundingCircles)
                    DrawBoundingCircle(body);

                if (ShowBoundingBoxes && body is ClipableBody)
                    DrawBoundingBox(body as ClipableBody);
            }

            foreach (var joint in World.GetJoints())
            {
                if (ShowOnlyVisibleConstraings && !joint.IsVisible)
                    continue;

                if (joint is LineJoint)
                    DrawLineJoint(joint as LineJoint);
                else if (joint is DistanceJoint)
                    DrawDistanceJoint(joint as DistanceJoint);
                else if (joint is RevoluteJoint)
                    DrawRevoluteJoint(joint as RevoluteJoint);
                else if (joint is PulleyJoint)
                    DrawPulleyJoint(joint as PulleyJoint);
                else if (joint is WeldJoint)
                    DrawWeldJoint(joint as WeldJoint);
                else if (joint is MouseJoint)
                    DrawMouseJoint(joint as MouseJoint);
            }

            if (ShowContactPoints)
            {
                foreach (var bodyConnection in World.GetBodyConnections())
                {
                    foreach (var point in bodyConnection.Manifold.Points)
                    {
                        DrawContactPoint(point);
                    }
                }
            }

            //if (ShowTree)
            //{
            //    DrawTree();
            //}
        }

        //private void DrawTree()
        //{
        //    var tree = World.GetTree();
        //    DrawTreeNode(tree.Root);
        //}

        //private void DrawTreeNode(Node node)
        //{
        //    var min = node.GlobalMin;
        //    var max = node.GlobalMax;

        //    if(FillTree)
        //    {
        //        var color = new Color4(node.Color.X, node.Color.Y, node.Color.Z, .5f);
        //        _triangles.Add(new Triangle(min, new Vector2(max.X, min.Y), new Vector2(min.X, max.Y), color));
        //        _triangles.Add(new Triangle(new Vector2(max.X, min.Y), max, new Vector2(min.X, max.Y), color));
        //    }

        //    DrawBox(min, max, TreeColor);

        //    if (node.LeftNode != null)
        //    {
        //        DrawTreeNode(node.LeftNode);
        //    }

        //    if (node.RightNode != null)
        //    {
        //        DrawTreeNode(node.RightNode);
        //    }
        //}

        private void DrawBoundingBox(ClipableBody clipableBody)
        {
            var min = clipableBody.GlobalMin;
            var max = clipableBody.GlobalMax;
            DrawBox(min, max, Color.Red);
            
        }

        private void DrawSegment(Segment segment)
        {
            _lines.Add(new Line(segment.P1, segment.P2, PrimitiveColor));
        }

        private void DrawMouseJoint(MouseJoint mouseJoint)
        {
            var p1 = mouseJoint.Body1.ToGlobal(mouseJoint.R);
            var p2 = mouseJoint.Target;

            _lines.Add(new Line(p1, p2, MouseJointColor));
            _circles.Add(new DrawableShapes.Wireframes.Circle(p1, 0.03f, MouseJointColor));
        }

        private void DrawWeldJoint(WeldJoint weldJoint)
        {
            var p1 = weldJoint.Body1.Position;
            var p2 = weldJoint.Body1.ToGlobal(weldJoint.R1);
            var p3 = weldJoint.Body2.Position;

            _lines.Add(new Line(p1, p2));
            _lines.Add(new Line(p2, p3));
        }

        private void DrawPulleyJoint(PulleyJoint pulleyJoint)
        {
            var p1 = pulleyJoint.Body1.ToGlobal(pulleyJoint.R1);
            var p2 = pulleyJoint.A1;
            _lines.Add(new Line(p1, p2));

            var p3 = pulleyJoint.Body2.ToGlobal(pulleyJoint.R2);
            var p4 = pulleyJoint.A2;
            _lines.Add(new Line(p3, p4));


            _lines.Add(new Line(p2, p4));

        }

        private void DrawRevoluteJoint(RevoluteJoint revoluteJoint)
        {
            var p1 = revoluteJoint.Body1.Position;
            var p2 = revoluteJoint.Body1.ToGlobal(revoluteJoint.R1);
            var p3 = revoluteJoint.Body2.Position;
            _circles.Add(new DrawableShapes.Wireframes.Circle(p2, 0.1f));
            _lines.Add(new Line(p1, p2));
            _lines.Add(new Line(p2, p3));

        }

        private void DrawDistanceJoint(DistanceJoint distanceJoint)
        {
            var p1 = distanceJoint.Body1.ToGlobal(distanceJoint.R1);
            var p2 = distanceJoint.Body2.ToGlobal(distanceJoint.R2);
            _circles.Add(new DrawableShapes.Wireframes.Circle(p1, 0.1f));
            _circles.Add(new DrawableShapes.Wireframes.Circle(p2, 0.1f));
            _lines.Add(new Line(p1, p2));
        }

        private void DrawLineJoint(LineJoint lineJoint)
        {
            var p1 = lineJoint.Body1.Position;
            var p2 = lineJoint.Body1.ToGlobal(lineJoint.R1);

            _lines.Add(new Line(p1, p2, Color.Red));

            var p3 = lineJoint.Body2.Position;
            var p4 = lineJoint.Body2.ToGlobal(lineJoint.R2);
            _lines.Add(new Line(p3, p4));

            _lines.Add(new Line(p2, p4));
        }

      

        //private void DrawWeldJoint(WeldJoint WeldJoint)
        //{
        //    var p1 = WeldJoint.Body1.Position;
        //    var p2 = WeldJoint.Body2.Position;
        //    
        //    var p3 = Vector2.Rotate(WeldJoint.Body1.Rotation, WeldJoint.LocalAnchor1) + WeldJoint.Body1.Position;
        //
        //    _lines.Add(new Line(p1, p3, WeldJointColor));
        //    _lines.Add(new Line(p2, p3, WeldJointColor));
        //
        //    _circles.Add(new DrawableShapes.Wireframes.Circle(p3, 0.02f, WeldJointColor));
        //}

        private void DrawBoundingCircle(Body body)
        {
            _circles.Add(new DrawableShapes.Wireframes.Circle(body.Position, body.BoundingCircleRadius, BoundingCircleColor));
        }

        private void DrawContactPoint(ManifoldPoint contactPoint)
        {
            _circles.Add(new DrawableShapes.Wireframes.Circle(contactPoint.GlobalVertex, 0.05f, ContactPointColor));
        }

        public void DrawPolygon(Polygon polygon)
        {
            var isCursorOn = _mouseDragAndDropManager.HoveredBody != null && _mouseDragAndDropManager.HoveredBody == polygon;
            var color = isCursorOn ? HoveredBodyColor : PolygonColor;

            for (var i = 0; i < polygon.GlobalVertices.Length; i++)
            {
                var p1 = polygon.GlobalVertices[i];
                var p2 = polygon.GlobalVertices[i == polygon.GlobalVertices.Length - 1 ? 0 : i + 1];
                if (DrawTriangles)
                {
                    _triangles.Add(new Triangle(p1, p2, polygon.Position, TriangleColor));
                }
                _lines.Add(new Line(p1, p2, color));
            }

            

            if (ShowStaticBodyMarkers && polygon.IsLocked)
            {
                foreach (var vertex in polygon.GlobalVertices)
                {
                    _lines.Add(new Line(polygon.Position, vertex, StaticBodyMarkerColor));
                }
            }
        }

        public void DrawCircle(Circle circle)
        {
            var isCursorOn = _mouseDragAndDropManager.HoveredBody != null && _mouseDragAndDropManager.HoveredBody == circle;
            var color = isCursorOn ? HoveredBodyColor : CircleColor;

            _circles.Add(new DrawableShapes.Wireframes.Circle(circle.Position, circle.Radius, color));
            _lines.Add(new Line(circle.Position, circle.Position + Vector2.Rotate(circle.Rotation, Vector2.UnitX * circle.Radius), color));

            if (ShowStaticBodyMarkers && circle.IsLocked)
            {
                var topLeft = Vector2.TopLeft*circle.Radius + circle.Position;
                var topRight = Vector2.TopRight * circle.Radius + circle.Position;
                var bottomLeft = Vector2.BottomLeft * circle.Radius + circle.Position;
                var bottomRight = Vector2.BottomRight * circle.Radius + circle.Position;

                _lines.Add(new Line(topLeft, bottomRight, StaticBodyMarkerColor));
                _lines.Add(new Line(topRight, bottomLeft, StaticBodyMarkerColor));
            }
        }
    }
}
