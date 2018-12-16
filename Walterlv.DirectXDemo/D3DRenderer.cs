using System;
using System.Drawing;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using SharpDX.Windows;
using Device = SharpDX.Direct3D11.Device;

namespace Walterlv.DirectXDemo
{
    class D3DRenderer : IDisposable
    {
        private readonly RenderForm _renderForm;
        private SwapChain _swapChain;
        private Device _device;
        private DeviceContext _deviceContext;
        private RenderTargetView _renderTargetView;

        public D3DRenderer()
        {
            _renderForm = new RenderForm
            {
                ClientSize = new Size(1280, 720),
            };
        }

        public void Run()
        {
            RenderLoop.Run(_renderForm, OnRender);
        }

        public void InitializeDeviceResources()
        {
            var swapChainDescription = new SwapChainDescription
            {
                ModeDescription = new ModeDescription(1280, 720, new Rational(60, 1), Format.R8G8B8A8_UNorm),
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
        }

        private void OnRender()
        {
            _deviceContext.OutputMerger.SetRenderTargets(_renderTargetView);
            _deviceContext.ClearRenderTargetView(_renderTargetView,
                new RawColor4(0x18 / 255f, 0xa1 / 255f, 0x5f / 255f, 0xff / 255f));
            _swapChain.Present(1, PresentFlags.None);
        }

        public void Dispose()
        {
            _renderTargetView?.Dispose();
            _swapChain?.Dispose();
            _device?.Dispose();
            _deviceContext?.Dispose();
            _renderForm?.Dispose();
        }
    }
}
