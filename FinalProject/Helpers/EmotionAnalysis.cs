using FinalProject.Entities;
using System.Text;
using System.Text.Json;

namespace FinalProject.Helpers
{
    public class EmotionAnalysis
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public EmotionAnalysis(string apiUrl = "http://127.0.0.1:5001")
        {
            _httpClient = new HttpClient();
            _apiUrl = apiUrl;
        }
        public async Task<serResponse> RecognizeEmotionAsync(string audioFilePath)
        {
            try
            {
                var formContent = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(File.ReadAllBytes(audioFilePath));
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("audio/wav");
                formContent.Add(fileContent, "file", Path.GetFileName(audioFilePath));

                var response = await _httpClient.PostAsync($"{_apiUrl}/predict", formContent);

                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(jsonResponse);
                var emotionResponse = new serResponse();
                emotionResponse.predictedEmotion = doc.RootElement.GetProperty("predicted_emotion").GetString();
                var confidenceScores = doc.RootElement.GetProperty("confidence_scores");
                emotionResponse.confidenceRate = confidenceScores.GetProperty(emotionResponse.predictedEmotion).GetDouble();
                return emotionResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error recognizing emotion: {ex.Message}");
                throw;
            }
        }
        public async Task<string> TranscribeAudioAsync(string audioFilePath)
        {
            try
            {
                var formContent = new MultipartFormDataContent();

                var fileContent = new ByteArrayContent(File.ReadAllBytes(audioFilePath));
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("audio/wav");
                formContent.Add(fileContent, "file", Path.GetFileName(audioFilePath));

                var response = await _httpClient.PostAsync($"{_apiUrl}/transcribe", formContent);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(jsonResponse);
                var transcription = doc.RootElement.GetProperty("transcription").GetString();

                return transcription;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error transcribing audio: {ex.Message}");
                throw;
            }
        }
        public async Task<terResponse> RecognizeTextEmotionAsync(string text)
        {
            try
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(new { text }), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_apiUrl}/text", jsonContent);

                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(jsonResponse);
                var emotionResponse=new terResponse();
                emotionResponse.predictedEmotion = doc.RootElement.GetProperty("predicted_class").GetInt32();
                emotionResponse.confidenceRate = doc.RootElement.GetProperty("confidence").GetDouble();
                return emotionResponse;
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
