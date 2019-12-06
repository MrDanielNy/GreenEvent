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

        /// <summary>
        /// Check if passwords match eachother
        /// </summary>
        /// <param name="password"></param>
        /// <returns>True or False</returns>
        public bool CheckPassword(string password)
        {
            return Password == password;
        }

        /// <summary>
        /// Method to send data for register new User
        /// </summary>
        /// <param name="rolename">Used to select if this is a user or a admin.</param>
        static public User RegisterNewUser(string rolename)
        {

            string newUsername; //var for new users name
            string newPassword; //var for new users password
            DataBase dataBase = new DataBase();

            User controlUser; //user object to check new registration
            do
            {
                Console.Write("Välj ett användarnamn: ");
                newUsername = Console.ReadLine();

                controlUser = dataBase.GetUserByUsername(newUsername);

                if (newUsername.Length < 3)
                {
                    Console.WriteLine($"Användarnamn {newUsername} är för kort..");
                    Console.ReadLine();
                    //tryInput = false;
                }
                else if(controlUser != null)
                {
                    Console.WriteLine($"Användarnamn {newUsername} existerar redan..");
                    Console.ReadLine();
                    //tryInput = true;
                }

            } while (controlUser != null || newUsername.Length < 3);

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
                    newPassword = null;
                }
                else
                {
                    //tryInput = true;
                }

            } while (newPassword == null);

            User newUser = new User();

            newUser.UserName = newUsername;
            newUser.Password = newPassword;
            newUser.Role = rolename;

            DataBase db = new DataBase();
            
            db.AddUser(newUser);
            //get the new user and set user Id 
            newUser = db.GetUserByUsername(newUsername);

            return newUser;
        }
    }

}
