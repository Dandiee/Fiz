using System;

namespace TestBed.WinForms.Input.InputControllers
{
    public class MouseWheel : IInputController
    {
        public Action<int> Action { get; private set; }

        public MouseWheel(Action<int> action)
        {
            Action = action;
        }

        public void Update(InputState state)
        {
            if (state.MouseState.WheelDelta != 0)
                Action.Invoke(state.MouseState.WheelDelta);
        }
    }
}
