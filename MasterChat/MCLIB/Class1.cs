﻿namespace MCLIB
{

    [Serializable]
    public class IPaket : EventArgs
    {
        public enum PAKETTYPE {PLUGINSORGU,ODABAGLAN,ODAKUR ,CEVAP,MESAJ,GETPARTICIPANTS}
        public bool cevap;
        public PAKETTYPE type;
        public string detay, JOINID, msj,roompass,nick;
        public int ARGB;
        public List<Plugin> pluginler = new List<Plugin>();
    }
    [Serializable]
    public class Plugin
    {
        public string name,desc;
        public bool essential=false;
    }
  
}