﻿using System;
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

        /// <summary>
        /// Show posts on event
        /// </summary>
        /// <param name="myEvent"></param>
        /// <param name="userId"></param>
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
                bool userIsJoined = database.IsUserJoined(myEvent.Id, userId);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("1) Lägg till nytt inlägg \t");
                if (!userIsJoined)
                {
                    Console.Write("2) Joina event \t");
                }
                Console.Write("esc) Tillbaka");


                ConsoleKey userChoice = Console.ReadKey().Key;

                switch (userChoice)
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        AddNewPost(userId, myEvent.Id);
                        myEvent.ShowEvent(userId);
                        isRunning = false;
                        break;
                    case ConsoleKey.D2:
                        Console.Clear();
                        if (!userIsJoined)
                        {
                            database.JoinEvent(userId, myEvent.Id);
                        }
                        myEvent.ShowEvent(userId);
                        isRunning = false;
                        break;
                    case ConsoleKey.Escape:
                       
                        isRunning = false;
                        //myEvent.ShowEvent(userId);
                        break;

                    default:
                        Console.Clear();
                        myEvent.ShowEvent(userId);
                        break;
                }
            }


            


        }

        /// <summary>
        /// Add new post to event.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="eventId"></param>
        private static void AddNewPost(int userId, int eventId)
        {

            Post newPost = new Post();
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
        }
    }
}
