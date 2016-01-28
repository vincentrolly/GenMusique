using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Gene_Musique.Interface
{
    /// <summary>
    /// Logique d'interaction pour config.xaml
    /// </summary>
    public partial class config : Window
    {
        public int debIntervalle = 20;
        public int finIntervalle = 160;
        public config(ref int debIntervalle,ref int finIntervalle)
        {
            InitializeComponent();
            this.debIntervalle = debIntervalle;
            this.finIntervalle = finIntervalle;
        }

        private void button_save_Click(object sender, RoutedEventArgs e)
        {
            int debutInter = 20;
            int finInter = 160;
            if(int.TryParse(this.textBlock_debintinst.Text, out debutInter)&&int.TryParse(this.textBlock_finintinst.Text,out finInter))
            {
                // Gene_Musique.Interface.Properties.Resources.debIntervalleInstr = textBlock_debintinst;
                int[] intervalleInstrument = new int[] { debutInter, finInter };
                XmlSerializer serializer =  new XmlSerializer(intervalleInstrument.GetType());
                using (StreamWriter writer = new StreamWriter(Gene_Musique.Interface.Properties.Resources.config_xml))
                {
                    serializer.Serialize(writer, intervalleInstrument);
                }
                this.debIntervalle = debutInter;
                this.finIntervalle = finInter;
                
            }
        }
    }
}
