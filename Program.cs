using System.Net.WebSockets;
using System.Text;
using System.Diagnostics;

namespace AssemblyAIRealtime
{
    class Program
    {
        static async Task Main()
        {
            using var client = new AssemblyAIRealtimeClient();
            await client.StartTranscriptionAsync();
        }
    }

    class AssemblyAIRealtimeClient : IDisposable
    {
        private readonly ClientWebSocket _webSocket;
        private Process _soxProcess;
        private const string SoxCommand = "sox -d -t wav -r 16000 -b 16 -c 1 -e signed-integer -";

        public AssemblyAIRealtimeClient()
        {
            _webSocket = new ClientWebSocket();
        }

        public async Task StartTranscriptionAsync()
        {
            var apiKey = "API_KEY_HERE";

            _webSocket.Options.SetRequestHeader("Authorization", apiKey);
            await _webSocket.ConnectAsync(new Uri("wss://api.assemblyai.com/v2/realtime/ws?sample_rate=16000"), CancellationToken.None);
            _soxProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{SoxCommand}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            _soxProcess.Start();

            _soxProcess.BeginErrorReadLine();

            var soxOuputStream = _soxProcess.StandardOutput.BaseStream;
            await Task.WhenAll(SendAudioDataAsync(soxOuputStream), ReceiveMessagesAsync());
        }

        private async Task SendAudioDataAsync(Stream audioStream)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = await audioStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                await _webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, bytesRead), WebSocketMessageType.Binary, true, CancellationToken.None);
            }
        }

        private async Task ReceiveMessagesAsync()
        {
            var buffer = new byte[4096];
            WebSocketReceiveResult result;
            do
            {
                result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine("Received message: " + message);
            } while (!result.CloseStatus.HasValue);
        }

        public void Dispose()
        {
            _webSocket.Dispose();
            _soxProcess?.Kill();
            _soxProcess?.Dispose();
        }
    }
}
