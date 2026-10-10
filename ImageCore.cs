using System;
using System.IO;

namespace OverlayImg;

public struct Rgba32 { }

public class Image<TPixel> : IDisposable where TPixel : struct {
    public int Width { get; set; }
    public int Height { get; set; }
    public byte[]? Data { get; set; } = Array.Empty<byte>();

    public static Image<Rgba32> Load(string filePath) {
        using var stream = File.OpenRead(filePath);
        var result = StbImageSharp.ImageResult.FromStream(stream, StbImageSharp.ColorComponents.RedGreenBlueAlpha);
        return new Image<Rgba32> { Width = result.Width, Height = result.Height, Data = result.Data };
    }

    public void Dispose() {
        Data = null;
    }
}

public struct Rgb24 {
    public byte R;
    public byte G { get; set; }
    public byte B;
    public Rgb24(byte r, byte g, byte b) { R = r; G = g; B = b; }
}

public class RgbImage {
    public int Width { get; set; }
    public int Height { get; set; }
    public Rgb24[] Pixels { get; set; }

    public RgbImage(int width, int height) {
        Width = width;
        Height = height;
        Pixels = new Rgb24[width * height];
    }
}

public static class IMAGE {
    public static void WriteToPixels(this RgbImage image) {
        for (int y = 0; y < image.Height; y++) {
            int rowOffset = y * image.Width;
            for (int x = 0; x < image.Width; x++) {
                image.Pixels[rowOffset + x] = new Rgb24((byte)x, (byte)y, 0);
            }
        }
    }

    public static byte[] ImageToByteArray(byte[] rgbaData, int width, int height) {
        using (var ms = new MemoryStream()) {
            var writer = new StbImageWriteSharp.ImageWriter();
            writer.WritePng(rgbaData, width, height, StbImageWriteSharp.ColorComponents.RedGreenBlueAlpha, ms);
            return ms.ToArray();
        }
    }

    public static void Resize(string fname) {
        int targetWidth = 800;
        int targetHeight = 600;

        using var stream = File.OpenRead(fname);
        var original = StbImageSharp.ImageResult.FromStream(stream, StbImageSharp.ColorComponents.RedGreenBlueAlpha);

        byte[] resizedData = new byte[targetWidth * targetHeight * 4];

        unsafe {
            fixed (byte* pSrc = original.Data, pDst = resizedData) {
                float xRatio = (float)original.Width / targetWidth;
                float yRatio = (float)original.Height / targetHeight;

                for (int y = 0; y < targetHeight; y++) {
                    int srcY = (int)(y * yRatio);
                    int dstRowOffset = y * targetWidth * 4;
                    int srcRowOffset = srcY * original.Width * 4;

                    for (int x = 0; x < targetWidth; x++) {
                        int srcX = (int)(x * xRatio);

                        int dstPixel = dstRowOffset + (x * 4);
                        int srcPixel = srcRowOffset + (srcX * 4);

                        pDst[dstPixel] = pSrc[srcPixel];
                        pDst[dstPixel + 1] = pSrc[srcPixel + 1];
                        pDst[dstPixel + 2] = pSrc[srcPixel + 2];
                        pDst[dstPixel + 3] = pSrc[srcPixel + 3];
                    }
                }
            }
        }

        using (var outStream = File.Create("modified_image_path.png")) {
            var writer = new StbImageWriteSharp.ImageWriter();
            writer.WritePng(resizedData, targetWidth, targetHeight, StbImageWriteSharp.ColorComponents.RedGreenBlueAlpha, outStream);
        }
    }
}
