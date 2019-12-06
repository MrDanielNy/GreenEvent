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

        public void CreateNewLocation() //, EventSystem eventSystem)
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
            while(hasNotMadeSelection)
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
                    Console.WriteLine($"Comparing {location.Name.ToLower()} with {nameOfLocation.ToLower()}");
                    if(location.Name.ToLower() == nameOfLocation.ToLower())
                    {
                        doesItExist = true;
                        Console.WriteLine(doesItExist);
                        break;
                    }
                }

                //Check to see we have at least two chars in the name field, otherwise we prompt the user to either go back or try again.
                if (nameOfLocation.Length >=2 && doesItExist==false)
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
                        switch(userChoice)
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
            if(userInputCorrect)
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
        /// Edit location already saved
        /// </summary>
        public void EditLocation()
        {
            List<Location> currentLocations = new List<Location>();
            currentLocations = GetAllLocations();

            bool isNotDone = true;

            int maxNumberOfLocations = 0;
            string newName = "";
            string newMapLink = "";

            if (currentLocations.Count < 10)
                maxNumberOfLocations = currentLocations.Count;
            else
                maxNumberOfLocations = 9;

            int menuSelectionNr = 0;

            while (isNotDone)
            {
                Console.Clear();

                for (menuSelectionNr=0;menuSelectionNr<maxNumberOfLocations;menuSelectionNr++)
                {
                    Console.WriteLine($"{menuSelectionNr+1}. {currentLocations[menuSelectionNr].Name}");
                }
                Console.WriteLine("Välj vilken plats du vill redigera, backa med Escape-knappen");
                ConsoleKey userChoice = Console.ReadKey().Key;
                int userChoiceAsNumber = 0;
                //Did the user choose a valid number or escape?
                if(int.TryParse(userChoice.ToString().Substring(1, 1), out userChoiceAsNumber)) //Check if second char is a number
                {
                    if (userChoiceAsNumber <= maxNumberOfLocations && userChoiceAsNumber >= 1)
                    {
                        //Show old name
                        Console.WriteLine("Tidigare namn: " + currentLocations[userChoiceAsNumber - 1].Name);
                        Console.Write("Vänligen ange ett nytt namn eller tryck enter för att behålla det gamla: ");
                        newName = Console.ReadLine();
                        if (newName == "") //User just pressed enter, save the old name back
                            newName = currentLocations[userChoiceAsNumber - 1].Name;

                        //Show old maplink
                        Console.WriteLine("Tidigare kartlänk " + currentLocations[userChoiceAsNumber - 1].MapLink);
                        Console.Write("Vänligen skriv in en ny länkadress: ");
                        newMapLink = Console.ReadLine();
                        if (newMapLink == "")
                            newMapLink = currentLocations[userChoiceAsNumber - 1].MapLink;

                        //Save to db at index                      
                        Location location = new Location();
                        location.Name = newName;
                        location.MapLink = newMapLink;
                        location.Id = currentLocations[userChoiceAsNumber - 1].Id;

                        DataBase db = new DataBase();
                        db.EditLocation(location);

                        Console.WriteLine($"Sparade {location.Name} med länk {location.MapLink}. Tryck enter för att gå vidare");
                        isNotDone = false;
                    }
                    else //Not a correct choise
                    {
                        Console.WriteLine("Vänligen gör om ditt val eller prova igen.");
                    }
                }
                else if (userChoice == ConsoleKey.Escape) //Hit escape, show previous menu
                {
                    isNotDone = false;
                }
                else 
                {
                    Console.WriteLine("Vänligen gör om ditt val eller prova igen.");
                }
            }
        }

        /// <summary>
        /// Get all locations from the database
        /// </summary>
        /// <returns>A list of locations</returns>
        public List<Location> GetAllLocations()
        {
            DataBase db = new DataBase();
            return db.GetAllLocations();
        }
    }
}

