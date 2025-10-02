using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using TCP_Server;

var ip = IPAddress.Parse("127.0.0.1");
var port = 27001;

var listener = new TcpListener(ip, port);
listener.Start();

using var db = new CarContext();

while (true)
{
    var client = listener.AcceptTcpClient();
    var stream = client.GetStream();
    var bw = new BinaryWriter(stream);
    var br = new BinaryReader(stream);

    while (true)
    {
        var input = br.ReadString();
        Console.WriteLine(input);
        var command = JsonSerializer.Deserialize<Command>(input);

        switch (command!.Method)
        {
            case Command.Get:
                bw.Write(GetMethod());
                break;
            case Command.Post:
                bw.Write(PostMethod(command.Car!));
                break;
            case Command.Put:
                bw.Write(PutMethod(command.Car!));
                break;
            case Command.Delete:
                bw.Write(DeleteMethod(command.Car!));
                break;
        }
    }
}

string GetMethod()
{
    var allCars = db.Cars.ToList();
    return JsonSerializer.Serialize(allCars);
}

string PostMethod(Car car)
{
    db.Cars.Add(car);
    db.SaveChanges();
    return "Car added";
}

string PutMethod(Car carUpd)
{
    var car = db.Cars.FirstOrDefault(c => c.Id == carUpd.Id);
    if (car == null) return "Car not found";

    car.Brand = carUpd.Brand;
    car.Model = carUpd.Model;
    car.Year = carUpd.Year;
    db.SaveChanges();
    return "Car updated";
}

string DeleteMethod(Car car)
{
    var carDel = db.Cars.FirstOrDefault(c => c.Id == car.Id);
    if (carDel == null) return "Car not found";

    db.Cars.Remove(carDel);
    db.SaveChanges();
    return "Car deleted";
}
