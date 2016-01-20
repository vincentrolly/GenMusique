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
         int iNumber = 0;
        static Boolean Avis;
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
            Avis = false;
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
            labelAvis.Content = Math.Round(sliderAvis.Value, 0).ToString();
            
            //  Tempo par défaut
            tempo = (int)Math.Round(sliderTempo.Value, 0);
            labelTempo.Content = tempo.ToString();

            //  Longueur note par défaut
            lengthNote = (int)Math.Round(sliderLengthNote.Value, 0);
            labelLengthSound.Content = lengthNote.ToString();

            //  Affichage du numéro de la chanson dans une textbox
            labelNumberSong.Content = Math.Round((double)iNumber, 0).ToString();
            
            // On s'abonne à la fermeture du programme pour pouvoir nettoyer le répertoire et les fichiers midi
            this.Closed += MainWindow_Closed;
        }

        // On efface les fichiers .mid que l'on avait créé à la fin du programme
        void MainWindow_Closed(object sender, EventArgs e)
        {
            // s'il y a un fichier en cours de lecture on l'arrête 
            // et on delete le fichier temporaire s'il existe
            if (isPlaying)
                stop_and_delete_file();
        }
        public void generatePopulationFile()
        {
            Individu[] population = this.genMusique.GetPopulation();
            int nbIndividu = population.Count();
            for(int i=0;i<nbIndividu;i++)
            {
                this.WriteMusic(population[i]);
            }
        }

        // Lancé lorsque le fichier a fini sa lecture, pour le fermer proprement
        void mplayer_MediaEnded(object sender, EventArgs e)
        {
            stop_and_delete_file();
        }

        //  Stop lecture en cours + delete temporary midi file
        void stop_and_delete_file()
        {
            mplayer.Stop();
            mplayer.Close();
            isPlaying = false;

            if (File.Exists(directory + "piste" + this.iNumber + ".midi"))
                File.Delete(directory + "piste" + this.iNumber + ".midi");
        }

        private void ButtonRecord_Click(object sender, RoutedEventArgs e)
        {

        }

        //  Ecriture musique dans fichier temporaire
        private void WriteMusic(Individu individu)
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

            FileStream objWriter = File.Create (directory+"piste"+this.iNumber+".midi");

            objWriter.Write(src, 0, src.Length);
            objWriter.Close();
            objWriter.Dispose();
            objWriter = null;
        }

        private void PlayMusic()
        {
            mplayer.Open(new Uri(directory + "piste" + this.iNumber + ".midi", UriKind.Relative));
            isPlaying = true;
            mplayer.Play();
        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            // s'il y a un fichier en cours de lecture on l'arrête 
            if (isPlaying == true)
                stop_and_delete_file();

            //  Attente pour delete file midi done
            while(File.Exists(directory)) { }

            
            PlayMusic();
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            if(Avis == true)
            {
                if (iNumber == numberIndividu)
                {
                    genMusique.Accouplement();
                    generatePopulationFile();
                    iNumber = 0;
                }
                else iNumber++;
                Avis = false;
            }
          labelNumberSong.Content = Math.Round((double)iNumber, 0).ToString();
        }
/*
        private void sauverFichier(int indice)
        {
            Individu[] toto = genMusique.GetPopulation();

            saveFileDialog.ShowDialog();
            string nomFichier = saveFileDialog.FileName;
            Debug.WriteLine("nomFichier : " + nomFichier);  //This is the instrument value
            lm.enregistreFichier(unIndividu, nomFichier);
            saveFileDialog.Dispose();
            saveFileDialog.Reset();
        }
        */
        private void sliderAvis_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Avis = true;
            labelAvis.Content = Math.Round(sliderAvis.Value, 0).ToString();
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
    }
}
