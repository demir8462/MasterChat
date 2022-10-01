﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using MCLIB;
using System.Runtime.Serialization.Formatters.Binary;

namespace MasterChatServer
{
    public class Mesaj
    {
        public string msj;
        public int ARGB;
        public Mesaj(string msj, int mcolor)
        {
            this.msj = msj;
            this.ARGB = mcolor;
        }   
    }
    public class SClient
    {
        BinaryFormatter bf = new BinaryFormatter();
        NetworkStream stream;
        Socket s;
        IPaket paket= new IPaket(),Gpaket;
        ChatRoom room;
        bool oldu = false;
        public List<Mesaj> mesajlar = new List<Mesaj>(); 
        public SClient(Socket s)
        {
            stream = new NetworkStream(s);
            this.s = s;
            Task.Run(paketKontrol);
        }
        public bool sendPaket(IPaket paket)
        {
            try
            {
                bf.Serialize(stream, paket);
                return true;
            }catch(Exception e)
            {
                clientOl();
                return false;
            }
        }
        public bool getPaket()
        {
            try
            {
                Gpaket = (IPaket)bf.Deserialize(stream);
                if (Gpaket == null)
                    return false;
                return true;
            }
            catch (Exception e)
            {
                clientOl();
                return false;
            }
        }
        void paketKontrol()
        {
            while (s.Connected)
            {
                if(getPaket())
                {
                    paketIslemci();
                }
            }
            clientOl();
        }
        void paketIslemci()
        {
            if(Gpaket.type == IPaket.PAKETTYPE.ODAKUR)
            {
                lock(Server.odao)
                {
                    Form1.writeConsole("ODA KURMA ISTEGI DEGERLENDIRILIYOR..");
                    ChatRoom room = new ChatRoom(Gpaket.roompass);
                    room.ID = Server.odalar.Count;
                    room.JOINID = "AA" + room.ID.ToString();
                    room.participants.Add(this);
                    room.participantcount = 1;
                    foreach (Plugin p in Gpaket.pluginler)
                    {
                        room.plugins.Add(p);
                    }
                    paket.type = IPaket.PAKETTYPE.CEVAP;
                    paket.cevap = true;
                    paket.JOINID = room.JOINID;
                    this.room = room;
                    Server.odalar.Add(this.room);
                    
                    Form1.writeConsole("CEVAP YOLLANIYOR JOINID:" + paket.JOINID+" PASS:"+Gpaket.roompass);
                }
                sendPaket(paket);
            }
            else if (Gpaket.type == IPaket.PAKETTYPE.ODABAGLAN)
            {
                bool x = false;
                string sonp;
                lock(Server.odao)
                {
                   if(room.plugins.Count > 0 && Gpaket.pluginler.Count > 0)
                    {
                        foreach (ChatRoom oda in Server.odalar)
                        {
                            if (oda.JOINID == Gpaket.JOINID && Gpaket.roompass == oda.PASS)
                            {
                                bool sunucudaolmayanplugin = true;
                                bool kullanicidaolmaynaplugin = true;
                                foreach (Plugin Kplugin in Gpaket.pluginler)
                                {
                                    foreach (Plugin Splugin in oda.plugins)
                                    {
                                        if (Splugin.name == Kplugin.name)
                                        {
                                            sunucudaolmayanplugin = false;
                                            break;
                                        }
                                        else
                                        {
                                            sonp = Kplugin.name;
                                        }
                                    }
                                    if (sunucudaolmayanplugin)
                                        break;
                                }
                                foreach (Plugin Splugin in oda.plugins)
                                {
                                    foreach (Plugin Kplugin in Gpaket.pluginler)
                                    {
                                        if (Kplugin.name == Splugin.name)
                                        {
                                            kullanicidaolmaynaplugin = false;
                                            break;
                                        }
                                        else
                                        {
                                            sonp = Splugin.name;

                                        }
                                    }
                                    if (kullanicidaolmaynaplugin)
                                        break;

                                }
                                if (kullanicidaolmaynaplugin)
                                    paket.detay = "Kullanıcıda Eksik Plugin Var Gerekli Pluginleri Temin Et";
                                else if (sunucudaolmayanplugin)
                                    paket.detay = "Sunucuda olmayan pluginler yüklü";
                                if (!kullanicidaolmaynaplugin && !sunucudaolmayanplugin)
                                {
                                    oda.participants.Add(this);
                                    oda.participantcount++;
                                    room = oda;
                                    x = true;
                                }
                                else
                                    x = false;
                            }
                            else
                            {
                                paket.detay = "Şifre yanlış !";
                            }
                        }
                    }else if (room.plugins.Count == 0 && Gpaket.pluginler.Count == 0)
                    {
                        x = true;
                    }
                }
                paket.type = IPaket.PAKETTYPE.CEVAP;    
                paket.cevap = x;
                sendPaket(paket);
            }
            else if (Gpaket.type == IPaket.PAKETTYPE.MESAJ)
            {
                lock(ChatRoom.mesajo)
                {
                    mesajlar.Add(new Mesaj(Gpaket.msj, Gpaket.ARGB));
                }
                Form1.writeConsole(Gpaket.msj);
            }
            
        }
        public void mesajAt(string msj,int mcolor)
        {
            paket.type = IPaket.PAKETTYPE.MESAJ;
            paket.msj = msj;
            paket.ARGB = mcolor;
            sendPaket(paket);
        }
        void clientOl()
        {
            if (oldu)
                return;
            oldu = true;
            s.Close();
            stream.Close();
            lock (Server.odao)
            {
                room.participantcount--;
                room.participants.Remove(this);

                Form1.writeConsole("Client öldü");

            }
            

        }
    }
}
