using Messenger.Core.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Messenger.Core.ExternalApi
{
    public class ApiRequestsService
    {
        private HttpClient _httplClient;
        private ILogHandler _logger;
        string optionsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Options.json");

        public ApiRequestsService()
        {
            _httplClient = new HttpClient();
            _logger = new LogHandlerConsole();
        }

        internal async Task<string> GetExternalApiFileHash(byte[] data)
        {
            var form = new MultipartFormDataContent();
            form.Add(new ByteArrayContent(data, 0, data.Length), "data", "image");
            var fileUploadResponse = await _httplClient.PostAsync(SettingsHelper.Get(optionsPath).UploadFileGate, form);
            return await fileUploadResponse.Content.ReadAsStringAsync();
        }

        internal async Task<FaceApiTaskResponse> GetExternalApiLivenessTaskId(string fileHash, string engineId)
        {
            HttpResponseMessage response = new();
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), SettingsHelper.Get(optionsPath).LivenessGate))
            {
                var livenessBodyRequest = new StringContent(JsonSerializer.Serialize(new FaceApiTaskRequest()
                {
                    EngineId = engineId,
                    FileHash = fileHash
                }));
                request.Content = livenessBodyRequest;
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                response = await _httplClient.SendAsync(request);
            }
            string content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<FaceApiTaskResponse>(content);
        }

        internal async Task<FaceApiTaskResponse> GetExternalApiBestMatchTaskId(string fileHash, string engineId)
        {
            HttpResponseMessage response = new();
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), SettingsHelper.Get(optionsPath).BestMatchGate))
            {
                var livenessBodyRequest = new StringContent(JsonSerializer.Serialize(new FaceApiTaskRequest()
                {
                    EngineId = engineId,
                    FileHash = fileHash
                }));
                request.Content = livenessBodyRequest;
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                response = await _httplClient.SendAsync(request);
            }
            string content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<FaceApiTaskResponse>(content);
        }

        internal async Task<IFaceApiTaskResult> GetTaskResult(string taskId)
        {
            HttpResponseMessage response = new();
            string url = $"{SettingsHelper.Get(optionsPath).TaskResultGate}?uuid={taskId}";
            using (var request = new HttpRequestMessage(new HttpMethod("GET"), url))
            {
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                response = await _httplClient.SendAsync(request);
            }
            string content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<FaceApiTaskResult>(content);
            else
                return JsonSerializer.Deserialize<FaceApiTaskResultError>(content);
        }

    }
}
