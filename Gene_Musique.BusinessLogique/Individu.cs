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
        private int NumberNote = 16;
        static Random randomizerNote = new Random();

        /// <summary>
        /// Constructeur permettant de générer aléatioirement un individu avec ses notes et son type
        /// </summary>
        /// <param name="intervalNote"></param>
        /// <param name="intervalInstrument"></param>
        public Individu(int[] intervalNote, int[] intervalInstrument)
        {
            this.notesMusique = new int[NumberNote]; ;
            for (int i = 0; i < NumberNote; i++)
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

        public void setNotation(int note)
        {
            this.notation = note;
        }

        public int getNotation()
        {
            return this.notation;
        }

        public int GetTypeInstrument()
        {
            return this.typeInstrument;
        }

        public string ToString()
        {
            return this.notesMusique.ToString();
        }

        private Individu(int[] noteDeMusique, int typeInstrument)
        {
            this.notesMusique = noteDeMusique;
            this.notation = 5;
            this.typeInstrument = typeInstrument;
        }

        public Individu GetEnfantMuter()
        {
            randomizerNote = new Random();
            int[] notesDeMusiqueMuter = new int[NumberNote];
            int txMutation;

            for (int i = 0; i < NumberNote; i++)
            {
                txMutation = randomizerNote.Next(-5, 5);
                notesDeMusiqueMuter[i] = this.notesMusique[i] + txMutation;
            }
            return new Individu(notesDeMusiqueMuter, this.typeInstrument);
        }

        public Individu GetEnfantNaturel(Individu secondParent, int tauxFusion)
        {
            randomizerNote = new Random();
            int[] notesDeMusiqueEnfant = new int[NumberNote];

            if (this.notation >= secondParent.getNotation())
            {
                notesDeMusiqueEnfant = FusionNotesDeMusique(this.notesMusique, secondParent.GetNotesDeMusique(), tauxFusion);
                return new Individu(notesDeMusiqueEnfant, this.typeInstrument);
            }
            else
            {
                notesDeMusiqueEnfant = FusionNotesDeMusique(secondParent.GetNotesDeMusique(), this.notesMusique, tauxFusion);
                return new Individu(notesDeMusiqueEnfant, secondParent.GetTypeInstrument());
            }
        }

        private int[] FusionNotesDeMusique(int[] noteParentsPlusFort, int[] noteParentFaible,int taux)
        {
            int[] notesDeMusiqueFusionner = new int[NumberNote];
            int tauxCoupure = taux > NumberNote ? NumberNote/2 : taux;

            for (int i = 0; i < NumberNote; i++)
            {
                if(i < tauxCoupure)
                    notesDeMusiqueFusionner[i] = noteParentsPlusFort[i];
                else
                    notesDeMusiqueFusionner[i] = noteParentFaible[i];
            }
            return notesDeMusiqueFusionner;
        }
    }
}
