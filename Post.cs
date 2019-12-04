using System;
using System.Collections.Generic;
using System.Text;

namespace GreenEvent
{
    class Post
    {
        public int Id;
        public int UserId;
        public int EventId;
        public string Body;
        public string UserName;
        public bool isAdmin = false;

        static DataBase database = new DataBase();

        public static void ShowPosts(Event myEvent, int userId)
        {

            bool isRunning = true;

            while (isRunning)
            {
                var posts = database.GetPostsByEventId(myEvent.Id);

                Console.ForegroundColor = ConsoleColor.White;

                Console.SetCursorPosition(0, 8);
                Console.WriteLine("---<<<Inlägg tillhörande event>>>---");

                if (posts.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\n  Det finns inga inlägg i detta event än\n");
                }
                else
                {
                    foreach (var post in posts)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        if (post.isAdmin) Console.ForegroundColor = ConsoleColor.Red; //If admin write post in red

                        Console.WriteLine($"{post.Body} /{post.UserName}");
                    }
                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("1) Lägg till nytt inlägg \tesc) Tillbaka");

                ConsoleKey userChoice = Console.ReadKey().Key;

                switch (userChoice)
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        AddNewPost(userId, myEvent.Id);
                        myEvent.ShowEvent(userId);
                        break;

                    case ConsoleKey.Escape:
                        isRunning = false;
                        break;

                    default:
                        Console.Clear();
                        myEvent.ShowEvent(userId);
                        break;
                }
            }


            


        }
        private static void AddNewPost(int userId, int eventId)
        {

            Post newPost = new Post();
            //string newPostBody;
            //int newPostUserId;
            //int newPostEventId;
            bool tryInput;

            do
            {
                Console.Write("Enter post: ");
                newPost.Body = Console.ReadLine();
                newPost.UserId = userId;
                newPost.EventId = eventId;

                if (newPost.Body == null)
                {
                    Console.WriteLine($"Post cant be blank");
                    Console.ReadLine();
                    tryInput = false;
                }
                else
                {
                    tryInput = true;
                }

            } while (!tryInput);

            database.AddPost(newPost);
            //AdminPost.CreateAdminPost(newPostBody, newPostUserId);

        }
    }
}
