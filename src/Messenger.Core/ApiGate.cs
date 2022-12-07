using Messenger.Core.Entities;
using Messenger.Core.ExternalApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Messenger.Core
{
    public class ApiGate
    {
        private ILogHandler _logger;
        private ApiRequestsService _apiRequestsService;
        private string optionsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Options.json");

        public ApiGate()
        {
            _apiRequestsService = new ApiRequestsService();
            _logger = new LogHandlerConsole();
        }

        public async Task<ApiResponse> PostDataGetLiveness(List<byte[]> frames, Engine engine, byte[] videoBase64)
        {
            foreach (var frame in frames)
            {
                _logger.Log("Frame: " + frame);
            }
            _logger.Log("Video base64:\n" + videoBase64);

            ApiResponse resonse = new();

            switch (engine)
            {
                case Engine.Luna:
                    resonse = await PostDataGetLiveness(SettingsHelper.Get(optionsPath).LunaId, frames.First());
                    break;
                case Engine.Tevian:
                    resonse = await PostDataGetLiveness(SettingsHelper.Get(optionsPath).TevianId, frames.First());
                    break;
                case Engine.NTech:
                    resonse = await PostDataGetLiveness(SettingsHelper.Get(optionsPath).NTechId, videoBase64);
                    break;
            }
            return resonse is not null ? 
                               resonse :
                               new ApiResponse()
                               {
                                   ResponseStatus = ResponseStatus.BadRequest
                               };
        }

        private async Task<ApiResponse> PostDataGetLiveness(string engineId, byte[] data)
        {
            if (engineId == String.Empty)
                return new ApiResponse()
                {
                    ResponseStatus = ResponseStatus.EmptyApiGateString
                };

            _logger.Log($"Sending frames array to {engineId}");

            double liveness = 0;
            string matchId = String.Empty;
            string message = String.Empty;

            try
            {
                string fileHash = await _apiRequestsService.GetExternalApiFileHash(data);
                var livenessTaskId = await _apiRequestsService.GetExternalApiLivenessTaskId(fileHash, engineId);
                var bestMatchTaskId = await _apiRequestsService.GetExternalApiBestMatchTaskId(fileHash, engineId);
                var livenessTaskResult = await _apiRequestsService.GetTaskResult(livenessTaskId.TaskId);
                var bestMatchTaskResult = await _apiRequestsService.GetTaskResult(bestMatchTaskId.TaskId);

                if (livenessTaskResult is FaceApiTaskResult)
                {
                    var livenessResult = (FaceApiTaskResult)livenessTaskResult;
                    liveness = Convert.ToDouble(livenessResult.Result.FaceId);
                }
                else
                    message += "Liveness validation error. ";

                if (bestMatchTaskResult is FaceApiTaskResult)
                {
                    var matchResult = (FaceApiTaskResult)bestMatchTaskResult;
                    matchId = matchResult.Result.FaceId;
                }
                else
                    message += "Best match validation error. ";

                return new ApiResponse() 
                {
                    Liveness = liveness,
                    MatchedId = matchId,
                    Message = message == String.Empty ? "Ok" : message,
                    ResponseStatus = ResponseStatus.Ok
                };
            }
            catch (System.InvalidOperationException ex)
            {
                _logger.Log(ex.Message);
                return new ApiResponse() 
                {
                    ResponseStatus = ResponseStatus.BadRequest
                };
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message);
                return new ApiResponse()
                {
                    ResponseStatus = ResponseStatus.BadRequest
                };
            }
        }
    }
}
