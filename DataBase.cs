﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace GreenEvent
{
    class DataBase
    {
        private readonly string connectionString = "Data Source=localhost;Initial Catalog=GreenEvent;Integrated Security=True";



        /// <summary>
        /// Get all users joined to event with sent in eventId 
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public List<User> GetUsersByEventId(int eventId)
        {

            List<User> users = new List<User>();

            string sqlQuery = "SELECT u.Username, u.[Id] FROM[Join] AS j " +
                "LEFT JOIN[User] AS u ON j.[UserId] = u.[Id] " +
                "WHERE j.[EventId] = @id";

            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, myConnection);
                sqlCommand.Parameters.AddWithValue("@id", eventId);
                
                myConnection.Open();

                using SqlDataReader dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    User user = new User();
                    user.Id = int.Parse(dataReader["Id"].ToString());
                    user.UserName = dataReader["Username"].ToString();

                    users.Add(user);
                }

                myConnection.Close();
            }

            return users;

        }
        /// <summary>
        /// check if loggedInUser can join event
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="userId"></param>
        /// <returns>bool</returns>
        public bool IsUserJoined(int eventId, int userId)
        {
            int userIs = 0;
            int roleId = GetRoleIdByUserId(userId);


            string sqlQuery = "SELECT u.Username, u.[Id] FROM[Join] AS j " +
                "LEFT JOIN[User] AS u ON j.[UserId] = u.[Id] " +
                "WHERE j.[EventId] = @eventId AND u.Id = @userId";

            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, myConnection);
                sqlCommand.Parameters.AddWithValue("@eventId", eventId);
                sqlCommand.Parameters.AddWithValue("@userId", userId);

                myConnection.Open();

                using SqlDataReader dataReader = sqlCommand.ExecuteReader();

                if (dataReader.Read())
                {
                    userIs = 1;
                }

                myConnection.Close();
            }
            if (userIs == 1 || roleId == 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// check if loggedInUser is admin or user to check if loggedInUser can join an event
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private int GetRoleIdByUserId(int userId)
        {
            string sqlQuery = "SELECT [RoleId] FROM [User] WHERE [Id] = @userId"; // Query to run against the DB

            int id;            

            using (SqlConnection myConnection = new SqlConnection(connectionString)) // Prepare connection to the db
            {
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, myConnection); // Prepare the query for the db

                sqlCommand.Parameters.AddWithValue("@userId", userId); // Add username to the query 

                myConnection.Open(); // Open connection to the db

                using (SqlDataReader dataReader = sqlCommand.ExecuteReader()) // Run query on db
                {
                    dataReader.Read();
                    id = int.Parse(dataReader["RoleId"].ToString());
                    myConnection.Close(); // Close connection to the db
                }
            }

            return id;
        }
        /// <summary>
        /// delete event and all tables cónnected to event
        /// </summary>
        /// <param name="eventId"></param>
        public void DeleteEvent(int eventId)
        {


            string sqlQuery = "DELETE FROM [JOIN] WHERE [EventId] = @id " +
                "DELETE FROM [POST] WHERE [EventId] = @id " +
                "DELETE FROM [Event] WHERE [Id] = @id ";

            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, myConnection);
                sqlCommand.Parameters.AddWithValue("@id", eventId);

                myConnection.Open();

                using SqlDataReader dataReader = sqlCommand.ExecuteReader();

                myConnection.Close();
            }

        }
        /// <summary>
        /// change data in unique event
        /// </summary>
        /// <param name="myEvent"></param>
        public void EditEvent(Event myEvent)
        {
            string sqlQuery = "UPDATE [Event]" +
                " SET [Name] = @name, [Description] = @description, [Date] = @date, [LocationId] = @locationId, " +
                "[Price] = @price, [Time] = @time " +
                "WHERE [Id] = @id";
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, myConnection);
                sqlCommand.Parameters.AddWithValue("@name", myEvent.Name);
                sqlCommand.Parameters.AddWithValue("@description", myEvent.Description);
                sqlCommand.Parameters.AddWithValue("@date", myEvent.Date);
                sqlCommand.Parameters.AddWithValue("@locationId", myEvent.LocationId);
                sqlCommand.Parameters.AddWithValue("@price", myEvent.Price);
                sqlCommand.Parameters.AddWithValue("@time", myEvent.Time);
                sqlCommand.Parameters.AddWithValue("@id", myEvent.Id);

                myConnection.Open();

                using SqlDataReader dataReader = sqlCommand.ExecuteReader();

                myConnection.Close();

            }


        }
        /// <summary>
        /// gets all events from database
        /// </summary>
        /// <returns></returns>
        public List<Event> GetAllEvents()
        {
            string sqlQuery = "SELECT * FROM [Event]";

            List<Event> allEvents = new List<Event>(); 

            using (SqlConnection myConnection = new SqlConnection(connectionString)) 
            {
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, myConnection);

                myConnection.Open();

                using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Event myEvent = new Event();

                        myEvent.Id = int.Parse(dataReader["Id"].ToString());
                        myEvent.Name = dataReader["Name"].ToString();
                        myEvent.Date = dataReader["Date"].ToString();

                        allEvents.Add(myEvent); 
                    }

                    myConnection.Close();
                }
            }

            return allEvents;
        }
        /// <summary>
        /// get all events joined by unique user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List<Event></returns>
        public List<Event> GetAllEventsJoined(int userId)
        {
            string sqlQuery = "SELECT e.Id, e.[Name], e.[Date] FROM[Join] AS j " +
                "LEFT JOIN[Event] AS e ON e.Id = j.EventId " +
                "WHERE j.UserId = @userId";

            List<Event> allEvents = new List<Event>();

            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, myConnection);
                sqlCommand.Parameters.AddWithValue("@userId", userId);
                myConnection.Open();

                using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Event myEvent = new Event();

                        myEvent.Id = int.Parse(dataReader["Id"].ToString());
                        myEvent.Name = dataReader["Name"].ToString();
                        myEvent.Date = dataReader["Date"].ToString();

                        allEvents.Add(myEvent);
                    }

                    myConnection.Close();
                }
            }

            return allEvents;
        }
        /// <summary>
        /// get event by unique id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Event</returns>
        public Event GetEventByEventId(int id)
        {
            Event myEvent = null;

            string sqlQuery = "SELECT * FROM [Event] WHERE [Id] = @id";

            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, myConnection); 

                sqlCommand.Parameters.AddWithValue("@id", id);

                myConnection.Open(); 

                using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    if (dataReader.Read())
                    {
                        myEvent = new Event();

                        myEvent.Id = int.Parse(dataReader["Id"].ToString());
                        myEvent.Name = dataReader["Name"].ToString();
                        myEvent.Description = dataReader["Description"].ToString();
                        myEvent.Date = dataReader["Date"].ToString();
                        myEvent.Time = dataReader["Time"].ToString();
                        myEvent.Price = int.Parse(dataReader["Price"].ToString());
                        myEvent.LocationId = int.Parse(dataReader["LocationId"].ToString());

                    }

                    myConnection.Close();
                }
            }

            if (myEvent != null)
            {
                myEvent.Location = GetLocationNameById(myEvent.LocationId);
            }

            return myEvent;
        }
        /// <summary>
        /// get location name to set to event
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        private string GetLocationNameById(int locationId)
        {
            string locationName = "";
            string sqlQuery = "SELECT Name FROM [Location] WHERE Id = @locationId";

            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, myConnection);
                sqlCommand.Parameters.AddWithValue("@locationId", locationId);

                myConnection.Open();

                using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    if (dataReader.Read())
                    {
                        locationName = dataReader["Name"].ToString();
                    }

                    myConnection.Close();
                }
            }


            return locationName;
        }
        /// <summary>
        /// Get a user from database to check if exist or set loggedInUser
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
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
        /// <summary>
        /// get rolename from role to set to user
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
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
        /// <summary>
        /// add user or admin to database
        /// </summary>
        /// <param name="user"></param>
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
        /// add a new event to database
        /// </summary>
        /// <param name="newEvent"></param>
        public void AddEvent(Event newEvent)
        {
            string sqlQuery = "INSERT INTO[Event]( [Name], [Description], [Date], [LocationId], [Price], [Time])" +
                " VALUES (@name, @description, @date, @locationId, @price, @time)";
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, myConnection);
                sqlCommand.Parameters.AddWithValue("@name", newEvent.Name);
                sqlCommand.Parameters.AddWithValue("@description", newEvent.Description);
                sqlCommand.Parameters.AddWithValue("@date", newEvent.Date);
                sqlCommand.Parameters.AddWithValue("@locationId", newEvent.LocationId);
                sqlCommand.Parameters.AddWithValue("@price", newEvent.Price);
                sqlCommand.Parameters.AddWithValue("@time", newEvent.Time);
                
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
        /// didnt complete this function but leave it for version 1.1...
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        public void EditPosts(int id, string text)
        {
            string sqlQuery = "UPDATE [Post] SET [Body] = @Text WHERE Id = @Id";
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, myConnection);
                sqlCommand.Parameters.AddWithValue("@Id", id);
                sqlCommand.Parameters.AddWithValue("@Text", text);
                myConnection.Open();
                using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Post post = new Post();
                        post.Id = int.Parse(dataReader["Id"].ToString());
                        post.Body = dataReader["Body"].ToString();
                    }
                }
            }
        }
        /// <summary>
        /// get all post connected to an event
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public List<Post> GetPostsByEventId(int eventId)
        {
            string sqlQuery = "SELECT p.[Id], p.[Body], u.Username, u.RoleId, p.[UserId] FROM[POST] AS p " +
                "LEFT JOIN[User] AS u ON p.[UserId] = u.[Id] " +
                "WHERE p.[EventId] = @eventId";

            int RoleId;

           List<Post> posts = new List<Post>();
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, myConnection);
                sqlCommand.Parameters.AddWithValue("@eventId", eventId);
                myConnection.Open();

                using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Post post = new Post();

                        post.Id = int.Parse(dataReader["Id"].ToString());
                        post.UserId = int.Parse(dataReader["UserId"].ToString());
                        //post.EventId = int.Parse(dataReader["EventId"].ToString());
                        post.Body = dataReader["Body"].ToString();
                        post.UserName = dataReader["Username"].ToString();
                        RoleId = int.Parse(dataReader["RoleId"].ToString());

                        if (RoleId == 1)
                        {
                            post.isAdmin = true; //if post created by admin
                            post.UserName = "Admin";
                        }

                        posts.Add(post);
                    }
                }
            }

            posts.Reverse();

            return posts;
        }

        /// <summary>
        /// Add post to database
        /// </summary>
        /// <param name="post"></param>        
        public void AddPost(Post post) 
        {
            string sqlQuery = "INSERT INTO[Post](Body, UserId, EventId) VALUES (@body, @userid, @eventid)";
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, myConnection);
                sqlCommand.Parameters.AddWithValue("@body", post.Body);
                sqlCommand.Parameters.AddWithValue("@userid", post.UserId);
                sqlCommand.Parameters.AddWithValue("@eventid", post.EventId);

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

       
        //
        //Get all events the user has not joined
        public List<Event> GetAvailableEvents(int userId)
        {
            string sqlQuery = "select [Name], Id , [DATE] from [Event] where [Event].Id not in (select [Event].Id from [User] join [Join] on [User].Id = [Join].UserId join [Event] on [Join].EventId = [Event].Id WHERE [User].Id = @userId)";

            List<Event> eventName = new List<Event>();

            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, myConnection);
                sqlCommand.Parameters.AddWithValue("@userId", userId);

                myConnection.Open();

                using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Event currentEvents = new Event();
                        currentEvents.Id = int.Parse(dataReader["Id"].ToString());
                        currentEvents.Name = dataReader["Name"].ToString();
                        currentEvents.Date = dataReader["Date"].ToString();
                        eventName.Add(currentEvents);
                    }

                    myConnection.Close();
                }
            }

            return eventName;

        }
        //
        //Join an event
        public void JoinEvent(int userId, int userChoice)
        {
            string sqlQuery = "INSERT INTO [Join] (EventId, UserId) VALUES (@eventid, @userId)";
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, myConnection);
                sqlCommand.Parameters.AddWithValue("@eventid", userChoice);
                sqlCommand.Parameters.AddWithValue("@userId", userId);

                myConnection.Open();

                using SqlDataReader dataReader = sqlCommand.ExecuteReader();

                myConnection.Close();
            }
        }
        
       
    }
}
