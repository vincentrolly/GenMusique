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
        static Random rand;
        MediaPlayer mplayer;
        Boolean isPlaying;
        static int iNumber;
        static Boolean Avis;
        static int tempo = 250;
        static int lengthNote = 20;
        static Random randomizer;
        GenerationMusique genMusique;

        public MainWindow()
        {
            InitializeComponent();

            // Initialisation du lecteur
            mplayer = new MediaPlayer();
            mplayer.MediaEnded += mplayer_MediaEnded;
            isPlaying = false;
            randomizer = new Random();
            randomizer.Next();
            Avis = false;

            genMusique = new GenerationMusique();

            sliderAvis.Value = 5;
            textBoxAvis.Text = Math.Round(sliderAvis.Value, 0).ToString();
            sliderTempo.Value = 250;
            textBoxTempo.Text = Math.Round(sliderTempo.Value, 0).ToString();

            // On s'abonne à la fermeture du programme pour pouvoir nettoyer le répertoire et les fichiers midi
            this.Closed += MainWindow_Closed;
        }

        // On efface les fichiers .mid que l'on avait créé à la fin du programme
        void MainWindow_Closed(object sender, EventArgs e)
        {
            // s'il y a un fichier en cours de lecture on l'arrête 
            if (isPlaying)
            {
                mplayer.Stop();
                mplayer.Close();
                isPlaying = false;
            }
        }

        // Lancé lorsque le fichier a fini sa lecture, pour le fermer proprement
        void mplayer_MediaEnded(object sender, EventArgs e)
        {
            mplayer.Stop();
            mplayer.Close();
            isPlaying = false;
        }

        private void ButtonRecord_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WriteMusic()
        {
            // 1) Créer le fichier MIDI
            // a. Créer un fichier et une piste audio ainsi que les informations de tempo
            MIDISong song = new MIDISong();
            song.AddTrack("Piste1");
            song.SetTimeSignature(0, 4, 4);
            song.SetTempo(0, tempo);
            rand = new Random();
            int[] tabMusique = genMusique.GetPopulation()[iNumber].GetNotesDeMusique();

            int instrument = randomizer.Next(1, 129);
            song.SetChannelInstrument(0, 0, instrument);

            for (int i = 0; i < 16; i++)
                song.AddNote(0, 0, tabMusique[i], lengthNote);

            // d. Enregistrer le fichier .mid (lisible dans un lecteur externe par exemple)
            // on prépare le flux de sortie
            MemoryStream ms = new MemoryStream();
            song.Save(ms);
            ms.Seek(0, SeekOrigin.Begin);
            byte[] src = ms.GetBuffer();
            ms.Close();

            FileStream objWriter = File.Create("toto.midi");
            objWriter.Write(src, 0, src.Length);
            objWriter.Close();
            objWriter.Dispose();
            objWriter = null;
        }

        private void PlayMusic()
        {
            mplayer.Open(new Uri("toto.midi", UriKind.Relative));
            isPlaying = true;
            mplayer.Play();
        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            // s'il y a un fichier en cours de lecture on l'arrête 
            if (isPlaying)
            {
                mplayer.Stop();
                mplayer.Close();
                isPlaying = false;
            }
            WriteMusic();
            PlayMusic();
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            if(Avis == true)
            {
                if (iNumber == 9) iNumber = 0;
                else iNumber++;
                Avis = false;
            }
        }

        private void sliderAvis_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Avis = true;
            textBoxAvis.Text = Math.Round(sliderAvis.Value, 0).ToString();
        }

        private void sliderTempo_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            textBoxTempo.Text = Math.Round(sliderTempo .Value, 0).ToString();
        }
    }
}
