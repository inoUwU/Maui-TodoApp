using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace Maui_TodoApp.WsServer
{
    internal class WsServer
    {
        private Thread? serverThread; // サーバースレッド
        private HttpListener? httpListener; // httpリスナー
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private static WsServer? _Instance;
        public string? Port { get; set; }

        // ポート番号が割り当てられたときに発火するイベント
        public event Action<int>? OnPortAssigned;

        private static int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Any, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }

        private WsServer() { }

        public static WsServer GetInstance()
        {
            if (WsServer._Instance == null)
            {
                _Instance = new WsServer();
            }
            return _Instance;
        }

        public void Start()
        {
            serverThread = new Thread(new ThreadStart(StartServer))
            {
                // バックグランウドスレッドに設定
                IsBackground = true
            };
            serverThread.Start();
        }

        public void Stop()
        {
            if (httpListener is not null) httpListener?.Stop();
        }

        public async void Dispose()
        {
            _cancellationTokenSource.Cancel();
            await Task.Delay(3000);
            if (httpListener is not null && !httpListener.IsListening)
            {
                httpListener?.Close();
            }
        }

        private async void StartServer()
        {
            httpListener = new HttpListener();
            var port = GetRandomUnusedPort();
            httpListener.Prefixes.Add("http://localhost:" + port + "/");
            httpListener.Start();
            Debug.WriteLine("Listening on" + port);
            this.Port = port.ToString();
            OnPortAssigned?.Invoke(port);

            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    var context = await httpListener.GetContextAsync();

                    if (context.Request.IsWebSocketRequest)
                    {
                        await ProcessWebSocketRequestAsync(context);
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                        context.Response.Close();
                    }
                }
                catch (HttpListenerException)
                {
                    // HttpListener stopped
                }
            }

            httpListener.Stop();
            Console.WriteLine("WebSocket server stopped.");
        }

        private async Task ProcessWebSocketRequestAsync(HttpListenerContext context)
        {
            var webSocketContext = await context.AcceptWebSocketAsync(subProtocol: null);

            Debug.WriteLine("WebSocket connection established.");

            var webSocket = webSocketContext.WebSocket;

            ThreadPool.QueueUserWorkItem(async state =>
            {
                var buffer = new byte[1024];
                while (webSocket.State == WebSocketState.Open)
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationTokenSource.Token);
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        Debug.WriteLine($"Received message: {message}");
                        var responseMessage = $"You sent '{message}' at {DateTime.Now.ToLongTimeString()}";
                        var responseBytes = Encoding.UTF8.GetBytes(responseMessage);
                        await webSocket.SendAsync(new ArraySegment<byte>(responseBytes), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                    }
                }
                Debug.WriteLine("WebSocket connection closed.");
            });
        }
    }
}
