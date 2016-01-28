using System;
using System.Collections.Generic;
using System.Threading;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Gene_Musique.BusinessLogique;

namespace Gene_Musique.Interface
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MediaPlayer mplayer;
        Boolean isPlaying;
        int iNumberGeneration = 0;
        //static Boolean Avis;
        static int tempo = 460;
        static int lengthNote = 25;
        static Random randomizer;
        GenerationMusique genMusique;
        static int numberIndividu;
        string directory = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),"generation"); 

        public MainWindow()
        {
            InitializeComponent();

            // Initialisation du lecteur
            mplayer = new MediaPlayer();
            mplayer.MediaEnded += mplayer_MediaEnded;
            isPlaying = false;
            randomizer = new Random();
            randomizer.Next();
            //Avis = false;
            if(!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Génération d'une nouvelle musique
            genMusique = new GenerationMusique();

            // récupération du nombre d'individu
            numberIndividu = genMusique.GetNumberIndividu();
            generatePopulationFile();
            

            //  Avis par défaut
            labelAvis_1.Content = Math.Round(slider_1.Value, 0).ToString();
            labelAvis_2.Content = Math.Round(slider_2.Value, 0).ToString();
            labelAvis_3.Content = Math.Round(slider_3.Value, 0).ToString();
            labelAvis_4.Content = Math.Round(slider_4.Value, 0).ToString();
            labelAvis_5.Content = Math.Round(slider_5.Value, 0).ToString();
            labelAvis_6.Content = Math.Round(slider_6.Value, 0).ToString();
            labelAvis_7.Content = Math.Round(slider_7.Value, 0).ToString();
            labelAvis_8.Content = Math.Round(slider_8.Value, 0).ToString();

            //  Tempo par défaut
            tempo = (int)Math.Round(sliderTempo.Value, 0);
            labelTempo.Content = tempo.ToString();

            //  Longueur note par défaut
            lengthNote = (int)Math.Round(sliderLengthNote.Value, 0);
            labelLengthSound.Content = lengthNote.ToString();
            
            // On s'abonne à la fermeture du programme pour pouvoir nettoyer le répertoire et les fichiers midi
            this.Closed += MainWindow_Closed;
        }

        // On efface les fichiers .mid que l'on avait créé à la fin du programme
        void MainWindow_Closed(object sender, EventArgs e)
        {
            // s'il y a un fichier en cours de lecture on l'arrête 
            // et on delete le fichier temporaire s'il existe
            
                stop_and_delete_file();
        }
        public void generatePopulationFile()
        {
            Individu[] population = this.genMusique.GetPopulation();
            int nbIndividu = population.Count();
            for(int i=0;i<nbIndividu;i++)
            {
                this.WriteMusic(population[i],i);
            }
        }

        // Lancé lorsque le fichier a fini sa lecture, pour le fermer proprement
        void mplayer_MediaEnded(object sender, EventArgs e)
        {
            Stop_music();
        }

        //  Stop lecture en cours + delete temporary midi file
        void stop_and_delete_file()
        {
            mplayer.Stop();
            mplayer.Close();
            isPlaying = false;
            deleteFile();
            
        }
        private void deleteFile()
        {
            string[] files = Directory.GetFiles(directory);
            foreach(string filename in files)
            {
                File.Delete(filename);
            }

        }

        private void ButtonRecord_Click(object sender, RoutedEventArgs e)
        {
            Button btnDeclencheur = (Button)sender;
            int indexMusicTOSave = int.Parse(btnDeclencheur.Name.Split('_')[1]);
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Piste"+ indexMusicTOSave+"midi"; // Default file name
            dlg.DefaultExt = ".midi"; // Default file extension
            dlg.Filter = "Midi song (.midi)|*.midi"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                File.Copy(directory+"\\piste"+ indexMusicTOSave + ".midi",dlg.FileName);
            }
        }

        //  Ecriture musique dans fichier temporaire
        private void WriteMusic(Individu individu,int numeroPiste)
        {
            // 1) Créer le fichier MIDI
            // a. Créer un fichier et une piste audio ainsi que les informations de tempo
            MIDISong song = new MIDISong();
            song.AddTrack("Piste1");
            song.SetTimeSignature(0, 4, 4);
            song.SetTempo(0, tempo);

            //  Récupération du tableau contenant les notes de musique
            int[] tabMusique = individu.GetNotesDeMusique();

            //  Récupération du type d'instrument dans l'individu présent dans la population
            int instrument = individu.GetTypeInstrument();
            song.SetChannelInstrument(0, 0, instrument);

            //  Ajout des notes une à une dans la piste son
            for (int i = 0; i < numberIndividu; i++)
                song.AddNote(0, 0, tabMusique[i], lengthNote);

            // d. Enregistrer le fichier .mid (lisible dans un lecteur externe par exemple)
            // on prépare le flux de sortie
            MemoryStream ms = new MemoryStream();
            song.Save(ms);
            ms.Seek(0, SeekOrigin.Begin);
            byte[] src = ms.GetBuffer();
            ms.Close();

            FileStream objWriter = File.Create (directory+"/piste"+numeroPiste+".midi");

            objWriter.Write(src, 0, src.Length);
            objWriter.Close();
            objWriter.Dispose();
            objWriter = null;
        }

        private void PlayMusic(int INumberMusic)
        {
            mplayer.Open(new Uri(directory + "/piste" + INumberMusic + ".midi", UriKind.Absolute));
            isPlaying = true;
            mplayer.Play();
        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int INumberMusic = int.Parse(button.Name.Split('_')[1]);

            //if(button.Content)
            // s'il y a un fichier en cours de lecture on l'arrête 
            if (isPlaying == true)
                Stop_music();

            //  Attente pour delete file midi done
            if (File.Exists(directory+ "/piste" + INumberMusic + ".midi"))
            {
                PlayMusic(INumberMusic);
            }
        }

        private void Stop_music()
        {
            mplayer.Stop();
            mplayer.Close();
            isPlaying = false;
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult msg = MessageBox.Show("Voulez vous sauvegarder?", "Voulez vous sauvegarder cette génération", MessageBoxButton.YesNoCancel);
            if (msg == MessageBoxResult.Yes)
            {

                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "generationMidi" + DateTime.Now.ToShortDateString(); // Default file name
                dlg.DefaultExt = ".xml"; // Default file extension
                dlg.Filter = "Xml document (.xml)|*.xml"; // Filter files by extension

                // Show save file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {
                    stop_and_delete_file();
                    this.genMusique.SavePopulation(dlg.FileName);
                    genMusique.Accouplement();
                    generatePopulationFile();
                    iNumberGeneration++;
                }
            }
            else if (msg == MessageBoxResult.No)
            {
                stop_and_delete_file();
                genMusique.Accouplement();
                generatePopulationFile();
                iNumberGeneration++;
            }
          labelNumberGeneration.Content = Math.Round((double)iNumberGeneration, 0).ToString();
        }

        private void sliderAvis_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            if(slider == slider_1)
                labelAvis_1.Content = Math.Round(slider_1.Value, 0).ToString();
            else if (slider == slider_2)
                labelAvis_2.Content = Math.Round(slider_2.Value, 0).ToString();
            else if (slider == slider_3)
                labelAvis_3.Content = Math.Round(slider_3.Value, 0).ToString();
            else if (slider == slider_4)
                labelAvis_4.Content = Math.Round(slider_4.Value, 0).ToString();
            else if (slider == slider_5)
                labelAvis_5.Content = Math.Round(slider_5.Value, 0).ToString();
            else if (slider == slider_6)
                labelAvis_6.Content = Math.Round(slider_6.Value, 0).ToString();
            else if (slider == slider_7)
                labelAvis_7.Content = Math.Round(slider_7.Value, 0).ToString();
            else if (slider == slider_8)
                labelAvis_8.Content = Math.Round(slider_8.Value, 0).ToString();
        }

        private void sliderTempo_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tempo = (int)Math.Round(sliderTempo.Value, 0);
            labelTempo.Content = tempo.ToString();
        }

        private void sliderLengthNote_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lengthNote = (int)Math.Round(sliderLengthNote.Value, 0);
            labelLengthSound.Content = lengthNote.ToString();
        }

        private void open_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = directory;
            dlg.Filter = "";
            Nullable<bool> isClose = dlg.ShowDialog();
            if(isClose==true)
            {
                this.genMusique.LoadPopulation(dlg.FileName);
            }
        }
        private void save_as_Click(object sender, RoutedEventArgs e)
        {
            SavePopulationXml();
        }
        private void SavePopulationXml()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "generationMidi" + DateTime.Now.ToShortDateString(); // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "Xml document (.xml)|*.xml"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
                this.genMusique.SavePopulation(dlg.FileName);
        }

        private void btn_Avis_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if(button == btn_plus_1)
            {
                int value_avis = Convert.ToInt32(labelAvis_1.Content);
                if (value_avis < 10)
                {
                    value_avis++;
                    labelAvis_1.Content = value_avis.ToString();
                    slider_1.Value = (double)value_avis;
                }
            }
            else if (button == btn_minus_1)
            {
                int value_avis = Convert.ToInt32(labelAvis_1.Content);
                if (value_avis > 0)
                {
                    value_avis--;
                    labelAvis_1.Content = value_avis.ToString();
                    slider_1.Value = (double)value_avis;
                }
            }
            else if (button == btn_plus_2)
            {
                int value_avis = Convert.ToInt32(labelAvis_2.Content);
                if (value_avis < 10)
                {
                    value_avis++;
                    labelAvis_2.Content = value_avis.ToString();
                    slider_2.Value = (double)value_avis;
                }
            }
            else if (button == btn_minus_2)
            {
                int value_avis = Convert.ToInt32(labelAvis_2.Content);
                if (value_avis > 0)
                {
                    value_avis--;
                    labelAvis_2.Content = value_avis.ToString();
                    slider_2.Value = (double)value_avis;
                }
            }
            else if (button == btn_plus_3)
            {
                int value_avis = Convert.ToInt32(labelAvis_3.Content);
                if (value_avis < 10)
                {
                    value_avis++;
                    labelAvis_3.Content = value_avis.ToString();
                    slider_3.Value = (double)value_avis;
                }
            }
            else if (button == btn_minus_3)
            {
                int value_avis = Convert.ToInt32(labelAvis_3.Content);
                if (value_avis > 0)
                {
                    value_avis--;
                    labelAvis_3.Content = value_avis.ToString();
                    slider_3.Value = (double)value_avis;
                }
            }
            else if (button == btn_plus_4)
            {
                int value_avis = Convert.ToInt32(labelAvis_4.Content);
                if (value_avis < 10)
                {
                    value_avis++;
                    labelAvis_4.Content = value_avis.ToString();
                    slider_4.Value = (double)value_avis;
                }
            }
            else if (button == btn_minus_4)
            {
                int value_avis = Convert.ToInt32(labelAvis_4.Content);
                if (value_avis > 0)
                {
                    value_avis--;
                    labelAvis_4.Content = value_avis.ToString();
                    slider_4.Value = (double)value_avis;
                }
            }
            else if (button == btn_plus_5)
            {
                int value_avis = Convert.ToInt32(labelAvis_5.Content);
                if (value_avis < 10)
                {
                    value_avis++;
                    labelAvis_5.Content = value_avis.ToString();
                    slider_5.Value = (double)value_avis;
                }
            }
            else if (button == btn_minus_5)
            {
                int value_avis = Convert.ToInt32(labelAvis_5.Content);
                if (value_avis > 0)
                {
                    value_avis--;
                    labelAvis_5.Content = value_avis.ToString();
                    slider_5.Value = (double)value_avis;
                }
            }
            else if (button == btn_plus_6)
            {
                int value_avis = Convert.ToInt32(labelAvis_6.Content);
                if (value_avis < 10)
                {
                    value_avis++;
                    labelAvis_6.Content = value_avis.ToString();
                    slider_6.Value = (double)value_avis;
                }
            }
            else if (button == btn_minus_6)
            {
                int value_avis = Convert.ToInt32(labelAvis_6.Content);
                if (value_avis > 0)
                {
                    value_avis--;
                    labelAvis_6.Content = value_avis.ToString();
                    slider_6.Value = (double)value_avis;
                }
            }
            else if (button == btn_plus_7)
            {
                int value_avis = Convert.ToInt32(labelAvis_7.Content);
                if (value_avis < 10)
                {
                    value_avis++;
                    labelAvis_7.Content = value_avis.ToString();
                    slider_7.Value = (double)value_avis;
                }
            }
            else if (button == btn_minus_7)
            {
                int value_avis = Convert.ToInt32(labelAvis_7.Content);
                if (value_avis > 0)
                {
                    value_avis--;
                    labelAvis_7.Content = value_avis.ToString();
                    slider_7.Value = (double)value_avis;
                }
            }
            else if (button == btn_plus_8)
            {
                int value_avis = Convert.ToInt32(labelAvis_8.Content);
                if (value_avis < 10)
                {
                    value_avis++;
                    labelAvis_8.Content = value_avis.ToString();
                    slider_8.Value = (double)value_avis;
                }
            }
            else if (button == btn_minus_8)
            {
                int value_avis = Convert.ToInt32(labelAvis_8.Content);
                if (value_avis > 0)
                {
                    value_avis--;
                    labelAvis_8.Content = value_avis.ToString();
                    slider_8.Value = (double)value_avis;
                }
            }
        }

        private void btn_config_Click(object sender, RoutedEventArgs e)
        {
            int debIntervalle = 0;
            int finIntervalle = 0;
            int tauxMutation = 0;
            config window = new config(ref debIntervalle,ref finIntervalle, ref tauxMutation);
            window.ShowDialog();
        }
    }
}
