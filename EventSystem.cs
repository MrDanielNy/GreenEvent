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
                    case ConsoleKey.D0:
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

            Console.WriteLine($"Login successfull as {loggedInUser.Role}");

            if (loggedInUser.Role == "Admin")
            {
                ShowMenu();
            }
            else
            {
                Console.WriteLine("Travelmenu not yet implemented");
                ShowMenu();
            }



        }

        //This function is not completed
        /// <summary>
        /// ShowMenu
        /// </summary>
        /// <returns></returns>
        private void ShowMenu()
        {
            bool running = true;

            while (running)
            {

                //changed exit to 0
                if(loggedInUser.Role == "Admin")
                {
                    Console.WriteLine($"Welcome {loggedInUser.UserName}, showing Admin-menu");
                    Console.WriteLine("1) Hello");
                    Console.WriteLine("0) Exit");
                } else
                {
                    Console.WriteLine($"Welcome {loggedInUser.UserName}, showing User-menu");
                    Console.WriteLine("1) Hello");
                    Console.WriteLine("0) Exit");
                }
                

                ConsoleKey userChoice = Console.ReadKey().Key;

                switch (userChoice)
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        Console.WriteLine("Hello!! :)");
                        break;
                    case ConsoleKey.D0:
                        Console.Clear();
                        loggedInUser = null;
                        running = false;
                        break;
                }

            }
                                                     
        } //end of showMenu
    }
}
