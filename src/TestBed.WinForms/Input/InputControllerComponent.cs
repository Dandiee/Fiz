using System.Collections.Generic;
using System.Linq;

namespace TestBed.WinForms.Input
{
    public static class InputControllerComponent
    {
        private static readonly Dictionary<string, List<IInputController>> _controllers;

        static InputControllerComponent()
        {
            _controllers = new Dictionary<string, List<IInputController>>();
        }

        public static void Update(InputState inputState)
        {
            foreach (var controller in _controllers.ToList())
            {
                foreach (var inputController in controller.Value.ToList())
                {
                    inputController.Update(inputState);
                }
            }
        }

        public static void RegisterController(string key, IInputController controller)
        {
            List<IInputController> inputControllers;
            if (_controllers.TryGetValue(key, out inputControllers))
            {
                if (inputControllers == null)
                {
                    inputControllers = new List<IInputController>();
                    _controllers[key] = inputControllers;
                }

                inputControllers.Add(controller);
            }
            else
            {
                inputControllers = new List<IInputController> { controller };
                _controllers[key] = inputControllers;
            }
        }

        public static void DeregisterControllers(string key)
        {
            _controllers.Remove(key);
        }
    }
}
