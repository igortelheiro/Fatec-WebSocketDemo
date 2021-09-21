using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using WebSocketDemo.Common;

namespace WebSocketDemo.Client
{
    class Program
    {
        public static void Main(string[] args)
        {
            // Cria um buffer de 1024 bytes para comunicação com o servidor 
            var buffer = new byte[1024];

            try
            {
                // Estabelece o endpoint que será usado para o socket
                var ipHost = Dns.GetHostEntry(WebSocketConnectionConstants.ConnectionAddress);
                var ipAddress = ipHost.AddressList[0];
                var remoteEndpoint = new IPEndPoint(ipAddress, WebSocketConnectionConstants.ConnectionPort);

                // Cria um socket TCP/IP
                var sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    // Conecta o socket no endpoint estabelecido
                    sender.Connect(remoteEndpoint);

                    Logger.Log($"Socket connected to {sender.RemoteEndPoint.ToString()}");

                    // Codifica a mensagem em um array de bytes
                    var msg = Encoding.ASCII.GetBytes("This is a test");

                    // Envia a mensagem para o servidor
                    sender.Send(msg);

                    // Recebe a resposta do servidor
                    sender.Receive(buffer);
                    Logger.Log(Encoding.ASCII.GetString(buffer));

                    Logger.Log("Comunicação realizada com sucesso, desconectando o socket");
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
                #region ErrorHandling
                catch (ArgumentNullException ane)
                {
                    Logger.Log($"ArgumentNullException : {ane.ToString()}");
                }
                catch (SocketException se)
                {
                    Logger.Log($"SocketException : {se.ToString()}");
                }
                catch (Exception e)
                {
                    Logger.Log($"Unexpected exception : {e.ToString()}");
                }

            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
            }
            #endregion
        }
    }
}
