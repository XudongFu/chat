using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using chat = WindowsFormsApplication2.contract;
using System.Collections.Concurrent;
using WindowsFormsApplication2.dataEntry;
namespace WindowsFormsApplication2.core
{
    class manager
    {
        Thread receiveMessage;
        Thread solveMessages;
        

        ConcurrentQueue<chat::message> messages = new ConcurrentQueue<chat.message>();

        ConcurrentDictionary<uint, user> onlineUser = new ConcurrentDictionary<uint, user>();

        ConcurrentDictionary<uint, group> groups = new ConcurrentDictionary<uint, group>();

        void solveMessage(chat::message mess)
        {
            switch (mess.getAction())
            {




                default:
                    break;

            }
        }




        void mainfunction()
        {



        }



    }
}
