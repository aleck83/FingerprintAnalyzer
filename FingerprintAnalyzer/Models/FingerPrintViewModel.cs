using System.ComponentModel.DataAnnotations;

namespace FingerprintAnalyzer.Models
{
    public class FingerPrintViewModel
    {
        [DataType(DataType.Upload)]
        [Display(Name = "Upload File")]
        [Required(ErrorMessage = "Please choose file to upload.")]
        public string File { get; set; }

        public byte[] OriginImageData { get; set; }
        public byte[] BlackedImageData { get; set; }
        public byte[] SkeletonImageData { get; set; }
    }
}
