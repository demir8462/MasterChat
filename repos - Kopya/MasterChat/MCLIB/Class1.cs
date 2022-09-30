namespace MCLIB
{

    [Serializable]
    public class IPaket : EventArgs
    {
        public enum PAKETTYPE {PLUGINSORGU,ODABAGLAN,ODAKUR ,CEVAP,MESAJ}
        public bool cevap;
        public PAKETTYPE type;
        public string detay, JOINID, msj,roompass;
        public int ARGB;
    }
  
}