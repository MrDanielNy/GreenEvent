using System;
using System.Collections.Generic;
using System.Text;

namespace GreenEvent
{
    class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        private string _date;
        public string Date {
            get => _date;
            set
            {
                _date = value.Remove(11);
            }
        }
        private string _time;
        public string Time {
            get => _time;
            set
            {
                _time = value.Remove(6);
            } 
        }
        public int Price { get; set; }
        public string Location { get; set; }


        //public string FirstName
        //{
        //    get => "";
        //    set
        //    {
        //        if (value.Length <= 16 && value.Length >= 2)
        //        {
        //            name = value;
        //        }
        //    }
        //}


        static public void ShowEvent()
        {
            DataBase db = new DataBase();
            //Event myEvent = new Event();
            var myEvent = db.GetEventByEventId(3);

            Console.WriteLine(myEvent.Name);
            Console.WriteLine(myEvent.Description);
            Console.WriteLine(myEvent.Date);
            Console.WriteLine(myEvent.Time);
            Console.WriteLine(myEvent.Price);
            Console.WriteLine(myEvent.Location);
            Console.ReadLine();





        }

    }
}
