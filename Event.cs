using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GreenEvent
{
    class Event
    {
        public int Id { get; set; }
        public string Name { get; set; } //Name change nr 1
        public string Description { get; set; } = "en liten beskrivning"; //Description change nr 2
        
        //when getting data from database trim the string
        //for a nice output
        private string _date = "2019-01-01"; //Date change nr 4
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
        private string _time = "00:00";     //Time change nr 5
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
        public int Price { get; set; } //Price change nr 6
        public string Location { get; set; } = "plats där eventet ska hållas"; //Location change nr 3
        public int LocationId { get; set; }

        public DataBase database = new DataBase();

        public void ShowEvent()
        {
            List<User> users = database.GetUsersByEventId(this.Id);

            ModifyEvent(7);

            Console.SetCursorPosition(60, 0);
            Console.Write("--<<Användare som har joinat detta event>>--");

            if (users.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(67, 3);
                Console.Write("Inga användare har joinat än");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                for (int i = 0; i < users.Count ; i++)
                {
                    Console.SetCursorPosition(74, i + 2);
                    Console.Write(users[i].UserName);
                }
            }

            Console.ForegroundColor = ConsoleColor.White;

            Console.ReadLine();

        }

        public void EditEvent()
        {
            bool isRunning = true;

            while (isRunning)
            {
                ModifyEvent(7);

                Console.WriteLine("Vad vill du redigera?\n");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("1) Namn");
                Console.WriteLine("2) Beskrivning");
                Console.WriteLine("3) Plats");
                Console.WriteLine("4) Datum");
                Console.WriteLine("5) Tid");
                Console.WriteLine("6) Pris\n");

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("T) Ta bort event");
                Console.WriteLine("S) Spara ändringar\n");
                
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Gå tillbaka utan att spara med esc..");
                

                ConsoleKey userChoice = Console.ReadKey().Key;

                switch (userChoice)
                {
                    case ConsoleKey.D1:
                        this.Name = "";
                        ModifyEvent(1);
                        break;
                    case ConsoleKey.D2:
                        this.Description = "";
                        ModifyEvent(2);
                        break;
                    case ConsoleKey.D3:
                        this.Location = "";
                        ModifyEvent(3);
                        break;
                    case ConsoleKey.D4:
                        this.Date = "";
                        ModifyEvent(4);
                        break;
                    case ConsoleKey.D5:
                        this.Time = "";
                        ModifyEvent(5);
                        break;
                    case ConsoleKey.D6:
                        this.Price = 0;
                        ModifyEvent(6);
                        break;
                    case ConsoleKey.S:
                        database.EditEvent(this);
                        isRunning = false;
                        break;
                    case ConsoleKey.T:
                        isRunning = DeleteEvent(this.Id);
                        break;
                    case ConsoleKey.Escape:
                        Console.Clear();
                        isRunning = false;
                        break;
                    default:
                        Console.Clear();
                        break;
                }
            }
           

        }
        
        
        private bool DeleteEvent(int eventId)
        {
            Console.WriteLine("");
            Console.WriteLine("Är du säker på att du vill ta bort eventet?");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("J) Ta bort");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("N) Ångra");
            Console.ForegroundColor = ConsoleColor.White;

            ConsoleKey userChoice = Console.ReadKey().Key;

            if (userChoice == ConsoleKey.J)
            {
                database.DeleteEvent(eventId);
                return false;
            }
            else
            {
                return true;
            }

        }
        
        /// <summary>
        /// originally for just creating event but changed til also view and edit
        /// depending on what number you send in (0 for creating, 1-6 for editing and 7 for view
        /// </summary>
        /// <param name="createTurn"></param>
        public void ModifyEvent(int createTurn)
        {
            var locationNames = database.GetAllLocations(); //create list of locations
            if (locationNames.Count == 0)  //if no locations return to menu
            {
                Console.WriteLine("Du måste skapa en plats innan du kan skapa ett event..");
                Console.ReadLine();
                return;
            }

            bool isCreating = true; //while event is being created
            bool isEditing = true; //if event is being edited or showed


            if (createTurn == 0)
            {
                createTurn = 1; //witch field user is going to fill in
                isEditing = false; //creating a new Event
            }

            
            bool isNameCorrect = true;
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
                else if (isEditing && createTurn == 7)
                {
                    Console.WriteLine("----<<<<<<Valt event>>>>>>----");
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
                        string name = Console.ReadLine();
                        isNameCorrect = (name.Length > 3 && name.Length < 31);
                        
                        if (isNameCorrect)
                        {
                            this.Name = name;
                            if (!isEditing)
                            {
                                this.Description = ""; //clear field for nice look
                            }
                        }
                        else
                        {
                            isNameCorrect = false;
                            createTurn--;
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
                            if (!isEditing)
                            {
                                this.Time = "";
                            }
                            
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
                        if (price == "")
                        {
                            price = "0";
                        }
                        isPriceCorrect = int.TryParse(price, out int myPrice);
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
                if (!isNameCorrect)
                {
                    Console.SetCursorPosition(22, 1);
                    Console.ForegroundColor = red;
                    Console.Write("namnet ska innehålla minst 4 och högst 30 tecken");
                    Console.ReadLine();
                    this.Name = "";
                    Console.ForegroundColor = white;
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

                Console.SetCursorPosition(0, 10);

                createTurn++;

                if (isEditing && isNameCorrect && isDateCorrect && isTimeCorrect && isPriceCorrect)
                {
                    isCreating = false;
                }
                
            }

            if (!isEditing)
            {
                Console.WriteLine("Vill du spara det här eventet?");
                Console.Write("(J)A / (N)EJ");

                while (true)
                {
                    ConsoleKey userChoice = Console.ReadKey().Key;

                    if (userChoice == ConsoleKey.J)
                    {
                        database.AddEvent(this);
                        break;
                    }
                    else if (userChoice == ConsoleKey.N)
                    {
                        break;
                    }
                }

            }


        }

       
        /// <summary>
        /// method for set location to event
        /// </summary>
        /// <param name="locations"></param>
        private static Location SetLocationForEvent(List<Location> locations)
        {
            Location location = new Location();
            bool isSelecting = true; //user is selecting a location
            int showRow = 0; //What row of 10 currently showing
            int maxNrLocations = locations.Count -1; //the number of locations in list
            int shownLocation; //the location user picks
            bool showMore; //show the next 10 locations
            bool showLess = false; //show the 10 locations before
            int selectedLocation = -1; //if location not selected this is -1

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
                    
                    if (selectionNr % 10 == 0 && shownLocation - 1 != maxNrLocations) //when 10 locations are shown break the loop
                    {
                        showMore = true;
                        break;
                    }
                }
                if (showMore || showLess)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                if (showLess)
                {
                    Console.Write("<<<<--- Bläddra vänster\t");
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
        public static int ShowAllEvents()
        {
            int eventId; //return if event is selected,  -1 if not
            DataBase database = new DataBase();
            var allEvents = database.GetAllEvents(); //create list of events..

            //check if there is any events to list
            if (allEvents.Count == 0)
            {
                Console.WriteLine("Det finns inga event...");
                Console.ReadLine();
                return -1;
            }

            bool isSelecting = true; //user is looking at events
            int showRow = 0; //Which 10 events currently showing
            int maxNrEvents = allEvents.Count - 1; //the number of events in list 0-based
            int usersPick; //the event user picks with showRow to get correct indexnumber
            bool showMore; //shows the ten foremost events
            bool showLess = false; //shows the 10 later events
            int selectedEvent = -1; //if event not selected this is -1

            while (isSelecting)
            {
                Console.Clear();
                Console.WriteLine("     Välj ett event eller ecs för att gå tillbaka");

                showMore = false;
                usersPick = 0 + showRow; //the indexnumber in list
                int selectionNr = 0; //the number shown in console


                while (usersPick <= maxNrEvents) //As long as there are locations to show
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"{selectionNr}) {allEvents[usersPick].Name}");
                    Console.SetCursorPosition(44, selectionNr + 1);
                    Console.WriteLine($"{allEvents[usersPick].Date}");
                    selectionNr++;
                    usersPick++;
                    if (selectionNr % 10 == 0 && usersPick -1 != maxNrEvents) //when 10 locations are shown break the loop
                    {
                        showMore = true;
                        break;
                    }
                }


                if (showMore || showLess)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                if (showLess)
                {
                    Console.Write("<<<<--- Bläddra vänster       ");
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
                        selectedEvent = 0 + showRow;
                        break;
                    case ConsoleKey.D1:
                        selectedEvent = 1 + showRow;
                        break;
                    case ConsoleKey.D2:
                        selectedEvent = 2 + showRow;
                        break;
                    case ConsoleKey.D3:
                        selectedEvent = 3 + showRow;
                        break;
                    case ConsoleKey.D4:
                        selectedEvent = 4 + showRow;
                        break;
                    case ConsoleKey.D5:
                        selectedEvent = 5 + showRow;
                        break;
                    case ConsoleKey.D6:
                        selectedEvent = 6 + showRow;
                        break;
                    case ConsoleKey.D7:
                        selectedEvent = 7 + showRow;
                        break;
                    case ConsoleKey.D8:
                        selectedEvent = 8 + showRow;
                        break;
                    case ConsoleKey.D9:
                        selectedEvent = 9 + showRow;
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
                            showRow += 10; //now we see indexnumber 0+10 in list and so on..
                            showLess = true;
                        }
                        break;
                    case ConsoleKey.Escape:
                        isSelecting = false;
                        break;
                }

                if (selectedEvent > maxNrEvents) //if picking a number not on the list
                {
                    selectedEvent = -1;
                }
                else if (selectedEvent >= 0)
                {
                    isSelecting = false;
                }


            }// end isSelecting


            if (selectedEvent == -1)
            {
                return -1;
            }
            else
            {
                eventId = allEvents[selectedEvent].Id;
                return eventId;
            }

            
        }
    }
}
