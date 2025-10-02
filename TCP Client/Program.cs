using Microsoft.VisualBasic;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

var ip = IPAddress.Parse("127.0.0.1");
var port = 27001;
var client = new TcpClient();
client.Connect(ip, port);

var stream = client.GetStream();
var bw = new BinaryWriter(stream);
var br = new BinaryReader(stream);

Command command = null!;
string response = null!;
string str = null!;

while (true)
{
    Console.Clear();
    Console.WriteLine("Write command name or HELP:");
    str = Console.ReadLine()!.ToUpper();
    if (str == "HELP")
    {
        Console.WriteLine();
        Console.WriteLine("Command list:");
        Console.WriteLine(Command.Get);
        Console.WriteLine($"{Command.Post} <car_brand> <car_model> <car_year>");
        Console.WriteLine($"{Command.Put} <car_id> <car_brand> <car_model> <car_year>");
        Console.WriteLine($"{Command.Delete} <car_id>");
        Console.WriteLine("Press any key to continue!");
        Console.ReadLine();
        Console.Clear();
    }
    var input = str.Split(' ');
    switch (input[0])
    {
        case Command.Get:
            Console.Clear();
            command = new Command { Text = Command.Get };
            bw.Write(JsonSerializer.Serialize(command));
            response = br.ReadString();
            var cars = JsonSerializer.Deserialize<List<Car>>(response);
            cars!.ForEach(c => Console.WriteLine($"Id={c.Id}, Brand={c.Brand}, Model={c.Model}, Year={c.Year}"));
            Console.WriteLine("Press any key to continue!");
            Console.ReadLine();
            break;
        case Command.Post:
            Console.Clear();
            var newCar = new Car { Brand = input[1], Model = input[2], Year = int.Parse(input[3]) };
            command = new Command { Text = Command.Post, Param = JsonSerializer.Serialize(newCar) };
            bw.Write(JsonSerializer.Serialize(command));
            response = br.ReadString();
            Console.WriteLine(response);
            Console.WriteLine("Press any key to continue!");
            Console.ReadLine();
            break;
        case Command.Put:
            Console.Clear();
            var updateCar = new Car { Id = int.Parse(input[1]), Brand = input[2], Model = input[3], Year = int.Parse(input[4]) };
            command = new Command { Text = Command.Put, Param = JsonSerializer.Serialize(updateCar) };
            bw.Write(JsonSerializer.Serialize(command));
            response = br.ReadString();
            Console.WriteLine(response);
            Console.WriteLine("Press any key to continue!");
            Console.ReadLine();
            break;
        case Command.Delete:
            Console.Clear();
            command = new Command { Text = Command.Delete, Param = input[1] };
            bw.Write(JsonSerializer.Serialize(command));
            response = br.ReadString();
            Console.WriteLine(response);
            Console.WriteLine("Press any key to continue!");
            Console.ReadLine();
            break;
    }

}
