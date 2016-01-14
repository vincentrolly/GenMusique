using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// FIRST COMMIT

namespace Gene_Musique.BusinessLogique
{
    public class GenerationMusique
    {
        public const int NOMBRE_INDIVIDU = 10;
        private int[] intervalNote;
        private Individu[] population;
        private double crossover;
        private double mutation;
        private static Random randomizer;

        public GenerationMusique()
        {
            this.population = new Individu[10];
            generationInitial();
        }

        public void generationInitial()
        {
            int[] intervalNote = new int[2] { 30, 100 };
            int[] intervalInstrument = new int[2] {0, 127};

            for (int i=0;i<NOMBRE_INDIVIDU;i++)
            {
                population[i] = new Individu(intervalNote, intervalInstrument);
            }
        }

        public void Accouplement()
        {

        }

        public Individu [] GetPopulation()
        {
            return this.population;
        }
    }
} 
 