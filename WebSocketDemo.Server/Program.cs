using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using WebSocketDemo.Common;

namespace WebSocketDemo.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var address = IPAddress.Parse(WebSocketConnectionConstants.ConnectionAddress);
            var port = WebSocketConnectionConstants.ConnectionPort;
            var server = new TcpListener(address, port);

            server.Start();
            Logger.Log($"Server has started on {address}:{port}.");
            Logger.Log("Waiting for a connection...");

            // A aplicação prende a thread para escutar qualquer tentativa de conexão pendente
            var client = server.AcceptTcpClient();

            Logger.Log("A client connected.");

            // Quando um cliente se conecta, usamos o canal de comunicação via stream para receber e enviar mensagens
            var stream = client.GetStream();

            // Com o canal de conexão estabelecido, o server pode ficar em loop escutando os dados que serão enviados e respondendo propriamente
            while (true)
            {
                // Até que haja dados para serem consumidos, a aplicação prende a thread
                while (!stream.DataAvailable);

                // Quando o canal identifica que existem dados para serem consumidos a thread é liberada
                // Criamos um buffer para armazenar os dados recebidos via stream
                var buffer = new Byte[client.Available];

                stream.Read(buffer, 0, buffer.Length);

                // Traduz o array de bytesdo buffer para uma string legível em UTF-8
                var data = Encoding.UTF8.GetString(buffer);
                Logger.Log($"Mensagem Recebida: {data}");

                // Enviamos uma resposta de volta
                var response = Encoding.ASCII.GetBytes("Bem vindo ao servidor");
                stream.Write(response, 0, response.Length);
            }
        }
    }
}
