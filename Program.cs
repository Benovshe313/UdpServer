
using System.Net;
using System.Net.Sockets;
using System.Text;

var ip = IPAddress.Parse("192.168.100.115");
var port = 27001;
var endPoint = new IPEndPoint(ip, port);

var server = new UdpClient(endPoint);

Console.WriteLine("Server is running");

var remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

while (true)
{
    var fileNameBytes = server.Receive(ref remoteEndPoint);
    var fileLengthBytes = server.Receive(ref remoteEndPoint);
    var fileName = Encoding.UTF8.GetString(fileNameBytes);
    var fileLength = int.Parse(Encoding.UTF8.GetString(fileLengthBytes));

    var path = $"{Path.GetExtension(fileName)}";

    using var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);

    var recievedBytes = 0;
    while (true)
    {
        var fileBytes = server.Receive(ref remoteEndPoint);
        stream.Write(fileBytes);
        recievedBytes += fileBytes.Length;
        if(fileLength == recievedBytes)
        {
            break;
        }
    }
    Console.WriteLine("File downloaded");
}

