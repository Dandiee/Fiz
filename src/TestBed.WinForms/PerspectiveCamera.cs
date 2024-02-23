using System;
using Common;

namespace TestBed.WinForms
{
    public class PerspectiveCamera
    {
        public Vector3 Position { get; set; }
        public Vector3 LookAt { get; set; }

        public Matrix4x4 ProjectionMatrix { get; private set; }
        public Matrix4x4 ViewMatrix { get; private set; }
        public Matrix4x4 WorldMatrix { get; private set; }
        public Matrix4x4 WorldViewProjectionTransposed { get; set; }
        public Matrix4x4 WorldViewProjection { get; set; }

        public Vector2 ObjectSpaceCursorPosition { get; private set; }
        public Vector2 ScreenSpaceCursorPosition { get; private set; }

        public float MoveSpeed { get; set; }
        public float ZNear { get; set; }
        public float ZFar { get; set; }
        public float FieldOfView { get; set; }

        private bool _isDragging;
        private Vector2 _draggedObjectSpacePosition;
        private Vector2 _draggedScreenSpacePosition;
        private Vector2 _viewportSize;

        private float _aspectRatio;

        public PerspectiveCamera(Vector2 viewportSize)
        {
            UpdateViewport(viewportSize);
            Reset();
            UpdateMatrices();
        }

        public void UpdateViewport(Vector2 viewportSize)
        {
            _viewportSize = viewportSize;
            _aspectRatio = _viewportSize.X / _viewportSize.Y;
        }

        public void Reset()
        {
            FieldOfView = MathUtil.PiOverFour;
            Position = new Vector3(0, 0, 80);
            LookAt = new Vector3(0, 0, -1);
            MoveSpeed = 2;
            ZNear = 0.01f;
            ZFar = 100000000000f;

            UpdateMatrices();
        }

        private void UpdateMatrices()
        {
            ProjectionMatrix = Matrix4x4.PerspectiveFovRH(FieldOfView, _aspectRatio, ZNear, ZFar);
            ViewMatrix = Matrix4x4.LookAtRH(Position, LookAt, Vector3.UnitY);
            WorldMatrix = Matrix4x4.Identity;
            WorldViewProjection = WorldMatrix * ViewMatrix * ProjectionMatrix;
            WorldViewProjectionTransposed = Matrix4x4.Transpose(WorldViewProjection);
        }

        public void MoveTo(Vector2 coordinate)
        {
            Position = new Vector3(coordinate.X, coordinate.Y, Position.Z);
            LookAt = new Vector3(Position.X, Position.Y, 0);
            UpdateMatrices();
        }

        public void Zoom(float step)
        {
            Position -= Vector3.UnitZ * step * 0.05f;
            if (Position.Z < 1)
                Position = new Vector3(Position.X, Position.Y, 1);
            UpdateMatrices();
        }

        public void StartDragging()
        {
            _isDragging = true;
            _draggedObjectSpacePosition = ObjectSpaceCursorPosition;
            _draggedScreenSpacePosition = ScreenSpaceCursorPosition;
        }

        public void Dragging(Vector2 mousePosition)
        {
            var objectSpaceDiff = ObjectSpaceCursorPosition - _draggedObjectSpacePosition;
            var objectSpaceDiffLength = objectSpaceDiff.Length();
            if (Math.Abs(objectSpaceDiffLength) < 0.0001f)
                return;

            var screenSpaceDiff = ScreenSpaceCursorPosition - _draggedScreenSpacePosition;
            var screenSpaceDiffLength = screenSpaceDiff.Length();

            var diff = screenSpaceDiffLength / objectSpaceDiffLength;

            var targetPosition = (screenSpaceDiff / -diff) * new Vector2(1, -1) + new Vector2(Position.X, Position.Y);
            MoveTo(targetPosition);
            UpdateCursorPosition(mousePosition);
            _draggedObjectSpacePosition = ObjectSpaceCursorPosition;
            _draggedScreenSpacePosition = ScreenSpaceCursorPosition;
        }

        public void EndDragging()
        {
            _isDragging = false;
        }

        public void UpdateCursor(Vector2 mousePosition)
        {
            UpdateCursorPosition(mousePosition);
            if (_isDragging)
            {
                Dragging(mousePosition);
            }
        }

        private void UpdateCursorPosition(Vector2 mousePosition)
        {
            ScreenSpaceCursorPosition = mousePosition * _viewportSize;

            var viewProjection = ViewMatrix * ProjectionMatrix;

            var p1 = new Vector3(ScreenSpaceCursorPosition.X, ScreenSpaceCursorPosition.Y, 0);
            var p2 = new Vector3(ScreenSpaceCursorPosition.X, ScreenSpaceCursorPosition.Y, -1);

            var q1 = Vector3.Unproject(p1, 0, 0, _viewportSize.X, _viewportSize.Y, 0, 1, viewProjection);
            var q2 = Vector3.Unproject(p2, 0, 0, _viewportSize.X, _viewportSize.Y, 0, 1, viewProjection);

            var diff = Vector3.Normalize(q2 - q1);
            var ray = new Ray(Position, -diff);
            var plane = new Plane(-Vector3.UnitZ, 0);

            Vector3 rayPlaneIntersectionPoint;
            if (MathUtil.RayIntersectsPlane(ray, plane, out rayPlaneIntersectionPoint))
            {
                ObjectSpaceCursorPosition = new Vector2(rayPlaneIntersectionPoint.X, rayPlaneIntersectionPoint.Y);
            }
        }

        public Vector2? UnprojectScreenPosition(Vector2 screenPosition)
        {
            var screenSpaceCursorPosition = screenPosition * _viewportSize;

            var viewProjection = ViewMatrix * ProjectionMatrix;

            var p1 = new Vector3(ScreenSpaceCursorPosition.X, ScreenSpaceCursorPosition.Y, 0);
            var p2 = new Vector3(ScreenSpaceCursorPosition.X, ScreenSpaceCursorPosition.Y, -1);

            var q1 = Vector3.Unproject(p1, 0, 0, _viewportSize.X, _viewportSize.Y, 0, 1, viewProjection);
            var q2 = Vector3.Unproject(p2, 0, 0, _viewportSize.X, _viewportSize.Y, 0, 1, viewProjection);

            var diff = Vector3.Normalize(q2 - q1);
            var ray = new Ray(Position, -diff);
            var plane = new Plane(-Vector3.UnitZ, 0);

            Vector3 rayPlaneIntersectionPoint;
            if (MathUtil.RayIntersectsPlane(ray, plane, out rayPlaneIntersectionPoint))
            {
                return new Vector2(rayPlaneIntersectionPoint.X, rayPlaneIntersectionPoint.Y);
            }

            return null;
        }
    }
}
