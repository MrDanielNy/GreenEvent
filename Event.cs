using System;
using System.Collections.Generic;
using System.Text;

namespace GreenEvent
{
    class Event
    {
        public int Id { get; set; }
        public string Name { get; set; } = "namn på event";
        public string Description { get; set; } = "en liten beskrivning";
        
        //when getting data from database trim the string
        //for a nice output
        private string _date = "2019-01-01";
        public string Date {
            get => _date;
            set
            {
                if (value.Length > 10)
                {
                    _date = value.Remove(10);
                }
                else
                {
                    _date = value;
                }
            }
        }
        //when getting data from database trim the string..
        private string _time = "00:00";
        public string Time {
            get => _time;
            set
            {
                if (value.Length > 5)
                {
                    _time = value.Remove(5);
                }
                else
                {
                    _time = value;
                }
            } 
        }
        public int Price { get; set; }
        public string Location { get; set; } = "plats för event";

              
        /// <summary>
        /// this is only for testing
        /// </summary>
        static public void ShowEvent()
        {
            DataBase db = new DataBase();
            var myEvent = db.GetEventByEventId(3);

            Console.WriteLine(myEvent.Name);
            Console.WriteLine(myEvent.Description);
            Console.WriteLine(myEvent.Date);
            Console.WriteLine(myEvent.Time);
            Console.WriteLine(myEvent.Price);
            Console.WriteLine(myEvent.Location);
            Console.ReadLine();

        }

        static public void ShowAllEvents()
        {
            DataBase database = new DataBase();
            List<Event> allEvents = database.GetAllEvents();

            //check if there is any events to list
            if (allEvents.Count == 0)
            {
                Console.WriteLine("Det finns inga event");
                Console.ReadLine();
                return;
            }

            foreach (var myEvent in allEvents)
            {
                Console.WriteLine(myEvent.Name);
            }
            Console.ReadLine();

        }

        static public void CreateNewEvent()
        {
            Event newEvent = new Event(); //new event to set in database
            bool creating = true;


            var green = ConsoleColor.Green;

            var white = ConsoleColor.White;

                       
            
            


            while (creating)
            {
                Console.WriteLine("----Skapa ett nytt event----");
                Console.ForegroundColor = green;
                Console.Write("Namn:\t\t");
                Console.ForegroundColor = white;
                Console.WriteLine(newEvent.Name);

                Console.ForegroundColor = green;
                Console.Write("Beskrivning\t:");
                Console.ForegroundColor = white;
                Console.WriteLine(newEvent.Description);

                Console.ForegroundColor = green;
                Console.Write("Plats:\t\t");
                Console.ForegroundColor = white;
                Console.WriteLine(newEvent.Location);

                Console.ForegroundColor = green;
                Console.Write("Datum:\t\t");
                Console.ForegroundColor = white;
                Console.WriteLine(newEvent.Date);

                Console.ForegroundColor = green;
                Console.Write("Tid:\t\t");
                Console.ForegroundColor = white;
                Console.WriteLine(newEvent.Time);

                Console.ForegroundColor = green;
                Console.Write("Pris:\t\t");
                Console.ForegroundColor = white;
                Console.WriteLine(newEvent.Price);

                //Console.WriteLine($"Namn: {newEvent.Name}");
                //Console.WriteLine($"Beskrivning: {newEvent.Description}");
                //Console.WriteLine($"Plats: {newEvent.Location}");
                //Console.WriteLine($"Datum: {newEvent.Date}");
                //Console.WriteLine($"Tid: {newEvent.Time}");
                //Console.WriteLine($"Pris: {newEvent.Price}");
                Console.WriteLine();
                Console.WriteLine("Tryck enter för att fylla i eller esc för att avbryta");

                ConsoleKey consoleKey = Console.ReadKey().Key;
                if (consoleKey == ConsoleKey.Escape)
                {
                    return;
                }



            }
           
            
        }


    }
}
