using SharpDX.Toolkit.Input;
using SharpDX.XInput;

namespace TestBed.WinForms.Input
{
    public class InputState
    {
        public KeyboardState KeyboardState { get; private set; }
        public MouseState MouseState { get; private set; }
        public Gamepad? GamepadState { get; private set; }

        public InputState(KeyboardState keyboardState, MouseState mouseState, Gamepad? gamepadState)
        {
            KeyboardState = keyboardState;
            MouseState = mouseState;
            GamepadState = gamepadState;
        }
    }
}
