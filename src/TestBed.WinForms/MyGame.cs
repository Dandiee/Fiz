using System;
using System.Diagnostics;
using Common;
using Physics;
using Physics.Helpers;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.XInput;
using TestBed.WinForms.DrawableShapes;
using TestBed.WinForms.Input;
using TestBed.WinForms.Input.InputControllers;

namespace TestBed.WinForms
{
    public partial class MyGame : Game
    {
        private readonly Dx.GraphicsDeviceManager _graphicsDeviceManager;
        private readonly KeyboardManager _keyboardManager;
        private readonly MouseManager _mouseManager;

        public PerspectiveCamera Camera { get; private set; }
        public World World { get; private set; }

        private Drawer _drawer;
        private MouseDragAndDropManager _mouseDragAndDropManager;
        private ScenarioFactory _scenarioFactory;

        public MyGame()
        {
            var _graphicsDeviceManager = new Dx.GraphicsDeviceManager(this);
            _keyboardManager = new KeyboardManager(this);
            _mouseManager = new MouseManager(this);
        }

        protected override void Initialize()
        {


            //Ray r;
            //Ray r2;
            //Vector3 v;
            //r2.Intersects(ref r, out v);
            Window.ClientSizeChanged += (sender, args) => Camera.UpdateViewport(new Common.Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height));
            IsMouseVisible = true;
            Camera = new PerspectiveCamera(new Common.Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height));
            World = new World();
            _drawer = new Drawer(this);
            //TreeInfoWindow treeInfoWindow = new TreeInfoWindow();
            //treeInfoWindow.Tree = World._tree;
            //
            //treeInfoWindow.Init();
            //treeInfoWindow.Show();
            InitializeDrawings();

            _scenarioFactory = new ScenarioFactory(World, Camera);
            _mouseDragAndDropManager = new MouseDragAndDropManager(World);
            // CameraComponent
            InputControllerComponent.RegisterController("Camera", new KeyDown(Keys.W, () => Camera.MoveTo(new Common.Vector2(Camera.Position.X, Camera.Position.Y) + Camera.MoveSpeed *  Common.Vector2.UnitY), ToggleKeyTypes.Continous));
            InputControllerComponent.RegisterController("Camera", new KeyDown(Keys.S, () => Camera.MoveTo(new Common.Vector2(Camera.Position.X, Camera.Position.Y) + Camera.MoveSpeed * -Common.Vector2.UnitY), ToggleKeyTypes.Continous));
            InputControllerComponent.RegisterController("Camera", new KeyDown(Keys.A, () => Camera.MoveTo(new Common.Vector2(Camera.Position.X, Camera.Position.Y) + Camera.MoveSpeed * -Common.Vector2.UnitX), ToggleKeyTypes.Continous));
            InputControllerComponent.RegisterController("Camera", new KeyDown(Keys.D, () => Camera.MoveTo(new Common.Vector2(Camera.Position.X, Camera.Position.Y) + Camera.MoveSpeed *  Common.Vector2.UnitX), ToggleKeyTypes.Continous));
            InputControllerComponent.RegisterController("Camera", new KeyDown(Keys.Q, () => Camera.Zoom( 50f), ToggleKeyTypes.Continous));
            InputControllerComponent.RegisterController("Camera", new KeyDown(Keys.E, () => Camera.Zoom(-50f), ToggleKeyTypes.Continous));
            InputControllerComponent.RegisterController("Camera", new KeyDown(Keys.R, () => Camera.Reset(), ToggleKeyTypes.Continous));
            InputControllerComponent.RegisterController("Camera", new MouseWheel(delta => Camera.Zoom(delta)));
            InputControllerComponent.RegisterController("Camera", new MouseMove(state => Camera.UpdateCursor(new Common.Vector2(state.X, state.Y))));
            InputControllerComponent.RegisterController("Camera", new MouseButton(MouseButtons.Right, MouseButtonStates.Pressed, state => Camera.StartDragging()));
            InputControllerComponent.RegisterController("Camera", new MouseButton(MouseButtons.Right, MouseButtonStates.Released, state => Camera.EndDragging()));

            // MouseDragAndDropManager
            InputControllerComponent.RegisterController("MouseD&D", new MouseButton(MouseButtons.Left, MouseButtonStates.Pressed, state => _mouseDragAndDropManager.DragStart(Camera.ObjectSpaceCursorPosition)));
            InputControllerComponent.RegisterController("MouseD&D", new MouseMove(state => _mouseDragAndDropManager.Update(Camera.ObjectSpaceCursorPosition)));
            InputControllerComponent.RegisterController("MouseD&D", new MouseButton(MouseButtons.Left, MouseButtonStates.Released, state => _mouseDragAndDropManager.DragEnd()));

            // Settings
            InputControllerComponent.RegisterController("Settings", new KeyDown(Keys.Home, () => Settings.PositionSolverIterations++, ToggleKeyTypes.Discrete));
            InputControllerComponent.RegisterController("Settings", new KeyDown(Keys.End, () => Settings.PositionSolverIterations--, ToggleKeyTypes.Discrete));
            InputControllerComponent.RegisterController("Settings", new KeyDown(Keys.PageUp, () => Settings.VelocitySolverIterations++, ToggleKeyTypes.Discrete));
            InputControllerComponent.RegisterController("Settings", new KeyDown(Keys.PageDown, () => Settings.VelocitySolverIterations--, ToggleKeyTypes.Discrete));

            InputControllerComponent.RegisterController("InGame", new KeyDown(Keys.Enter, () => World.Add(BodyBuilder.BuildBox(new Common.Vector2(1), Camera.ObjectSpaceCursorPosition)), ToggleKeyTypes.Discrete));
        }

        private void UpdateInputStates()
        {
            var controller = new Controller(UserIndex.One);
            InputControllerComponent.Update(new InputState(_keyboardManager.GetState(), _mouseManager.GetState(),
                controller.IsConnected ? controller.GetState().Gamepad : (Gamepad?) null));
        }

        protected override void Update(GameTime time)
        {
            base.Update(gameTime);
            UpdateInputStates();

            World.Step(Settings.TimeStep);
            
            
        }

        protected override void Draw(GameTime time)
        {
            GraphicsDevice.Clear(Color.Black);
            ClearDrawings();

            CreateDrawingPrimitives();
            _drawer.Update(time);
            _drawer.Draw(time);

            base.Draw(time);
        }
    }
}
