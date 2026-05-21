using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Google.GenAI;
using Google.GenAI.Types;

public class GeminiImageGenerator
{
    public static async Task Main(string[] args)
    {
        var apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
        if (string.IsNullOrEmpty(apiKey))
        {
            Console.WriteLine("Lỗi: Hãy thiết lập biến môi trường GEMINI_API_KEY.");
            return;
        }

        var client = new Client(apiKey);
        var model = "gemini-2.0-flash"; // ID model thực tế hiện tại

        var contents = new List<Content> {
            new Content {
                Role = "user",
                Parts = new List<Part> { new Part { Text = "Thiết kế một sơ đồ khối hệ thống cánh tay robot 4 bậc tự do." } }
            }
        };

        var config = new GenerateContentConfig {
            ResponseModalities = new List<string> { "IMAGE", "TEXT" }
        };

        using var imageStream = new MemoryStream();
        string mimeType = "image/png";

        try {
            await foreach (var chunk in client.Models.GenerateContentStreamAsync(model, contents, config))
            {
                var part = chunk.Candidates?[0].Content?.Parts?[0];
                if (part == null) continue;

                if (part.InlineData?.Data != null)
                {
                    imageStream.Write(part.InlineData.Data);
                    mimeType = part.InlineData.MimeType;
                }
                else if (!string.IsNullOrEmpty(chunk.Text()))
                {
                    Console.Write(chunk.Text());
                }
            }

            if (imageStream.Length > 0)
            {
                var ext = GetFileExtension(mimeType);
                var fullPath = $"output_robot_{DateTime.Now:yyyyMMdd_HHmmss}{ext}";
                File.WriteAllBytes(fullPath, imageStream.ToArray());
                Console.WriteLine($"\n[Thành công] Đã lưu ảnh vào: {fullPath}");
            }
        }
        catch (Exception ex) {
            Console.WriteLine($"\n[Lỗi]: {ex.Message}");
        }
    }

    static string GetFileExtension(string mimeType) => mimeType switch {
        "image/jpeg" => ".jpg",
        "image/png" => ".png",
        _ => ".bin"
    };
}
