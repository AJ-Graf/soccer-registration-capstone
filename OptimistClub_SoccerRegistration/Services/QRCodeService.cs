using QRCoder;

namespace OptimistClub_SoccerRegistration.Services
{
    public class QrCodeService
    {
        public byte[] GenerateQrCodePng(string url, int pixelsPerModule = 20)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrCodeData);
            return qrCode.GetGraphic(pixelsPerModule);
        }

        public string GenerateQrCodeBase64(string url, int pixelsPerModule = 20)
        {
            var pngBytes = GenerateQrCodePng(url, pixelsPerModule);
            return Convert.ToBase64String(pngBytes);
        }
    }
}