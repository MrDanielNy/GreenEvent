using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GreenEvent
{
    class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
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
        public string Location { get; set; } = "plats där eventet ska hållas";
        public int LocationId { get; set; }


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
            var allEvents = database.GetAllEvents(); //create list of events..

            //check if there is any events to list
            if (allEvents.Count == 0)
            {
                Console.WriteLine("Det finns inga event...");
                Console.ReadLine();
                return;
            }

            foreach (var myEvent in allEvents)
            {
                Console.WriteLine(myEvent.Name);
            }
            Console.ReadLine();

        }

        public void CreateNewEvent(int createTurn)
        {
            DataBase database = new DataBase();
            var locationNames = database.GetAllLocations(); //create list of locations
            if (locationNames.Count == 0)  //if no locations return to menu
            {
                Console.WriteLine("Du måste skapa en plats innan du kan skapa ett event..");
                Console.ReadLine();
                return;
            }

            bool isCreating = true; //while event is being created
            bool isEditing = true; //if event is being edited


            if (createTurn == 0)
            {
                createTurn = 1; //witch field user is going to fill in
                isEditing = false; //creating a new Event
            }



            bool isDateCorrect = true;
            bool isTimeCorrect = true;
            bool isPriceCorrect = true;

            while (isCreating)
            {

                Console.Clear();

                ConsoleColor green = ConsoleColor.Green;
                ConsoleColor white = ConsoleColor.White;
                ConsoleColor red = ConsoleColor.Red;

                if (!isEditing)
                {
                    Console.WriteLine("----<<<Skapa ett nytt event>>>----");
                }
                else
                {
                    Console.WriteLine("----<<<Redigera ett event>>>----");
                }
                
                Console.ForegroundColor = green;
                Console.Write("Namn:\t\t");
                Console.ForegroundColor = white;
                Console.WriteLine(this.Name);

                Console.ForegroundColor = green;
                Console.Write("Beskrivning:\t");
                Console.ForegroundColor = white;
                Console.WriteLine(this.Description);

                Console.ForegroundColor = green;
                Console.Write("Plats:\t\t");
                Console.ForegroundColor = white;
                Console.WriteLine(this.Location);

                Console.ForegroundColor = green;
                Console.Write("Datum:\t\t");
                Console.ForegroundColor = white;
                Console.WriteLine(this.Date);

                Console.ForegroundColor = green;
                Console.Write("Tid:\t\t");
                Console.ForegroundColor = white;
                Console.WriteLine(this.Time);

                Console.ForegroundColor = green;
                Console.Write("Pris:\t\t");
                Console.ForegroundColor = white;
                Console.WriteLine(this.Price);
                
                Console.WriteLine();


                switch (createTurn)
                {
                    
                    case 1: //fill in name of event
                        Console.SetCursorPosition(16, 1);
                        this.Name = Console.ReadLine();
                        if (!isEditing)
                        {
                            this.Description = ""; //clear field for nice look
                        }
                        break;
                   
                    case 2:  //fill in description
                        Console.SetCursorPosition(16, 2);
                        this.Description = Console.ReadLine();
                        break;
                   
                    case 3:  //fill in location
                        var location = SetLocationForEvent(locationNames);
                        this.Location = location.Name;
                        this.LocationId = location.Id;
                        if (!isEditing)
                        {
                            this.Date = "";
                        }
                        break;
                    
                    case 4: //fill in date
                        Console.SetCursorPosition(16, 4);
                        string date = Console.ReadLine();
                        isDateCorrect = DateTime.TryParse(date, out DateTime myDate);
                        if (isDateCorrect && !date.Contains(":"))
                        {
                            string correctDate = myDate.ToString();
                            this.Date = correctDate;
                            this.Time = "";
                        }
                        else
                        {
                            isDateCorrect = false;
                            this.Date = date;
                            createTurn--;
                        }
                        break;
                    
                    case 5: //fill in time
                        Console.SetCursorPosition(16, 5);
                        string time = Console.ReadLine();
                        isTimeCorrect = TimeSpan.TryParse(time, out TimeSpan myTime);
                        if (isTimeCorrect && time.Contains(":"))
                        {
                            string correctTime = myTime.ToString();
                            this.Time = correctTime;
                        }
                        else
                        {
                            this.Time = time;
                            isTimeCorrect = false;
                            createTurn--;
                        }
                        break;
                    
                    case 6: //fill in price
                        Console.SetCursorPosition(16, 6);
                        string price = Console.ReadLine();
                        isPriceCorrect = Int32.TryParse(price, out int myPrice);
                        if (isPriceCorrect)
                        {
                            this.Price = myPrice;
                        }
                        else
                        {
                            createTurn--;
                        }
                        break;
                    case 7:
                        isCreating = false;
                        break;
                }

                if (!isDateCorrect)
                {
                    Console.SetCursorPosition(27, 4);
                    Console.ForegroundColor = red;
                    Console.Write("felaktigt format, prova ÅÅ-MM-DD");
                    Console.ReadLine();
                    this.Date = "";
                    Console.ForegroundColor = white;
                }
                if (!isTimeCorrect)
                {
                    Console.SetCursorPosition(22, 5);
                    Console.ForegroundColor = red;
                    Console.Write("felaktigt format, prova HH:MM");
                    Console.ReadLine();
                    this.Time = "";
                    Console.ForegroundColor = white;
                }
                if (!isPriceCorrect)
                {
                    Console.SetCursorPosition(22, 6);
                    Console.ForegroundColor = red;
                    Console.Write("felaktigt format, endast nummer");
                    Console.ReadLine();
                    this.Price = 0;
                    Console.ForegroundColor = white;
                }


                createTurn++;
                if (isEditing)
                {
                    createTurn = 7;
                }
                
            }


        }

       
        /// <summary>
        /// method for set location data to event
        /// </summary>
        /// <param name="locations"></param>
        private static Location SetLocationForEvent(List<Location> locations)
        {
            Location location = new Location();
            bool isSelecting = true; //user is selecting a location
            int showRow = 0; //What row currently are showing
            int maxNrLocations = locations.Count -1; //the number of locations in list
            int shownLocation; //the location user picks
            bool showMore; //if can show 10 locations after
            bool showLess = false; //if can show 10 locations before
            int selectedLocation = -1; //if location not selected this is negative

            while (isSelecting)
            {
                Console.Clear();
                Console.WriteLine("Välj en plats för eventet");

                showMore = false;
                shownLocation = 0 + showRow;
                int selectionNr = 0;

                while (shownLocation <= maxNrLocations) //As long as there are locations to show
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{selectionNr}) {locations[shownLocation].Name}");
                    selectionNr++;
                    shownLocation++;
                    
                    if (selectionNr % 10 == 0 && shownLocation != maxNrLocations) //when 10 locations are shown break the loop
                    {
                        showMore = true;
                        break;
                    }
                }
                if (showMore || showLess)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                if (showLess)
                {
                    Console.Write("<<<<--- Bläddra vänster\t\t");
                }
                if (showMore)
                {
                    Console.Write("Bläddra höger------->>>>");
                }
                Console.ForegroundColor = ConsoleColor.White;

                ConsoleKey userChoice = Console.ReadKey().Key;

                switch (userChoice)
                {
                    case ConsoleKey.D0:
                        selectedLocation = 0 + showRow;
                        break;
                    case ConsoleKey.D1:
                        selectedLocation = 1 + showRow;
                        break;
                    case ConsoleKey.D2:
                        selectedLocation = 2 + showRow;
                        break;
                    case ConsoleKey.D3:
                        selectedLocation = 3 + showRow;
                        break;
                    case ConsoleKey.D4:
                        selectedLocation = 4 + showRow;
                        break;
                    case ConsoleKey.D5:
                        selectedLocation = 5 + showRow;
                        break;
                    case ConsoleKey.D6:
                        selectedLocation = 6 + showRow;
                        break;
                    case ConsoleKey.D7:
                        selectedLocation = 7 + showRow;
                        break;
                    case ConsoleKey.D8:
                        selectedLocation = 8 + showRow;
                        break;
                    case ConsoleKey.D9:
                        selectedLocation = 9 + showRow;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (showLess)
                        {
                            showRow -= 10;
                            if (showRow == 0) //if showing the first 10 in list
                            {
                                showLess = false;
                            }
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (showMore)
                        {
                            showRow += 10;
                            showLess = true;
                        }
                        break;
                }

                if (selectedLocation > maxNrLocations) //if picking a number not on the list
                {
                    selectedLocation = -1;
                }
                else if(selectedLocation >= 0)
                {
                    isSelecting = false;
                }


            }// end isSelecting

            location = locations[selectedLocation];


            return location;
            
        }
    }
}
