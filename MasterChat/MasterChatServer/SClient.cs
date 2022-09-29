using System;
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
                    ChatRoom room = new ChatRoom();
                    room.ID = Server.odalar.Count;
                    room.JOINID = "AA" + room.ID.ToString();
                    room.participants.Add(this);
                    room.participantcount = 1;
                    paket.type = IPaket.PAKETTYPE.CEVAP;
                    paket.cevap = true;
                    paket.JOINID = room.JOINID;
                    Server.odalar.Add(room);
                    this.room = room;
                    Form1.writeConsole("CEVAP YOLLANIYOR JOINID:" + paket.JOINID);
                }
                sendPaket(paket);
            }
            else if (Gpaket.type == IPaket.PAKETTYPE.ODABAGLAN)
            {
                bool x = false;
                foreach  (ChatRoom oda in Server.odalar)
                {
                    if(oda.JOINID == Gpaket.JOINID)
                    {
                        oda.participants.Add(this);
                        lock(Server.odao)
                        {
                            oda.participantcount++;
                            
                        }
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
            lock(Server.odao)
            {
                s.Close();
                stream.Close();
                Server.REMOVEclients.Add(this);
                Server.removeClientControl(this);
                room.participantcount--;
                room.participants.Remove(this);
                Form1.writeConsole("Client öldü");
            }
        }
    }
}
