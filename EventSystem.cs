using System;
using System.Collections.Generic;
using System.Text;

namespace GreenEvent
{
    class EventSystem
    {

        private User loggedInUser = null;
        private DataBase database = new DataBase();
        public int eventId; //id for getting event
        User User = new User();

        public void Start()
        {

            Console.WriteLine("Välkommen till GreenEvent");

            bool running = true;

            while (running)
            {
                string rolename = "User";

                Console.WriteLine("Gör ett val:");
                Console.WriteLine("1) Logga in");
                Console.WriteLine("2) Registrera en ny användare");
                Console.WriteLine("Esc) Avsluta");

                ConsoleKey userChoice = Console.ReadKey().Key;

                switch (userChoice)
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        LogIn();
                        break;
                    case ConsoleKey.D2:
                        Console.Clear();
                        loggedInUser = User.RegisterNewUser(rolename);
                        ShowMenu();
                        break;
                    case ConsoleKey.Escape:
                        running = false;
                        break;
                }
            }
            
        }// end Start

       
       

        /// <summary>
        /// Method to log in
        /// </summary>
        private void LogIn()
        {
            Console.Clear();
            while (loggedInUser == null)
            {
                Console.Write("Ange användarnamn: ");
                string username = Console.ReadLine();

                Console.Write("Ange lösenord: ");
                string password = Console.ReadLine();

                User user = database.GetUserByUsername(username);

                if (user != null)
                {
                    bool correctPassword = user.CheckPassword(password);

                    if (correctPassword)
                    {
                        loggedInUser = user;
                    }
                    else
                    {
                        Console.WriteLine("Fel användarnamn eller lösenord");
                    }
                }
                else
                {
                    Console.WriteLine("Fel användarnamn eller lösenord");
                }

            }

            Console.WriteLine($"{loggedInUser.Role} är inloggad.");

            ShowMenu();
        }

        //This function is not completed
        /// <summary>
        /// ShowMenu
        /// </summary>
        /// <returns></returns>
        private void ShowMenu()
        {
            bool running = true;
            Location location = new Location();
            string rolename = "Admin";

            while (running)
            {
                Console.Clear();
                //changed exit to 0
                if(loggedInUser.Role == "Admin")
                {
                    Console.WriteLine($"Välkommen {loggedInUser.UserName}, gör ditt val");
                    Console.WriteLine("1) Skapa event.");
                    Console.WriteLine("2) Redigera event.");
                    Console.WriteLine("3) Visa tillgängliga event.");
                    Console.WriteLine("4) Skapa plats.");
                    Console.WriteLine("5) Redigera plats.");
                    Console.WriteLine("6) Skapa admin.");
                    
                    Console.WriteLine("Esc för att logga ut.");
                } else
                {
                    Console.WriteLine($"Välkommen {loggedInUser.UserName}, gör ditt val.");
                    Console.WriteLine("1) Visa event du anmält dig till.");
                    Console.WriteLine("2) Visa tillgängliga event.");

                    Console.WriteLine("Esc för att logga ut.");
                }
                

                ConsoleKey userChoice = Console.ReadKey().Key;

                switch (userChoice)
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        Event newEvent = new Event();
                        newEvent.ModifyEvent(0);
                        break;
                    case ConsoleKey.D2:
                        eventId = Event.ShowAllEvents();
                        if (eventId != -1)
                        {
                            var myEvent = database.GetEventByEventId(eventId);
                            myEvent.EditEvent();
                        }
                        if(loggedInUser.Role == "Admin")
                        {
                            Console.Clear();
                            Console.WriteLine("Inte implementerad adminmeny");
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Inte implementerad usermenyu");
                        }
                        break;
                    case ConsoleKey.D3:
                        Console.Clear();
                        eventId = Event.ShowAllEvents();
                        if (eventId != -1)
                        {
                            var myEvent = database.GetEventByEventId(eventId);
                            myEvent.ShowEvent(loggedInUser.Id);
                        }
                        break;
                    case ConsoleKey.D4:
                        location.CreateNewLocation();
                        break;
                    case ConsoleKey.D5:
                        location.EditLocation();
                        break;
                    case ConsoleKey.D6:
                        Console.Clear();
                        loggedInUser.RegisterNewUser(rolename);
                        break;
                    case ConsoleKey.Escape:
                        Console.WriteLine("Loggar ut...");
                        Console.Clear();
                        loggedInUser = null;
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Felaktigt val, försök igen.");
                        break;
                }

            }
                                                     
        } //end of showMenu


        //private void AddNewAdminPost()
        //{
        //    string newPostBody;
        //    int newPostUserId;
        //    //int newPostEventId;
        //    bool tryInput;

        //    do
        //    {
        //        Console.Write("Skriv inlägg: ");
        //        newPostBody = Console.ReadLine();
        //        newPostUserId = loggedInUser.Id;


        //        if (newPostBody == null)
        //        {
        //            Console.WriteLine($"Inlägg kan inte vara tomt");
        //            Console.ReadLine();
        //            tryInput = false;
        //        }
        //        else
        //        {
        //            tryInput = true;
        //        }

        //    } while (!tryInput);


        //    AdminPost.CreateAdminPost(newPostBody, newPostUserId);

        //}

    }
}
