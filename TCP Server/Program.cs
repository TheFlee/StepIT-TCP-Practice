using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using TCP_Server;

var ip = IPAddress.Parse("127.0.0.1");
var port = 27001;

var listener = new TcpListener(ip, port);
listener.Start();

while (true)
{
    var client = listener.AcceptTcpClient();
    var stream = client.GetStream();
    var bw = new BinaryWriter(stream);
    var br = new BinaryReader(stream);

    using var db = new CarContext();

    while (true)
    {
        var input = br.ReadString();
        Console.WriteLine(input);
        var command = JsonSerializer.Deserialize<Command>(input);

        switch (command!.Text)
        {
            case Command.Get:
                var allCars = db.Cars.ToList();
                bw.Write(JsonSerializer.Serialize(allCars));
                break;
            case Command.Post:
                var newCar = JsonSerializer.Deserialize<Car>(command.Param!);
                db.Cars.Add(newCar!);
                db.SaveChanges();
                bw.Write("Car added");
                break;
            case Command.Put:
                newCar = JsonSerializer.Deserialize<Car>(command.Param!);
                var car = db.Cars.FirstOrDefault(c => c.Id == newCar!.Id);
                if (car != null)
                {
                    car.Brand = newCar.Brand;
                    car.Model = newCar.Model;
                    car.Year = newCar.Year;
                    db.SaveChanges();
                    bw.Write("Car updated");
                }
                else
                {
                    bw.Write("Car not found");
                }
                break;
            case Command.Delete:
                if (int.TryParse(command.Param, out int id))
                {
                    car = db.Cars.FirstOrDefault(c => c.Id == id);
                    if (car != null)
                    {
                        db.Cars.Remove(car);
                        db.SaveChanges();
                        bw.Write("Car deleted");
                    }
                    else
                    {
                        bw.Write("Car not found");
                    }
                }
                break;
        }
    }
}
