using System;
using System.Collections.Generic;
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

namespace Gene_Musique.Interface
{
    /// <summary>
    /// Logique d'interaction pour config.xaml
    /// </summary>
    public partial class config : Window
    {
        public config()
        {
            InitializeComponent();
        }

        private void button_save_Click(object sender, RoutedEventArgs e)
        {
            int debutInter = 20;
            int finInter = 160;
            if(int.TryParse(this.textBlock_debintinst.Text, out debutInter)&&int.TryParse(this.textBlock_finintinst.Text,out finInter))
            {
               // Gene_Musique.Interface.Properties.Resources.debIntervalleInstr = textBlock_debintinst;
            }
        }
    }
}
