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
        public int TauxMutation = 10;

        public config(ref int debIntervalle, ref int finIntervalle, ref int tauxMutation)
        {
            InitializeComponent();
            this.debIntervalle = debIntervalle;
            this.finIntervalle = finIntervalle;
            this.TauxMutation = tauxMutation;
            textBox_BeginIntervalInstrument.Text = debIntervalle.ToString();
            textBox_EndIntervalInstrument.Text = finIntervalle.ToString();
            textBox_TauxMutation.Text = tauxMutation.ToString();
        }

        private void button_save_Click(object sender, RoutedEventArgs e)
        {
            int debutInter = 32;
            int finInter = 96;
            int tauxMutation = 10;

            if(int.TryParse(this.textBox_BeginIntervalInstrument.Text, out debutInter)
             &&int.TryParse(this.textBox_EndIntervalInstrument.Text, out finInter)
             &&int.TryParse(this.textBox_TauxMutation.Text, out tauxMutation))
            {
                //int[] intervalleInstrument = new int[] { debutInter, finInter, tauxMutation};
                if(0 <= debutInter && debutInter <= 127)
                    this.debIntervalle = debutInter;

                if (0 <= finInter && finInter <= 127)
                    this.finIntervalle = finInter;

                if(2 <= TauxMutation && tauxMutation > 20)
                    this.TauxMutation = tauxMutation;
            }

            this.Close();
        }

        private void button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
