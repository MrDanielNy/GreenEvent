using System;
using System.Collections.Generic;
using System.Text;

namespace GreenEvent
{
    class EventSystem
    {

        private User loggedInUser = null;
        
        
        private DataBase database = new DataBase();
        private int eventId; //id for getting event
        private bool isAdmin = false; //for using menu with user and admin
        
        
        //User User = new User();



        public void Start()
        {

            Console.WriteLine("Välkommen till GreenEvent");

            bool running = true;

            while (running)
            {
                //string rolename = "User";

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
                        loggedInUser = User.RegisterNewUser("User");
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
            if (loggedInUser.Role == "Admin")
            {
                isAdmin = true;
            }
            bool running = true;
            Location location = new Location();
            //string rolename = "Admin";

            

            while (running)
            {
                Console.Clear();
                //changed exit to 0
                
                if(isAdmin)
                {
                    Console.WriteLine($"Välkommen {loggedInUser.UserName}, gör ditt val");
                    Console.WriteLine("1) Skapa event.");
                    Console.WriteLine("2) Redigera event.");
                    Console.WriteLine("3) Visa event.");
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
                        if (isAdmin)
                        {
                            Event newEvent = new Event();
                            newEvent.ModifyEvent(0);
                        }
                        else
                        {
                            eventId = Event.ShowAllEvents(loggedInUser.Id, false);
                            if (eventId != -1)
                            {
                                var myEvent = database.GetEventByEventId(eventId);
                                myEvent.ShowEvent(loggedInUser.Id);
                            }

                            //User.GetJoinedEvent(loggedInUser.Id);
                        }
                        break;
                    case ConsoleKey.D2:
                        Console.Clear();
                        if(isAdmin)
                        {
                            eventId = Event.ShowAllEvents(-1, true);
                            if (eventId != -1)
                            {
                                var myEvent = database.GetEventByEventId(eventId);
                                myEvent.EditEvent();
                            }
                        }
                        else
                        {
                            eventId = Event.ShowAllEvents(loggedInUser.Id, true);
                            if (eventId != -1)
                            {
                                var myEvent = database.GetEventByEventId(eventId);
                                myEvent.ShowEvent(loggedInUser.Id);
                            }

                            //User.GetAvailableEvent(loggedInUser.Id);
                        }
                        break;
                    case ConsoleKey.D3:
                        if (isAdmin)
                        {
                            Console.Clear();
                            eventId = Event.ShowAllEvents(-1, true);
                            if (eventId != -1)
                            {
                                var myEvent = database.GetEventByEventId(eventId);
                                myEvent.ShowEvent(loggedInUser.Id);
                            }
                        }
                        break;
                    case ConsoleKey.D4:
                        if (isAdmin)
                        {
                            location.CreateNewLocation();
                        }
                        break;
                    case ConsoleKey.D5:
                        if (isAdmin)
                        {
                            location.EditLocation();
                        }
                        
                        
                        break;
                    case ConsoleKey.D6:
                        if (isAdmin)
                        {
                            Console.Clear();
                            User.RegisterNewUser("Admin");
                        }
                        break;
                    case ConsoleKey.Escape:
                        Console.WriteLine("Loggar ut...");
                        Console.Clear();
                        isAdmin = false;
                        loggedInUser = null;
                        running = false;
                        break;
                    default:
                        //Console.WriteLine("Felaktigt val, försök igen.");
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
