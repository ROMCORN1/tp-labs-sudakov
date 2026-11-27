using System.Net;
using System.Net.Sockets;
using System.Text;

string host = "127.0.0.1";
int port = 5555;

Console.Write("Попытаться найти сервер в сети автоматически? (y/n): ");
if (Console.ReadLine()?.ToLower() == "y")
{
    using var udp = new UdpClient();
    udp.EnableBroadcast = true;
    byte[] request = Encoding.UTF8.GetBytes("DISCOVER_CHAT_SERVER");
    udp.Send(request, request.Length, new IPEndPoint(IPAddress.Broadcast, 5556));

    udp.Client.ReceiveTimeout = 1000;
    try
    {
        IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
        byte[] response = udp.Receive(ref ep);
        host = ep.Address.ToString();
        port = int.Parse(Encoding.UTF8.GetString(response));
        Console.WriteLine($"Сервер найден: {host}:{port}");
    }
    catch
    {
        Console.WriteLine("Сервер не найден, используем localhost.");
    }
}
else
{
    Console.Write("Адрес сервера (Enter = localhost): ");
    string? inputHost = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(inputHost)) host = inputHost;
}

using var client = new TcpClient();
try
{
    await client.ConnectAsync(host, port);
}
catch (Exception ex)
{
    Console.WriteLine($"Не удалось подключиться к серверу {host}:{port}. Ошибка: {ex.Message}");
    return;
}

var stream = client.GetStream();
var reader = new StreamReader(stream);
var writer = new StreamWriter(stream) { AutoFlush = true };

while (true)
{
    Console.Write("Ваш ник: ");
    string? nick = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(nick)) continue;

    await writer.WriteLineAsync(nick);
    string? response = await reader.ReadLineAsync();

    if (response == "OK") break;
    else Console.WriteLine("Этот ник уже занят. Попробуйте другой.");
}

_ = Task.Run(async () =>
{
    try
    {
        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
            Console.WriteLine(line);
    }
    catch (IOException) { }
    Console.WriteLine("Соединение с сервером потеряно.");
});

Console.WriteLine("Подключено. /exit — выход, /list — список, /w ник текст — ЛС, /join комната — переход в комнату.");
while (true)
{
    string? msg = Console.ReadLine();
    if (msg == null) continue;
    await writer.WriteLineAsync(msg);
    if (msg == "/exit") break;
}