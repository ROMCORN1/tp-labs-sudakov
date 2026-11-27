using System.Net;
using System.Net.Sockets;
using System.Text;

int port = 5555;
if (args.Length > 0 && int.TryParse(args[0], out int p)) port = p;

var clients = new Dictionary<string, ClientState>();
var lockObj = new object();
var history = new Queue<string>();

var listener = new TcpListener(IPAddress.Any, port);
listener.Start();
Log($"Сервер запущен на порту {port}. Ctrl+C — остановка.");

_ = Task.Run(async () =>
{
    using var udp = new UdpClient(5556);
    while (true)
    {
        try
        {
            var result = await udp.ReceiveAsync();
            if (Encoding.UTF8.GetString(result.Buffer) == "DISCOVER_CHAT_SERVER")
            {
                byte[] response = Encoding.UTF8.GetBytes(port.ToString());
                await udp.SendAsync(response, response.Length, result.RemoteEndPoint);
            }
        }
        catch { /* Игнорируем ошибки UDP */ }
    }
});

while (true)
{
    TcpClient client = await listener.AcceptTcpClientAsync();
    _ = HandleClientAsync(client);
}

async Task HandleClientAsync(TcpClient client)
{
    var endpoint = client.Client.RemoteEndPoint;
    var stream = client.GetStream();
    var reader = new StreamReader(stream);
    var writer = new StreamWriter(stream) { AutoFlush = true };

    string? nick = await reader.ReadLineAsync();
    if (string.IsNullOrWhiteSpace(nick)) { client.Close(); return; }

    lock (lockObj)
    {
        if (clients.ContainsKey(nick))
        {
            writer.WriteLine("ERROR: Ник занят");
            client.Close();
            return;
        }
        writer.WriteLine("OK");
        clients.Add(nick, new ClientState(nick, writer, "General"));
    }

    Log($"{nick} подключился ({endpoint})");
    await BroadcastAsync($"*** {nick} вошёл в чат ***", "General");

    List<string> historySnapshot;
    lock (lockObj)
    {
        historySnapshot = history.ToList();
    }
    foreach (var msg in historySnapshot)
    {
        await writer.WriteLineAsync(msg);
    }

    try
    {
        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            if (line == "/exit") break;

            if (line == "/list")
            {
                string users;
                lock (lockObj)
                {
                    users = string.Join(", ", clients.Keys);
                }
                await writer.WriteLineAsync($"*** Пользователи онлайн: {users} ***");
            }
            else if (line.StartsWith("/w "))
            {
                var parts = line.Split(' ', 3);
                if (parts.Length >= 3)
                {
                    string targetNick = parts[1];
                    string privateMsg = parts[2];
                    await SendPrivateMessageAsync(nick, targetNick, privateMsg);
                }
                else await writer.WriteLineAsync("*** Ошибка: формат /w ник текст ***");
            }
            else if (line.StartsWith("/join "))
            {
                string newRoom = line.Substring(6).Trim();
                string? oldRoom = null;
                bool roomChanged = false;

                lock (lockObj)
                {
                    if (clients.ContainsKey(nick))
                    {
                        oldRoom = clients[nick].Room;
                        clients[nick] = clients[nick] with { Room = newRoom };
                        roomChanged = true;
                    }
                }

                if (roomChanged)
                {
                    await BroadcastAsync($"*** {nick} перешёл в комнату {newRoom} ***", oldRoom!);
                    await BroadcastAsync($"*** {nick} присоединился к комнате ***", newRoom);
                }
            }
            else
            {
                string room;
                lock (lockObj) room = clients[nick].Room;
                string formattedMsg = $"[{DateTime.Now:HH:mm:ss}] {nick}: {line}";

                lock (lockObj)
                {
                    history.Enqueue(formattedMsg);
                    if (history.Count > 10) history.Dequeue();
                }
                Log($"[{room}] {nick}: {line}");
                await BroadcastAsync(formattedMsg, room);
            }
        }
    }
    catch (IOException) { }
    finally
    {
        string room = "General";
        lock (lockObj)
        {
            if (clients.ContainsKey(nick))
            {
                room = clients[nick].Room;
                clients.Remove(nick);
            }
        }
        client.Close();
        Log($"{nick} отключился");
        await BroadcastAsync($"*** {nick} покинул чат ***", room);
    }
}

async Task SendPrivateMessageAsync(string sender, string target, string message)
{
    StreamWriter? targetWriter = null;
    StreamWriter? senderWriter = null;

    lock (lockObj)
    {
        if (clients.ContainsKey(target)) targetWriter = clients[target].Writer;
        if (clients.ContainsKey(sender)) senderWriter = clients[sender].Writer;
    }

    if (targetWriter != null)
    {
        await targetWriter.WriteLineAsync($"[{DateTime.Now:HH:mm:ss}] (ЛС) {sender}: {message}");
        if (senderWriter != null)
            await senderWriter.WriteLineAsync($"*** Отправлено {target}: {message} ***");
        Log($"ЛС от {sender} к {target}: {message}");
    }
    else
    {
        if (senderWriter != null)
            await senderWriter.WriteLineAsync($"*** Ошибка: пользователь {target} не найден ***");
    }
}

async Task BroadcastAsync(string message, string room)
{
    List<StreamWriter> snapshot = new();
    lock (lockObj)
    {
        foreach (var c in clients.Values)
            if (c.Room == room) snapshot.Add(c.Writer);
    }

    foreach (var w in snapshot)
    {
        try { await w.WriteLineAsync(message); }
        catch (IOException) { }
    }
}

void Log(string message)
{
    string logLine = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
    Console.WriteLine(logLine);
    lock (lockObj)
    {
        File.AppendAllText("chat.log", logLine + Environment.NewLine);
    }
}

record ClientState(string Nick, StreamWriter Writer, string Room);