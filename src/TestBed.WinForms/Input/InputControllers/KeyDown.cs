using System;
using SharpDX.Toolkit.Input;

namespace TestBed.WinForms.Input.InputControllers
{
    public class KeyDown : IInputController
    {
        public Keys Key { get; private set; }
        public Action Action { get; private set; }
        public ToggleKeyTypes Type { get; private set; }

        public KeyDown(Keys key, Action action, ToggleKeyTypes type)
        {
            Key = key;
            Action = action;
            Type = type;
        }

        public void Update(InputState state)
        {
            if (Type == ToggleKeyTypes.Continous && state.KeyboardState.IsKeyDown(Key))
                Action.Invoke();
            else if (Type == ToggleKeyTypes.Discrete && state.KeyboardState.IsKeyPressed(Key))
                Action.Invoke();
        }
    }
}
