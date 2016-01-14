using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gene_Musique.BusinessLogique
{
    public class Individu
    {
        private int[] notesMusique;
        private int typeInstrument;
        private int notation;
        static Random randomizerNote;

        /// <summary>
        /// Constructeur permettant de générer aléatioirement un individu avec ses notes et son type
        /// </summary>
        /// <param name="intervalNote"></param>
        /// <param name="intervalInstrument"></param>
        public Individu(int [] intervalNote,int[] intervalInstrument)
        {
            this.notesMusique = new int[16]; ;
            for (int i=0;i<16;i++)
            {
                this.notesMusique[i] = randomizerNote.Next(intervalNote[0], intervalNote[1]);
            }
            this.typeInstrument = randomizerNote.Next(intervalInstrument[0], intervalInstrument[1]);
            this.notation = 5;
        }
        public int[] GetNotesDeMusique()
        {
            return this.notesMusique;
        }

    }
}
