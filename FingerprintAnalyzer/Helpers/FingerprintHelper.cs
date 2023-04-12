using PatternRecognition.FingerprintRecognition.Core;
using PatternRecognition.FingerprintRecognition.FeatureExtractors;
using SixLabors.ImageSharp.Formats.Bmp;
using System.Drawing;

namespace FingerprintAnalyzer.Helpers
{
    public static class FingerprintHelper
    {
        public static byte[] GetBWImage(byte[] imageData)
        {
            using var imageUploaded = SixLabors.ImageSharp.Image.Load(imageData);
            imageUploaded.Mutate(x => x.BlackWhite());

            using (MemoryStream ms = new MemoryStream())
            {
                imageUploaded.Save(ms, new BmpEncoder());
                return ms.ToArray();
            }
        }

        public static byte[] GetSkeletonImage(byte[] imageData)
        {
            Bitmap bitmap;
            using (var ms = new MemoryStream(imageData))
            {
                ms.Seek(0, SeekOrigin.Begin);
                bitmap = new Bitmap(ms);
            }

            var extractor = new Ratha1995SkeImgExtractor();
            SkeletonImage skeletonImage = extractor.ExtractFeatures(bitmap);

            Bitmap imgBitmap = skeletonImage.ConvertToBitmap();
            using (var ms = new MemoryStream())
            {
                imgBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Seek(0, SeekOrigin.Begin);
                return ms.ToArray();
            }
        }
    }
}
