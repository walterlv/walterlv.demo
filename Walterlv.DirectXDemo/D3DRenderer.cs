using System;
using System.Drawing;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using SharpDX.Windows;
using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;

namespace Walterlv.DirectXDemo
{
    class D3DRenderer : IDisposable
    {
        private const int Width = 1280;
        private const int Height = 720;

        private readonly RenderForm _renderForm;
        private SwapChain _swapChain;
        private Device _device;
        private DeviceContext _deviceContext;
        private RenderTargetView _renderTargetView;
        private Buffer _triangleVertexBuffer;
        private Vector3[] _vertexes;
        private VertexShader _vertexShader;
        private PixelShader _pixelShader;
        private ShaderSignature _inputSignature;
        private InputLayout _inputLayout;
        private Viewport _viewport;

        private readonly InputElement[] _inputElements =
        {
            new InputElement("POSITION", 0, Format.R32G32B32_Float, 0),
        };

        public D3DRenderer()
        {
            _renderForm = new RenderForm
            {
                ClientSize = new Size(Width, Height),
            };
        }

        public void Run()
        {
            RenderLoop.Run(_renderForm, Draw);
        }

        public void InitializeDeviceResources()
        {
            var swapChainDescription = new SwapChainDescription
            {
                ModeDescription = new ModeDescription(Width, Height, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                SampleDescription = new SampleDescription(1, 0),
                Usage = Usage.RenderTargetOutput,
                BufferCount = 1,
                OutputHandle = _renderForm.Handle,
                IsWindowed = true,
            };
            Device.CreateWithSwapChain(
                DriverType.Hardware, DeviceCreationFlags.None, swapChainDescription,
                out _device, out _swapChain);
            _deviceContext = _device.ImmediateContext;
            using (var backBuffer = _swapChain.GetBackBuffer<Texture2D>(0))
            {
                _renderTargetView = new RenderTargetView(_device, backBuffer);
            }

            // Vertex
            _vertexes = new[]
            {
                new Vector3(-0.5f, 0.5f, 0.0f),
                new Vector3(0.5f, 0.5f, 0.0f),
                new Vector3(-0.0f, -0.5f, 0.0f),
            };
            _triangleVertexBuffer = Buffer.Create(_device, BindFlags.VertexBuffer, _vertexes);

            // Shaders
            using (var vertexShaderBytecode = ShaderBytecode.CompileFromFile(
                "VertexShader.hlsl", "main", "vs_4_0", ShaderFlags.Debug))
            {
                _vertexShader = new VertexShader(_device, vertexShaderBytecode);
                _inputSignature = ShaderSignature.GetInputSignature(vertexShaderBytecode);
            }

            using (var pixelShaderBytecode = ShaderBytecode.CompileFromFile(
                "PixelShader.hlsl", "main", "ps_4_0", ShaderFlags.Debug))
            {
                _pixelShader = new PixelShader(_device, pixelShaderBytecode);
            }

            _deviceContext.VertexShader.Set(_vertexShader);
            _deviceContext.PixelShader.Set(_pixelShader);
            _inputLayout = new InputLayout(_device, _inputSignature, _inputElements);
            _deviceContext.InputAssembler.InputLayout = _inputLayout;
            _deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

            _viewport = new Viewport(0, 0, Width, Height);
            _deviceContext.Rasterizer.SetViewport(_viewport);
        }

        private void Draw()
        {
            _deviceContext.OutputMerger.SetRenderTargets(_renderTargetView);
            _deviceContext.ClearRenderTargetView(_renderTargetView, new RawColor4(1f, 1f, 1f, 1f));

            _deviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(_triangleVertexBuffer, Utilities.SizeOf<Vector3>(), 0));
            _deviceContext.Draw(_vertexes.Length, 0);

            _swapChain.Present(1, PresentFlags.None);
        }

        public void Dispose()
        {
            _renderForm?.Dispose();
            _swapChain?.Dispose();
            _device?.Dispose();
            _deviceContext?.Dispose();
            _renderTargetView?.Dispose();
            _triangleVertexBuffer?.Dispose();
            _vertexShader?.Dispose();
            _pixelShader?.Dispose();
            _inputSignature?.Dispose();
            _inputLayout?.Dispose();
        }
    }
}
