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
    }

}
