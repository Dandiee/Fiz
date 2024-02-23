using System;
using System.Collections.Generic;
using Physics;
using Scenarios;
using Scenarios.Implementations;
using SharpDX.Toolkit.Input;
using TestBed.WinForms.Input;
using TestBed.WinForms.Input.InputControllers;
using Common;

namespace TestBed.WinForms
{
    public class ScenarioFactory
    {
        private readonly World _world;
        private readonly PerspectiveCamera _camera;

        public BaseScenario CurrentScenario { get; private set; }

        public ScenarioFactory(World world, PerspectiveCamera camera)
        {
            _world = world;
            _camera = camera;
            InitializeControllers();
        }

        private void InitializeControllers()
        {
            //RegisterScenario(Keys.F1, world => new BoundingVolumeHierarchyScenario(world),
            //    new Func<BoundingVolumeHierarchyScenario, IInputController>[]
            //    {
            //        scenario => new MouseButton(MouseButtons.Middle, MouseButtonStates.Pressed, (s) => scenario.CreateBox(_camera.UnprojectScreenPosition(new Vector2(s.X, s.Y))))
            //    });

            RegisterScenario(Keys.F1, world => new PinJointScenario(world));


            RegisterScenario(Keys.F3, world => new BoxPyramidScenario(world), 
                new Func<BoxPyramidScenario, IInputController>[]
                {
                    scenario => new KeyDown(Keys.Add, scenario.AddBoxToBaseRow, ToggleKeyTypes.Discrete),
                    scenario => new KeyDown(Keys.Subtract, scenario.RemoveBoxToBaseRow, ToggleKeyTypes.Discrete),
                });

            RegisterScenario(Keys.F6, world => new SpawningBoxesScenario(world),
              new Func<SpawningBoxesScenario, IInputController>[]
                {
                    scenario => new KeyDown(Keys.Space, scenario.SpawnBox, ToggleKeyTypes.Continous)
                });

           
            RegisterScenario(Keys.F2, world => new CombustionEngineScenario(world),
                new Func<CombustionEngineScenario, IInputController>[]
                {
                     scenario => new KeyDown(Keys.Add, scenario.AddCylinder, ToggleKeyTypes.Discrete),
                     scenario => new KeyDown(Keys.Subtract, scenario.RemoveCylinder, ToggleKeyTypes.Discrete),
                },
                scenario =>
                {
                    //InputControllerComponent.RegisterController("ScenarioFactory", new KeyDown(Keys.Right,  scenario.RotateRight, ToggleKeyTypes.Continous));
                });

            RegisterScenario(Keys.F2, world => new MotorbikeScenario(world),
                new Func<MotorbikeScenario, IInputController>[]
                {
                    scenario => new ToggleKeyDown(Keys.Up, scenario.Accelerate, scenario.Idling, ToggleKeyTypes.Discrete),
                    scenario => new ToggleKeyDown(Keys.Down, scenario.Break, scenario.Idling, ToggleKeyTypes.Discrete),
                    scenario => new KeyDown(Keys.Left, scenario.RotateLeft, ToggleKeyTypes.Continous),
                    scenario => new KeyDown(Keys.Right, scenario.RotateRight, ToggleKeyTypes.Continous)
                },
                scenario =>
                {
                    if (scenario.BikeFrame != null)
                    {
                        scenario.BikeFrame.Moving += (sender, args) => _camera.MoveTo(args.NewPosition);
                    }
                });

            
            //RegisterScenario(Keys.F2, world => new BallBathScenario(world));
            
            RegisterScenario(Keys.F4, world => new DominoScenario(world));
            
          

            RegisterScenario(Keys.F7, world => new SuspendedVehicleScenario(world));
            RegisterScenario(Keys.F8, world => new TankScenario(world),
                new Func<TankScenario, IInputController>[]
                {
                    scenario => new KeyDown(Keys.LeftControl, scenario.LancFeszites, ToggleKeyTypes.Continous),
                    scenario => new KeyDown(Keys.Space, scenario.LancCsatolas, ToggleKeyTypes.Discrete)
                });

            RegisterScenario(Keys.F9, world => new DominoTowerScenario(world),
                new Func<DominoTowerScenario, IInputController>[]
                {
                    scenario => new KeyDown(Keys.Add, scenario.AddRow, ToggleKeyTypes.Discrete),
                    scenario => new KeyDown(Keys.Subtract, scenario.RemoveRow, ToggleKeyTypes.Discrete)
                });
            RegisterScenario(Keys.F10, world => new VaryingFrictionScenario(world));
            RegisterScenario(Keys.F11, world => new VerticalBoxStack(world));
            RegisterScenario(Keys.F12, world => new VerticalCircleStack(world));
        }

        private void RegisterScenario<TScenario>(Keys key, Func<World, TScenario> scenarioAction, IEnumerable<Func<TScenario, IInputController>> controllers = null, Action<TScenario> initalizeActions = null)
            where TScenario : BaseScenario
        {
            var controller = new KeyDown(key, () => InitializeScenario(scenarioAction, controllers, initalizeActions), ToggleKeyTypes.Continous);
            if (CurrentScenario == null)
            {
                controller.Action();
            }

            InputControllerComponent.RegisterController("ScenarioFactory", controller);
        }

        private void InitializeScenario<TScenario>(Func<World, TScenario> scenarioAction, IEnumerable<Func<TScenario, IInputController>> controllers, Action<TScenario> initalizeActions = null)
           where TScenario : BaseScenario
        {
            if (CurrentScenario != null)
            {
                InputControllerComponent.DeregisterControllers(CurrentScenario.GetType().ToString());
            }

            _world.Clear();

            var scenario = scenarioAction(_world);
            if (initalizeActions != null)
                initalizeActions(scenario);

            if (controllers != null)
                foreach (var controller in controllers)
                    InputControllerComponent.RegisterController(scenario.ToString(), controller.Invoke(scenario));

            CurrentScenario = scenario;
        }
    }
}
