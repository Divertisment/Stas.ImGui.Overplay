using System;
using Vortice.Direct3D11;
using Vortice.DXGI;
using Vortice.Direct3D;
using OverlayImg;
namespace ClickableTransparentOverlay;


public partial class Overlay {
    // Принимает наш автономный Image<Rgba32>
    public unsafe IntPtr CreateImageTexture(Image<Rgba32> image, Format format) {
        var texDesc = new Texture2DDescription(format, (uint)image.Width, (uint)image.Height, 1, 1);

        fixed (byte* pPixels = image.Data) {
            var subResource = new SubresourceData(pPixels, texDesc.Width * 4);
            using var texture = device.CreateTexture2D(texDesc, new[] { subResource });

            var resViewDesc = new ShaderResourceViewDescription(
                texture,
                ShaderResourceViewDimension.Texture2D,
                format,
                0,
                texDesc.MipLevels
            );

            var srv = device.CreateShaderResourceView(texture, resViewDesc);

            // Прямой обход старого метода регистрации
            return srv.NativePointer;
        }
    }
}
