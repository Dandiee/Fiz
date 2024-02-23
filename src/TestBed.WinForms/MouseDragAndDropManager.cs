using System.Linq;
using Common;
using Physics;
using Physics.Bodies;
using Physics.Joints;

namespace TestBed.WinForms
{
    public class MouseDragAndDropManager
    {
        public Body HoveredBody { get; private set; }

        private readonly World _world;
        private MouseJoint _mouseJoint;
        private bool _isDragAndDropping;

        public MouseDragAndDropManager(World world)
        {
            _world = world;
        }

        public void Update(Vector2 objectSpaceCursorPosition)
        {
            HoveredBody =
                _world.GetBodies().Where(b => !b.IsLocked)
                    .FirstOrDefault(body => body.IsPointInside(objectSpaceCursorPosition));

            if (_isDragAndDropping)
            {
                Drag(objectSpaceCursorPosition);
            }
        }

        public void DragStart(Vector2 objectSpaceCursorPosition)
        {
            if (HoveredBody != null)
            {
                _mouseJoint = new MouseJoint(HoveredBody, objectSpaceCursorPosition);
                _world.Add(_mouseJoint);
                _isDragAndDropping = true;
            }
        }

        private void Drag(Vector2 objectSpaceCursorPosition)
        {
            if (_isDragAndDropping)
            {
                _mouseJoint.Target = objectSpaceCursorPosition;
            }
        }

        public void DragEnd()
        {
            _isDragAndDropping = false;
            _world.Remove(_mouseJoint);
            _mouseJoint = null;
        }
    }
}
