using Tesseract;
using System.Text.RegularExpressions;

namespace PantryDjango.Services
{
    public class OcrService
    {
        private readonly string tessDataPath = @"C:\Program Files\Tesseract-OCR\tessdata"; // 경로 확인!

        public string ExtractExpiryDate(string imagePath)
        {
            using var engine = new TesseractEngine(tessDataPath, "eng+kor", EngineMode.Default);
            using var img = Pix.LoadFromFile(imagePath);
            using var page = engine.Process(img);

            string text = page.GetText();

            // 날짜 패턴 찾기
            var match = Regex.Match(text, @"\d{4}[-./]\d{2}[-./]\d{2}");
            return match.Success ? match.Value : "날짜 인식 실패";
        }
    }
}
