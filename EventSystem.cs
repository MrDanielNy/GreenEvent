using System;
using System.Collections.Generic;
using System.Text;

namespace GreenEvent
{
    class EventSystem
    {

        private User loggedInUser = null;
        private DataBase database = new DataBase();
        private User controlUser = null; //user object to check new registration
        public int eventId; //id for getting event

        public void Start()
        {

            Console.WriteLine("Welcome to GreenEvent");

            bool running = true;

            while (running)
            {
                Console.WriteLine("select an option:");
                Console.WriteLine("1) LogIn");
                Console.WriteLine("2) Register new user");
                Console.WriteLine("0) Exit");

                ConsoleKey userChoice = Console.ReadKey().Key;

                switch (userChoice)
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        LogIn();
                        break;
                    case ConsoleKey.D2:
                        Console.Clear();
                        RegisterNewUser();
                        ShowMenu();
                        break;
                    case ConsoleKey.Escape:
                        running = false;
                        break;
                }
            }
            
        }// end Start

       
        /// <summary>
        /// method to send data for register new User
        /// </summary>
        private void RegisterNewUser()
        {
            string newUsername; //var for new users name
            string newPassword; //var for new users password
           

            bool tryInput; //if new input is incorrect bool will be false

            do
            {
                Console.Write("Enter your username: ");
                newUsername = Console.ReadLine();

                controlUser = database.GetUserByUsername(newUsername);

                if (controlUser != null || newUsername.Length < 3)
                {
                    Console.WriteLine($"Username {newUsername} already exits or is to short..");
                    Console.ReadLine();
                    tryInput = false;
                    controlUser = null;
                }
                else
                {
                    tryInput = true;
                }

            } while (!tryInput);

            do
            {
                Console.Write("Enter your password: ");
                newPassword = Console.ReadLine();
                Console.Write("Repeat password: ");
                string controlPassword = Console.ReadLine();
                if (newPassword != controlPassword || newPassword.Length < 4)
                {
                    Console.WriteLine("Password doesn't match or is to short..");
                    Console.ReadLine();
                    tryInput = false;
                }
                else
                {
                    tryInput = true;
                }

            } while (!tryInput);

            loggedInUser = User.CreateUser(newUsername, newPassword);
            
        }
        /// <summary>
        /// Method to log in
        /// </summary>
        private void LogIn()
        {
            Console.Clear();
            while (loggedInUser == null)
            {
                Console.Write("Write your username: ");
                string username = Console.ReadLine();

                Console.Write("Write your password: ");
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
                        Console.WriteLine("Incorrect username or password");
                    }
                }
                else
                {
                    Console.WriteLine("Incorrect username or password");
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
                    case ConsoleKey.D2:
                        if (loggedInUser.Role == "Admin")
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
                            myEvent.ShowEvent();
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
                        Console.WriteLine("Inte implementerad");
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

        
    }
}
