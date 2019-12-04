using System;
using System.Collections.Generic;
using System.Text;

namespace GreenEvent
{
    class User
    {
        public int Id;
        public string UserName;
        public string Password;
        public string Role;
        User controlUser = null; //user object to check new registration


        public bool CheckPassword(string password)
        {
            return Password == password;
        }

        /// <summary>
        /// method to send data for register new User
        /// </summary>
        public User RegisterNewUser(string rolename)
        {

            string newUsername; //var for new users name
            string newPassword; //var for new users password
            DataBase dataBase = new DataBase();


            bool tryInput; //if new input is incorrect bool will be false

            do
            {
                Console.Write("Välj ett användarnamn: ");
                newUsername = Console.ReadLine();

                controlUser = dataBase.GetUserByUsername(newUsername);

                if (controlUser != null || newUsername.Length < 3)
                {
                    Console.WriteLine($"Användarnamn {newUsername} existerar redan eller är för kort..");
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
                Console.Write("Välj ett lösenord: ");
                newPassword = Console.ReadLine();
                Console.Write("Upprepa lösenord: ");
                string controlPassword = Console.ReadLine();
                if (newPassword != controlPassword || newPassword.Length < 4)
                {
                    Console.WriteLine("Lösenordet matchar inte eller är för kort..");
                    Console.ReadLine();
                    tryInput = false;
                }
                else
                {
                    tryInput = true;
                }

            } while (!tryInput);

            User user = CreateUser(newUsername, newPassword, rolename);
            return user;
        }


        static public User CreateUser(string username, string password, string rolename)
        {
            User newUser = new User();

            newUser.UserName = username;
            newUser.Password = password;
            newUser.Role = rolename;


            DataBase db = new DataBase();

            //DataBase.AddUser(newUser);
            //Skicka newUser till databasen
            db.AddUser(newUser);

            //Hämta tillbaka det nyassignade id:t från databasen och lägg till på newUser.Id
            newUser = db.GetUserByUsername(username);


            return newUser;
        }

        static public User CreateAdmin(string username, string password)
        {
            User newUser = new User();

            return newUser;
        }
        static public void GetJoinedEvent(int UserId)
        {
            //Events user is assigned to
            DataBase db = new DataBase();
            List<string> userEvents = db.GetEventNameByUser(UserId);
            int i = 1;
            foreach (string s in userEvents)
            {
                Console.WriteLine(i + ") " + s);
                i++;
            }
            Console.Write("\nTryck enter för att gå tillbaks..");
            Console.ReadLine();
        }

        static public void GetAvailableEvent(int UserId)
        {
            //Events user is not assigned to
            DataBase db = new DataBase();
            var userAvailableEvents = new List<Event>();
            //userAvailableEvents = db.GetAvailableEvents(UserId);
            userAvailableEvents = db.GetAllEvents();
            //userAvailableEvents.Name = db.GetAvailableEvents(UserId);
            int i = 1;
            Console.Clear();
            foreach (Event currentEvent in userAvailableEvents)
            {
                Console.WriteLine(i + ") " + currentEvent.Name);
                i++;
            }
            Console.Write("Välj ett event 1 to " + (i - 1) + " att joina: ");
            string input = Console.ReadLine();
            int.TryParse(input, out int userChoice);

            //ConsoleKeyInfo tempChoice = Console.ReadKey();
            //int userChoice = int.Parse(tempChoice.KeyChar.ToString());
            
            db.JoinEvent(UserId, userChoice);

        }
    }

}
