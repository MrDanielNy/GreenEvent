using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace GreenEvent
{
    class DataBase
    {
        private readonly string connectionString = "Data Source=localhost;Initial Catalog=GreenEvent;Integrated Security=True";

        public User GetUserByUsername(string username)
        {
            string sqlQuery = "SELECT * FROM [User] WHERE [Username] LIKE @username"; // Query to run against the DB

            User user = null; // Create a new user without any value
            int userRoleId = 0;

            using (SqlConnection myConnection = new SqlConnection(connectionString)) // Prepare connection to the db
            {
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, myConnection); // Prepare the query for the db

                sqlCommand.Parameters.AddWithValue("@username", username); // Add username to the query 

                myConnection.Open(); // Open connection to the db

                using (SqlDataReader dataReader = sqlCommand.ExecuteReader()) // Run query on db
                {
                    if (dataReader.Read()) // Read response from db (first row)
                    {
                        user = new User(); // create new User object

                        user.Id = int.Parse(dataReader["Id"].ToString()); // Set user Id from db
                        user.UserName = dataReader["Username"].ToString(); // Set user Username from db
                        user.Password = dataReader["Password"].ToString(); // Set user Password from db
                        userRoleId = int.Parse(dataReader["RoleId"].ToString());

                    }

                    myConnection.Close(); // Close connection to the db
                }
            }

            if (user != null)
            {
                user.Role = GetRoleNameById(userRoleId); //set user role from database
            }


            return user; // Return the user
        }

        private string GetRoleNameById(int roleId)
        {
            string sqlQuery = "SELECT Name FROM [Role] WHERE Id = @roleId";
            string roleName = "";
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, myConnection);
                sqlCommand.Parameters.AddWithValue("@roleId", roleId);

                myConnection.Open();

                using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    if (dataReader.Read())
                    {
                        roleName = dataReader["Name"].ToString();
                    }

                    myConnection.Close();
                }
            }

            return roleName;
        }

        public void AddUser(User user)
        {
            
            int roleId; //need an int to set data in user RoleId

            if (user.Role == "Admin")
            {
                roleId = 1;
            }
            else
            {
                roleId = 2;
            }


            string sqlQuery = "INSERT INTO[User](Username, [Password], RoleId) VALUES (@username, @password, @role)";
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, myConnection);
                sqlCommand.Parameters.AddWithValue("@username", user.UserName);
                sqlCommand.Parameters.AddWithValue("@password", user.Password);
                sqlCommand.Parameters.AddWithValue("@role", roleId);

                myConnection.Open();

                using SqlDataReader dataReader = sqlCommand.ExecuteReader();

                myConnection.Close();
            }

        }

        /// <summary>
        /// Adds a new location to the database
        /// </summary>
        /// <param name="newLocation">Use a location object as parameter.</param>
        public void AddLocation(Location newLocation)
        {
            string sqlQuery = "INSERT INTO[Location]([Name], [MapLink]) VALUES(@Name, @MapLink)";
            using(SqlConnection myConnection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, myConnection);
                sqlCommand.Parameters.AddWithValue("@Name", newLocation.Name);
                sqlCommand.Parameters.AddWithValue("@MapLink", newLocation.MapLink);

                myConnection.Open();

                using SqlDataReader dataReader = sqlCommand.ExecuteReader();

                myConnection.Close();
            }
        }

        /// <summary>
        /// Edit a location previously saved to the database
        /// </summary>
        /// <param name="newLocation">A lcoation object</param>
        public void EditLocation(Location newLocation)
        {
            //        cmdUpdate.CommandText = "UPDATE `user` SET `todo`=@todo WHERE `username` LIKE '" + _naam + "'";
            string sqlQuery = "UPDATE [Location] SET [Name]=@Name, [MapLink]=@MapLink WHERE [Id]=@locId";
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, myConnection);
                sqlCommand.Parameters.AddWithValue("@Name", newLocation.Name);
                sqlCommand.Parameters.AddWithValue("@MapLink", newLocation.MapLink);
                sqlCommand.Parameters.AddWithValue("@locId", newLocation.Id);
                myConnection.Open();

                using SqlDataReader dataReader = sqlCommand.ExecuteReader();

                myConnection.Close();
            }
        }

        /// <summary>
        /// Connect to the database, download the locations stored.
        /// </summary>
        /// <returns>A list of locationobjects</returns>
        public List<Location> GetAllLocations()
        {
            List<Location> locations = new List<Location>();
            string sqlQuery = "SELECT * FROM [Location]";
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, myConnection);
                myConnection.Open();
                using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Location location = new Location();
                        location.Id = int.Parse(dataReader["Id"].ToString());
                        location.Name = dataReader["Name"].ToString();
                        location.MapLink = dataReader["MapLink"].ToString();
                        locations.Add(location);
                    }
                    myConnection.Close();
                }
            }
            return locations;
        }
    }
}
