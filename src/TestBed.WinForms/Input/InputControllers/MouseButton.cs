using System;
using SharpDX.Toolkit.Input;

namespace TestBed.WinForms.Input.InputControllers
{
    public class MouseButton : IInputController
    {
        public Action<MouseState> Action { get; private set; }
        public MouseButtonStates Type { get; private set; }
        public MouseButtons Button { get; private set; }

        public MouseButton(MouseButtons button, MouseButtonStates type, Action<MouseState> action)
        {
            Action = action;
            Type = type;
            Button = button;
        }

        public void Update(InputState state)
        {
            if (Button == MouseButtons.Left)
            {
                if (Type == MouseButtonStates.Down && state.MouseState.LeftButton.Down)
                    Action.Invoke(state.MouseState);
                else if (Type == MouseButtonStates.Pressed && state.MouseState.LeftButton.Pressed)
                    Action.Invoke(state.MouseState);
                else if (Type == MouseButtonStates.Released && state.MouseState.LeftButton.Released)
                    Action.Invoke(state.MouseState);
            }

            if (Button == MouseButtons.Right)
            {
                if (Type == MouseButtonStates.Down && state.MouseState.RightButton.Down)
                    Action.Invoke(state.MouseState);
                else if (Type == MouseButtonStates.Pressed && state.MouseState.RightButton.Pressed)
                    Action.Invoke(state.MouseState);
                else if (Type == MouseButtonStates.Released && state.MouseState.RightButton.Released)
                    Action.Invoke(state.MouseState);
            }

            if (Button == MouseButtons.Middle)
            {
                if (Type == MouseButtonStates.Down && state.MouseState.MiddleButton.Down)
                    Action.Invoke(state.MouseState);
                else if (Type == MouseButtonStates.Pressed && state.MouseState.MiddleButton.Pressed)
                    Action.Invoke(state.MouseState);
                else if (Type == MouseButtonStates.Released && state.MouseState.MiddleButton.Released)
                    Action.Invoke(state.MouseState);
            }
        }
    }
}

