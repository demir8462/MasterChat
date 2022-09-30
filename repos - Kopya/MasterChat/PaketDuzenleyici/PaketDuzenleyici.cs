using MCPlugin;
namespace PaketDuzenleyici
{
    public class PaketDuzenleyici : MCPLUGIN
    {
        public EventManager manager { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public string Desc { get; set; }

        public bool Essential { get; set; }
        public PaketDuzenleyici()
        {
            manager = new EventManager();
            Name = "Paket Duzenleyici";
            Desc = "PD";
            Version = "asd"; 
        }
        public void Run()
        {
            manager.regEvent(paketAl, EventManager.EVENTTYPE.PAKETAL);
            manager.regEvent(paketYolla, EventManager.EVENTTYPE.PAKETYOLLA);
            
            
        }
        public void paketAl(object s,EventArgs e)
        {
            
        }
        public void paketYolla(object s,EventArgs e)
        {
            MessageBox.Show("sa");
            EventInfo info = (EventInfo)e;
            info.mesaj = "OYNANDI:" + info.mesaj;
        }
    }
}