using Tesseract;
using System.Text.RegularExpressions;

namespace PantryDjango.Services
{
    public class OcrService
    {
        private readonly string _tessDataPath;

        public OcrService(string tessDataPath)
        {
            _tessDataPath = tessDataPath;
        }

        public string ExtractExpiryDateFromStream(Stream imageStream, string lang = "eng+kor")
        {
            using var engine = new TesseractEngine(_tessDataPath, lang, EngineMode.Default);
            using var img = Pix.LoadFromMemory(ReadFully(imageStream));
            using var page = engine.Process(img);

            var text = page.GetText();
            var match = Regex.Match(text, @"\d{4}[-./]\d{2}[-./]\d{2}");
            return match.Success ? match.Value : "날짜 인식 실패";
        }

        private byte[] ReadFully(Stream input)
        {
            using var ms = new MemoryStream();
            input.CopyTo(ms);
            return ms.ToArray();
        }
    }
}
