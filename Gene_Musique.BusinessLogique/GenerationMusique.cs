using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

// FIRST COMMIT

namespace Gene_Musique.BusinessLogique
{
    public class GenerationMusique
    {
        public const int NOMBRE_INDIVIDU = 10;
        public const int TAUX_FUSION = 10;
        //private int[] intervalNote;
        private Individu[] population;
        private const double TXCROSSOVER = 0.6;
        //private double crossover;
        private int _mutation;
        private int _deb_intervalle_instrument;
        private int _fin_intervalle_instrument;
        public int debutIntervalleInstrument { get { return this._deb_intervalle_instrument; } set { this._deb_intervalle_instrument = value; } }
        public int finIntervalleInstrument { get { return _fin_intervalle_instrument; } set { this._fin_intervalle_instrument = value; } }
        public int mutation { get { return this._mutation; } set { this._mutation = value; } }
        private static Random randomizer = new Random();

        public GenerationMusique()
        {
            // LoadConfigurationEnvironnement();
            //On initialise un tableau avec 10 emplacement
            this.population = new Individu[NOMBRE_INDIVIDU];
                this.debutIntervalleInstrument = 1;
            this.finIntervalleInstrument = 127;
            //On génère la première génèration
            generationInitial();
        }

        public int GetNumberIndividu()
        {
            return NOMBRE_INDIVIDU;
        }

        public Individu[] GetPopulation()
        {
            return population;
        }

        public void generationInitial()
        {
            int[] intervalNote = new int[2] { 30, 100 };
            int[] intervalInstrument = new int[2] { this.debutIntervalleInstrument, this.finIntervalleInstrument };

            for (int i = 0; i < NOMBRE_INDIVIDU; i++)
            {
                population[i] = new Individu(intervalNote, intervalInstrument);
            }
        }

        public void Accouplement()
        {
            Individu[] nouvellePopulation = new Individu[NOMBRE_INDIVIDU];

            for (int i = 0; i < NOMBRE_INDIVIDU; i++)
            {
                Individu[] individuSelectionne = SelectionCouple();
                Individu enfant = null;
                int[] intervalleInstrument = new int[] { this.debutIntervalleInstrument, this.finIntervalleInstrument };

                if (individuSelectionne.Length == 1)
                     enfant = individuSelectionne[0].GetEnfantMuter(mutation);
                else
                    enfant = individuSelectionne[0].GetEnfantNaturel(individuSelectionne[1],TAUX_FUSION);

                nouvellePopulation[i] = enfant;
            }
            population = nouvellePopulation;
        }

        private Individu[] SelectionCouple()
        {    
            Individu parent1 = population[randomizer.Next(0, NOMBRE_INDIVIDU - 1)];
            Individu parent2 = population[randomizer.Next(0, NOMBRE_INDIVIDU - 1)];
            double crossover = randomizer.Next(0, 100);
            crossover /= 100;

            if (crossover < TXCROSSOVER)
                return new Individu[]{ parent1, parent2 };
            else
            {
                if (parent1.getNotation() >= parent2.getNotation())
                    return new Individu[] { parent1 };
                else
                    return new Individu[] { parent2 };
            }
        }

        public void SavePopulation(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(this.population.GetType());
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, this.population);
            }
           
        }
        public void LoadPopulation(string filename)
        {
            var xr = new XmlTextReader(filename);
            var xs = new XmlSerializer(typeof(Individu[]));
            this.population = (Individu[])xs.Deserialize(xr);
        }
        /// <summary>
        /// Méthode permettant de charger le fichier de configuration
        /// </summary>
        /// <param name="filename"></param>
        public void LoadConfigurationEnvironnement()
        {
            string directory = Path.Combine("C:\\",System.Environment.SpecialFolder.CommonProgramFilesX86.ToString(),"generationmusique");
            if(!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string path = directory + "\\config.xml";
            if (File.Exists(path))
            {

                var xr = new XmlTextReader(path);
                var xs = new XmlSerializer(typeof(int[]));
                int[] config = (int[])xs.Deserialize(xr);


                this.debutIntervalleInstrument = config[0];
                this.finIntervalleInstrument = config[1];
                this.mutation = config[2];
            }
            else
            {
                int[] config = new int[3] { 1, 127, 3 };
                XmlWriter.Create(File.Create(path)).Close();
                XmlSerializer serializer = new XmlSerializer(config.GetType());

                using (StreamWriter writer = new StreamWriter(path))
                {
                    serializer.Serialize(writer, config);
                }

            }
            
            
           
                
            
            
        }
        public void SaveMutationEnvironnement(int[] configToSave)
        {
            this.debutIntervalleInstrument = configToSave[0];
            this.finIntervalleInstrument = configToSave[1];
            this.mutation = configToSave[2];
            string path = Environment.SpecialFolder.CommonDocuments + "\\config.xml";
            XmlWriter.Create(File.Create(path)).Close();

            XmlSerializer serializer = new XmlSerializer(configToSave.GetType());
            using (StreamWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, configToSave);
            }
        }
        public static string GetEmbeddedResourceNames()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            List<string> resourceNames = new List<string>(assembly.GetManifestResourceNames());

            string nom = "config";
            string resourcePath = resourceNames.FirstOrDefault(r => r.Contains(nom));
            return resourcePath;

        }
        public static Stream GetEmbeddedResourceStream(string resourceName)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
        }


    }
} 
 