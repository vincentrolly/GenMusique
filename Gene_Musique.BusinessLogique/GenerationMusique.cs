using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private int[] intervalNote;
        private Individu[] population;
        private const double TXCROSSOVER = 0.6;
        private double crossover;
        private double mutation;
        private static Random randomizer = new Random();
        public GenerationMusique()
        {
            this.population = new Individu[10];
           // randomizer = new Random();

            generationInitial();
        }

        public Individu[] GetPopulation()
        {
            return population;
        }

        public void generationInitial()
        {
            int[] intervalNote = new int[2] { 30, 100 };
            int[] intervalInstrument = new int[2] { 0, 127 };
            for (int i = 0; i < NOMBRE_INDIVIDU; i++)
            {
                population[i] = new Individu(intervalNote, intervalInstrument);
            }
        }
        public void Accouplement()
        {
            Individu[] nouvellePopulation = new Individu[NOMBRE_INDIVIDU];
            for (int i = 0; i <= NOMBRE_INDIVIDU; i++)
            {
                Individu[] individuSelectionne = SelectionCouple();

                Individu enfant = null;
                if (individuSelectionne.Length == 1)
                {
                     enfant = individuSelectionne[0].GetEnfantMuter();
                }
                else
                {
                    enfant = individuSelectionne[0].GetEnfantNaturel(individuSelectionne[1],TAUX_FUSION);
                }
                nouvellePopulation[i] = enfant;
            }
            population = nouvellePopulation;
        }
        private Individu[] SelectionCouple()
        {
            
            Individu parent1 = population[randomizer.Next(0, NOMBRE_INDIVIDU - 1)];
            Individu parent2 = population[randomizer.Next(0, NOMBRE_INDIVIDU - 1)];
            double crossover = randomizer.Next(0,100)/100;
            if (crossover < TXCROSSOVER)
            {
                return new Individu[]{ parent1, parent2 };
            }
            else
            {
                if (parent1.getNotation() >= parent2.getNotation())
                {
                    return new Individu[] { parent1};
                }
                else
                {
                    return new Individu[] { parent2 };
                }
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
        public void loadPopulation(string filename)
        {
             public void loadPopulation(string filename)
        {
            var xr = new XmlTextReader(filename);
            var xs = new XmlSerializer(typeof(Individu[]));
            this.population = (Individu[])xs.Deserialize(xr);
        }
        }
    }

} 
 