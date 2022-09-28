﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCLIB;
namespace MasterChatServer
{
    public class ChatRoom
    {
        public int ID;
        public string JOINID;
        public List<SClient> participants = new List<SClient>();
        public int participantcount;
        public static object o = new object(),mesajo = new object();
        public ChatRoom()
        {
            lock(o)
            {
                Server.ALLROOMCOUNT++;
                ID = Server.ALLROOMCOUNT;
            }
            Task.Factory.StartNew(MesageRouter);
        }
        public void MesageRouter()
        {
            while (true)
            {
                try
                {
                    foreach (SClient sender in participants)
                    {

                        try
                        {
                            foreach (SClient members in participants)
                            {
                                if (members == sender)
                                    continue;
                                lock(mesajo)
                                {
                                    foreach (Mesaj msj in sender.mesajlar)
                                    {
                                        members.mesajAt(msj.msj, msj.ARGB);
                                    }
                                    sender.mesajlar.Clear();
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            continue;
                        }
                    }
                }catch(Exception e)
                {
                    continue;
                }
            }
                
            
        }
    }
}