using System;

namespace GreenEvent
{
    class Program
    {


        static void Main(string[] args)
        {

            //DataBase.CreateRolesForDebug();
            //DataBase.CreateUsersForDebug();

            EventSystem greenEvent = new EventSystem();

            greenEvent.Start();

            
        }
    }
}
