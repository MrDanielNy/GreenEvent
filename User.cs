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

        
        public bool CheckPassword(string password)
        {
            return Password == password;
        }


        static public User CreateUser(string username, string password)
        {
            User newUser = new User();

            newUser.UserName = username;
            newUser.Password = password;
            newUser.Role = "User";


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
        public void GetJoinedEvent(int UserId)
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
        }

        public void GetAvailableEvent(int UserId)
        {
            //Events user is not assigned to
            DataBase db = new DataBase();
            var userAvailableEvents = new List<Event>();
            userAvailableEvents = db.GetAllEvents();
            //Event userAvailableEvents = new Event();
            //userAvailableEvents.Name = db.GetAvailableEvents(UserId);
            int i = 1;
            foreach (Event currentEvent in userAvailableEvents)
            {
                Console.WriteLine(i + ") " + currentEvent.Name);
                i++;
            }
            Console.WriteLine("Chose an event from " + "1 " + "to " + (i-1) +" to join");
            ConsoleKeyInfo tempChoice = Console.ReadKey();

            int userChoice = int.Parse(tempChoice.KeyChar.ToString());
            db.JoinEvent(UserId, userChoice);
           
        }

        
    }

}
