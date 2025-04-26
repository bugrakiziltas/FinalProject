using FinalProject.Entities;
using System.Text.Json;

namespace FinalProject.Helpers
{
    public class SpeechEmotionRecognition
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public SpeechEmotionRecognition(string apiUrl = "http://127.0.0.1:5001")
        {
            _httpClient = new HttpClient();
            _apiUrl = apiUrl;
        }

        public async Task<string> RecognizeEmotionAsync(string audioFilePath)
        {
            try
            {
                // Create multipart form content
                var formContent = new MultipartFormDataContent();

                // Add the audio file
                var fileContent = new ByteArrayContent(File.ReadAllBytes(audioFilePath));
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("audio/wav");
                formContent.Add(fileContent, "file", Path.GetFileName(audioFilePath));
                Console.WriteLine(formContent);
                // Send request to the API
                var response = await _httpClient.PostAsync($"{_apiUrl}/predict", formContent);

                // Ensure successful response
                response.EnsureSuccessStatusCode();

                // Parse the JSON response
                var jsonResponse = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(jsonResponse);
                var predictedEmotion = doc.RootElement.GetProperty("predicted_emotion").GetString();

                return predictedEmotion;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error recognizing emotion: {ex.Message}");
                throw;
            }
        }

        // Health check method to verify the API is running
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
