using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // записываем ip адресc и порт
            var ipAddress = IPAddress.Parse("127.0.0.1");
            var port = 62227;

            // создаем TCP Listener
            var listener = new TcpListener(ipAddress, port);

            try
            {
                // начинаем слушать входящее соединение
                listener.Start();

                while (true)
                {
                    var incomingConnection = await listener.AcceptTcpClientAsync();

                    _ = Task.Run(() =>
                    {
                        var networkStream = incomingConnection.GetStream();

                        //var buffer = new byte[512];
                        //var text = string.Empty;
                        var stringBuilder = new StringBuilder();

                        do
                        {
                            var buffer = new byte[512];

                            networkStream.Read(buffer, 0, buffer.Length);
                            //text += Encoding.UTF8.GetString(buffer);
                            stringBuilder.Append(Encoding.UTF8.GetString(buffer));
                        } while (networkStream.DataAvailable);

                        Console.WriteLine($"Сообщение - {stringBuilder.ToString()}");

                        networkStream.Close();
                        incomingConnection.Close();

                    });
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            Console.ReadLine();
        }
    }
}
