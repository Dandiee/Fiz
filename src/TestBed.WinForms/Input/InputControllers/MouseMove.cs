using System;
using SharpDX.Toolkit.Input;

namespace TestBed.WinForms.Input.InputControllers
{
    public class MouseMove : IInputController
    {
        private readonly Action<MouseState> _action;

        public MouseMove(Action<MouseState> action)
        {
            _action = action;
        }

        public void Update(InputState state)
        {
            if (state.MouseState.DeltaX != 0 || state.MouseState.DeltaY != 0)
            {
                _action(state.MouseState);
            }
        }
    }
}
