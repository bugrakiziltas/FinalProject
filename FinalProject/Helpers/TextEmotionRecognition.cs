using System.Text.Json;
using System.Text;

namespace FinalProject.Helpers
{
    public class TextEmotionRecognition
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public TextEmotionRecognition(string apiUrl = "http://127.0.0.1:5001")
        {
            _httpClient = new HttpClient();
            _apiUrl = apiUrl;
        }

        public async Task<int> RecognizeEmotionAsync(string text)
        {
            try
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(new { text }), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_apiUrl}/text", jsonContent);

                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(jsonResponse);
                var predictedClass = doc.RootElement.GetProperty("predicted_class").GetInt32();
                return predictedClass;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error recognizing text emotion: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> IsApiHealthyAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrl}/health");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
