using System;
using System.Collections.Generic;
using System.Text;

namespace GreenEvent
{
    public class AdminPost
    {
        public int Id;
        public int UserId;
        public string Body;
        public int EventId;



        static public AdminPost CreateAdminPost(string newPostBody, int newPostUserId)
        {
            AdminPost newAdminPost = new AdminPost();

            newAdminPost.Body = newPostBody;
            newAdminPost.UserId = newPostUserId;
            newAdminPost.EventId = 1/*newPostEventId*/;



            DataBase db = new DataBase();

            db.AddAdminPost(newAdminPost);

            newAdminPost = db.GetAdmininPostByAdminPostId(id:1);


            return newAdminPost;
        }


    }

   
    


}
