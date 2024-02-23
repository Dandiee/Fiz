using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Common;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using TestBed.WinForms.DrawableShapes.Solids.Base;
using TestBed.WinForms.DrawableShapes.Vertices;
using TestBed.WinForms.DrawableShapes.Wireframes.Base;
using TestBed.WinForms.Extensions;

namespace TestBed.WinForms.DrawableShapes
{
    public class Drawer
    {
        private const string FileName = "LineDrawer.hlsl";

        private readonly MyGame _myGame;
        private PixelShader _pixelShader;
        private VertexShader _vertexShader;
        private InputLayout _layout;
        private Buffer _constantBuffer;
        private readonly DeviceContext _context;
        private ShaderContantBuffer _shaderParameters;
        
        private readonly int _vertexSizeInBytes;

        private static IList<BaseWireframe> _wireframes;
        private static IList<BaseSolid> _solids;

        public static void RegisterSolid(BaseSolid solid)
        {
            _solids.Add(solid);
        }

        public static void DeregisterSolid(BaseSolid solid)
        {
            _solids.Remove(solid);
        }

        public static void RegisterWireframe(BaseWireframe wireframe)
        {
            _wireframes.Add(wireframe);
        }

        public static void DeregisterWireframe(BaseWireframe wireframe)
        {
            _wireframes.Remove(wireframe);
        }

        static Drawer()
        {
            _solids = new List<BaseSolid>();
            _wireframes = new List<BaseWireframe>();
        }


        public Drawer(MyGame myGame)
        {
            _myGame = myGame;
            _context = myGame.GraphicsDevice;
            _vertexSizeInBytes = Marshal.SizeOf(typeof(Vertex2DPositionColor));

            InitializeEffect();
            InitializeConstantBuffer();
        }

        private void InitializeEffect()
        {
            var vsBytecode = ShaderBytecode.CompileFromFile(string.Format(@"Content\{0}", FileName), "VS_Main", "vs_5_0");
            var psBytecode = ShaderBytecode.CompileFromFile(string.Format(@"Content\{0}", FileName), "PS_Main", "ps_5_0");

            _pixelShader = new PixelShader(_myGame.GraphicsDevice, psBytecode);
            _vertexShader = new VertexShader(_myGame.GraphicsDevice, vsBytecode);

            _layout = new InputLayout(_myGame.GraphicsDevice, ShaderSignature.GetInputSignature(vsBytecode), new[]
                    {
                        new InputElement("POSITION", 0, Format.R32G32_Float, 0, 0),
                        new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 8, 0)
                    });
        }

        public void InitializeConstantBuffer()
        {
            _constantBuffer = Buffer.Create(_myGame.GraphicsDevice, BindFlags.ConstantBuffer, ref _shaderParameters);
        }

        public void Draw(SharpDX.Toolkit.GameTime gameTime)
        {
            DrawWireframes();
            DrawSolids();
        }

        public void DrawWireframes()
        {
            if (!_wireframes.Any())
                return;

            var vertices = _wireframes.SelectMany(s => s.GetVertices()).ToArray();
            using (var buffer = Buffer.Create(_myGame.GraphicsDevice, BindFlags.VertexBuffer, vertices))
            {
                _context.InputAssembler.InputLayout = _layout;
                _context.InputAssembler.PrimitiveTopology = PrimitiveTopology.LineList;
                _context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(buffer, _vertexSizeInBytes, 0));
                _context.VertexShader.SetConstantBuffer(0, _constantBuffer);
                _context.VertexShader.Set(_vertexShader);
                _context.PixelShader.Set(_pixelShader);
                _context.UpdateSubresource(ref _shaderParameters, _constantBuffer);
                _context.Draw(vertices.Length, 0);
            }
        }

        public void DrawSolids()
        {
            if (!_solids.Any())
                return;

            var vertices = _solids.SelectMany(s => s.GetVertices()).ToArray();
            using (var buffer = Buffer.Create(_myGame.GraphicsDevice, BindFlags.VertexBuffer, vertices))
            {
                _myGame.GraphicsDevice.SetBlendState(_myGame.GraphicsDevice.BlendStates.NonPremultiplied);
                _myGame.GraphicsDevice.SetDepthStencilState(_myGame.GraphicsDevice.DepthStencilStates.DepthRead);
                var desc = _myGame.GraphicsDevice.RasterizerStates.Default.Description;
                desc.IsAntialiasedLineEnabled = true;
                desc.IsMultisampleEnabled = true;
                var q = SharpDX.Toolkit.Graphics.RasterizerState.New(_myGame.GraphicsDevice, desc);
                _myGame.GraphicsDevice.SetRasterizerState(q);

                _context.InputAssembler.InputLayout = _layout;
                _context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
                _context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(buffer, _vertexSizeInBytes, 0));
                _context.VertexShader.SetConstantBuffer(0, _constantBuffer);
                _context.VertexShader.Set(_vertexShader);
                _context.PixelShader.Set(_pixelShader);
                _context.UpdateSubresource(ref _shaderParameters, _constantBuffer);
                _context.Draw(vertices.Length, 0);

            }
        }

        public void Update(SharpDX.Toolkit.GameTime gameTime)
        {
            var data = Matrix4x4.Transpose(_myGame.Camera.WorldViewProjection).AsSharpDxMatrix();

            _shaderParameters = new ShaderContantBuffer
            {
                WorldProjectionView = data,
            };
        }

        private struct ShaderContantBuffer
        {
            public Matrix WorldProjectionView;  // 64 byte
        }

        //private struct LocalVerticesConstantBuffer
        //{
        //    public Vector2
        //    public Matrix WorldProjectionView;  // 64 byte
        //}
    }
}
