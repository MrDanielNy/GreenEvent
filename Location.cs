using System;
using System.Collections.Generic;
using System.Text;

namespace GreenEvent
{
    class Location
    {
        public int Id;
        public string Name;
        public string MapLink;

        public void CreateNewLocation()
        {
            //Initiate Variables
            string nameOfLocation = "";
            string linkToLocation = "";
            bool hasNotMadeSelection = true; //Handles if the user has made a selection or not
            bool userInputCorrect = true; //Handles the check if the user input is correct

            List<Location> currectLocations = new List<Location>();
            currectLocations = GetAllLocations();

            DataBase dataBase = new DataBase();

            //Handle menu
            while (hasNotMadeSelection)
            {
                Console.Clear();
                Console.WriteLine("Platser sparade: ");
                foreach (Location location in currectLocations)
                {
                    Console.WriteLine(location.Name);
                }
                Console.Write("Ange namn på den nya platsen: ");
                nameOfLocation = Console.ReadLine();
                Console.Write("Ange länk till plats på karta: ");
                linkToLocation = Console.ReadLine();

                //Check if this location doesn't excist.
                bool doesItExist = false;
                foreach (Location location in currectLocations)
                {
                    if (location.Name.ToLower() == nameOfLocation.ToLower())
                    {
                        doesItExist = true;
                        Console.WriteLine(doesItExist);
                        break;
                    }
                }

                //Check to see we have at least two chars in the name field, otherwise we prompt the user to either go back or try again.
                if (nameOfLocation.Length >= 2 && doesItExist == false)
                {
                    hasNotMadeSelection = false;
                    userInputCorrect = true;
                }
                else
                {
                    bool willTheUserTryAgainMenu = true;
                    while (willTheUserTryAgainMenu)
                    {
                        Console.Clear();
                        Console.WriteLine("Namnet måste vara längre än 2 tecken eller så fanns redan platsen. Tryck ett (1) för att försöka igen eller Escape för att backa");
                        ConsoleKey userChoice = Console.ReadKey().Key;
                        switch (userChoice)
                        {
                            case ConsoleKey.D1:
                                willTheUserTryAgainMenu = false;
                                break;
                            case ConsoleKey.Escape:
                                willTheUserTryAgainMenu = false;
                                hasNotMadeSelection = false;
                                userInputCorrect = false; //Make sure the data is not upploaded to the database but exit both while-loops.
                                break;
                        }

                    }
                }
            }

            //if we have a correctly given location then add it to the database.
            if (userInputCorrect)
            {
                Location location = new Location();
                location.Name = nameOfLocation;
                location.MapLink = linkToLocation;

                DataBase db = new DataBase();
                db.AddLocation(location);

                Console.WriteLine($"Sparade {location.Name} med länk {location.MapLink}. Tryck enter för att gå vidare");
                Console.ReadLine();
            }
        }
        /// <summary>
        /// Select a location from location database
        /// </summary>
        /// <returns>location</returns>
        public Location SelectLocation()
        {
            var locations = GetAllLocations();

            Location location = new Location();
            bool isSelecting = true; //user is selecting a location
            int showRow = 0; //What row of 10 currently showing
            int maxNrLocations = locations.Count - 1; //the number of locations in list
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
                else if (selectedLocation >= 0)
                {
                    isSelecting = false;
                }


            }// end isSelecting

            location = locations[selectedLocation];

            return location;

        }



        /// <summary>
        /// Edit location already saved
        /// </summary>
        public void EditLocation()
        {
            //get a location to edit
            Location location = SelectLocation();

            if (location == null)
            {
                Console.WriteLine("Skapa en plats först");
                Console.ReadLine();
                return;
            }

            Console.Clear();

            //Show old name
            Console.WriteLine("Tidigare namn: " + location.Name);
            Console.Write("Vänligen ange ett nytt namn eller tryck enter för att behålla det gamla: ");
            string newName = Console.ReadLine();
            if (newName == "") //User just pressed enter, save the old name back
                newName = location.Name;

            //Show old maplink
            Console.WriteLine("Tidigare kartlänk " + location.MapLink);
            Console.Write("Vänligen skriv in en ny länkadress eller tryck enter för att behålla den gamla: ");
            string newMapLink = Console.ReadLine();
            if (newMapLink == "")  //User just pressed enter, save the old maplink back
                newMapLink = location.MapLink;

            location.Name = newName;
            location.MapLink = newMapLink;

            DataBase db = new DataBase();
            db.EditLocation(location);

            Console.WriteLine($"Sparade {location.Name} med länk {location.MapLink}. Tryck enter för att gå vidare");

            Console.ReadLine();

        }

        /// <summary>
        /// Get all locations from the database
        /// </summary>
        /// <returns>A list of locations</returns>
        private List<Location> GetAllLocations()
        {
            DataBase db = new DataBase();
            return db.GetAllLocations();
        }
    }
}

