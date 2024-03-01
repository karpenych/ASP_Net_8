using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography;

namespace WebSocketProj
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var _server = new WebSocServ();
        }
    }

    class WebSocServ
    {
        static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        static private string guid = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";

        public WebSocServ()
        {
            IPEndPoint localPoint = new(IPAddress.Any, 8080);
            serverSocket.Bind(localPoint);
            
            serverSocket.Listen(128);
            serverSocket.BeginAccept(OnAccept, null);

            Console.WriteLine("Ожидаю клиента . .");
            Console.Read(); 
        }

        private static void OnAccept(IAsyncResult result)
        {
            byte[] buffer = new byte[1024];
            try
            {
                Socket? client_soc = null;
                string headerRequest = "";

                if (serverSocket != null && serverSocket.IsBound)
                {
                    client_soc = serverSocket.EndAccept(result);
                    var i = client_soc.Receive(buffer);
                    headerRequest = (System.Text.Encoding.UTF8.GetString(buffer)).Substring(0, i);
                    Console.WriteLine("Заголовки от клиента \n" + headerRequest);
                }
                if (client_soc != null)
                { 
                    var key = headerRequest.Replace("ey:", "`").Split('`')[1].Replace("\r", "").Split('\n')[0].Trim();
                    
                    var newLine = "\r\n";
                    var response = "HTTP/1.1 101 Switching Protocols" + newLine
                    + "Upgrade: websocket" + newLine
                    + "Connection: Upgrade" + newLine
                    + "Sec-WebSocket-Accept: " + AcceptKey(key) + newLine
                    +"Sec-WebSocket-Version: 13" + newLine
                    + newLine;

                    client_soc.Send(System.Text.Encoding.UTF8.GetBytes(response));
                    var i = client_soc.Receive(buffer);
                    var Text64 = Convert.ToBase64String(buffer).Substring(0, i);
                    Console.WriteLine("buf = " + Text64);                    
                    byte[] buf1 = { 0x81, 0x05, 0x48, 0x65, 0x6c, 0x6c, 0x6f };                    
                    byte[] buf10 = { 0x01, 0x05, 0x48, 0x65, 0x6c, 0x6c, 0x6f }; //Hello
                    byte[] buf11 = { 0x00, 0x01, 0x20 }; //пробел
                    byte[] buf12 = { 0x80, 0x07, 0x57, 0x6f, 0x72, 0x6c, 0x64, 0x20, 0x21 }; // World !
                    client_soc.Send(buf1);
                    client_soc.Send(buf10);
                    client_soc.Send(buf11);
                    client_soc.Send(buf12);
                    System.Threading.Thread.Sleep(2000);
                    byte[] finish = { 0x88, 0x00 };
                    client_soc.Send(finish);
                    Console.WriteLine("\n\nНажать любую клавишу для выхода из приложения");
                    Console.Read();
                }
            }
            catch (SocketException exception)
            {
                throw exception;
            }
        }

        private static string AcceptKey(string key)
        {
            string longKey = key + guid;
         
            byte[] hashBytes = ComputeHash(longKey); // возвращает массив байтов
            return Convert.ToBase64String(hashBytes); // преобразует массив байтов
                                                      // в строку в формате Base64
        }

        static SHA1 sha1 = SHA1CryptoServiceProvider.Create();

        private static byte[] ComputeHash(string str)
        {
            return sha1.ComputeHash(System.Text.Encoding.ASCII.GetBytes(str));
        }
    }
}
