using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace LaVida.Services
{
    public static  class FirebaseDB
    {
        public static FirebaseClient firebaseClient;

        public static void Connect()
        {
            Console.WriteLine("Try to connect to RealtimeDB...");

            try
            {
                firebaseClient = new FirebaseClient("https://lavida-b6aca-default-rtdb.europe-west1.firebasedatabase.app/");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Console.WriteLine("...Connection established!");
        }
    }
}
