using System;
using System.Diagnostics;
using SharpDX.Toolkit.Input;

namespace TestBed.WinForms.Input.InputControllers
{
    public class ToggleKeyDown : IInputController
    {
        public Keys Key { get; private set; }
        public Action KeyDownAction { get; private set; }
        public Action KeyUpAction { get; private set; }
        public ToggleKeyTypes Type { get; private set; }

        public ToggleKeyDown(Keys key, Action keyDownAction, Action keyUpAction, ToggleKeyTypes type)
        {
            Key = key;
            KeyDownAction = keyDownAction;
            KeyUpAction = keyUpAction;
            Type = type;
        }

        public void Update(InputState state)
        {
            if (Type == ToggleKeyTypes.Continous)
            {
                if (state.KeyboardState.IsKeyDown(Key))
                {
                    KeyDownAction.Invoke();
                    Debug.WriteLine("Continous KeyDown");
                }
                else
                {
                    KeyUpAction.Invoke();
                    Debug.WriteLine("Continous KeyUp");
                }
            }
            else
            {
                if (state.KeyboardState.IsKeyDown(Key))
                {
                    KeyDownAction.Invoke();
                    Debug.WriteLine("Discrete KeyDown");
                }
                else if (state.KeyboardState.IsKeyReleased(Key))
                {
                    KeyUpAction.Invoke();
                    Debug.WriteLine("Discrete KeyUp");
                }
            }
        }
    }
}
