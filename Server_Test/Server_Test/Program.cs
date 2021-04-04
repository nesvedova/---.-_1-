using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using Server_Test;
using System.Linq;

namespace SocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new List<Film>();

            db = CreateDB();
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 54321);

            Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);

                while (true)
                {
                    Console.WriteLine("Wait connect with port .... {0}", ipEndPoint);

                    Socket handler = sListener.Accept();
                    string data = null;


                    byte[] bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);

                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);

                    Console.Write("Enter command: " + data + "\n\n");

                    var a = data.Split("\\");
                    string response = "";
                    try
                    {

                        if (data.Contains("scheduler\\film\\") && a.Length == 3)
                        {
                            var tempResponse = "";
                            var films = db.Where(x => x.Date == a.Last());

                            foreach (var film in films)
                            {
                                var filmInfo = $"{film.UUID_of_film}, {film.Title_of_film}, {film.Option}, {film.Time}, {film.Tikets.Where(x => !x.Reserved).Count()}; \n";
                                tempResponse = string.Join(tempResponse, filmInfo);
                            }

                            response = tempResponse;
                        }

                        if (data.Contains("scheduler\\film\\") && a.Length == 4)
                        {
                            var tempResponse = "";
                            var tikets = db.Where(x => x.UUID_of_film.ToString() == a.Last()).SelectMany(x => x.Tikets.Where(z => !z.Reserved)).ToList();

                            foreach (var tiket in tikets)
                            {
                                var filmInfo = $"{tiket.Ticket_ID}, {tiket.Row}, {tiket.Place}, {tiket.Price}; \n";
                                tempResponse = string.Join(tempResponse, filmInfo);
                            }

                            response = tempResponse;
                        }

                        if (data.Contains("reserve\\tiket\\") && a.Length == 4)
                        {
                            var tempResponse = "";
                            foreach (var film in db)
                            {
                                if (film.UUID_of_film.ToString() == a[2])
                                {
                                    foreach (var tiket in film.Tikets)
                                    {
                                        if (tiket.Ticket_ID.ToString() == a.Last())
                                        {
                                            tiket.Reserved = true;
                                            response = $"{film.Day}, {film.Name_of_film}, {film.Time}, {tiket.Row}, {tiket.Place}, {tiket.Price}; \n";
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        response = "Incorect request";
                    }
                    string reply = response;


                    byte[] msg = Encoding.UTF8.GetBytes(reply);
                    handler.Send(msg);

                    if (data.IndexOf("<TheEnd>") > -1)
                    {
                        Console.WriteLine("Disconnect with server.");
                        break;
                    }

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }

        public static List<Film> CreateDB()
        {
            return new List<Film>() {
            new Film()
            {
            UUID_of_film = 1,
            Name_of_film = "Flash",
            Day = "Monday",
            Date = "2021-03-10",
            Title_of_film = "Ассоциативная сеть (16+)",
            Option = "Cinetech+, 2D",
            Time = "10:15",
            Count_of_ticket = 2,
            Tikets = new List<Tiket>(){
                new Tiket() {
                Ticket_ID = 1,
                Row = 1,
                Place = 1,
                Price = 100
             }}
            }};
        }
    }
}