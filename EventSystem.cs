using System;
using System.Collections.Generic;
using System.Text;

namespace GreenEvent
{
    class EventSystem
    {

        private User loggedInUser = null;
        private DataBase database = new DataBase();

        public void Start()
        {

            Console.WriteLine("Welcome to GreenEvent");

            LogIn();
            
        }

        private void LogIn()
        {
           
            while (loggedInUser == null)
            {
                Console.Write("Write your username: ");
                string username = Console.ReadLine();

                Console.Write("Write your password: ");
                string password = Console.ReadLine();

                //Console.WriteLine($"{username} {password}");


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
                if(loggedInUser.Role == "Admin")
                {
                    Console.WriteLine($"Welcome {loggedInUser.UserName}, showing Admin-menu");
                    Console.WriteLine("1) Hello");
                    Console.WriteLine("2) Exit");
                } else
                {
                    Console.WriteLine($"Welcome {loggedInUser.UserName}, showing User-menu");
                    Console.WriteLine("1) Hello");
                    Console.WriteLine("2) Exit");
                }
                

                ConsoleKey userChoice = Console.ReadKey().Key;

                switch (userChoice)
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        Console.WriteLine("Hello!! :)");
                        break;
                    case ConsoleKey.D2:
                        running = false;
                        break;
                }

               

            }
            
                                                     
        }
    }
}
