using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Gene_Musique.BusinessLogique
{
    [Serializable]
    public class Individu
    {
        private string _nomChanson;
        private string _auteur;
        private int[] _notesMusique;
        private int _notation;
        private int _typeInstrument;
        [XmlAttribute("auteur")]
        public string auteur { get { return this._auteur; } set { this._auteur = value; } }
        [XmlAttribute("nomChanson")]
        public string nomChanson { get { return this._nomChanson; } set { this._nomChanson = value; } }
        [XmlAttribute("notesMusique")]
        public int[] notesMusique { get { return this._notesMusique; } set { this._notesMusique = value; } }
        [XmlAttribute("typeInstrument")]
        public int typeInstrument { get { return this._typeInstrument; } set { this._typeInstrument = value; } }
        [XmlAttribute("notation")]
        public int notation { get { return this._notation; }set { this._notation = value; } }
        private int NumberNote = 16;
        static Random randomizerNote = new Random();

        /// <summary>
        /// Constructeur permettant de générer aléatioirement un individu avec ses notes et son type
        /// </summary>
        /// <param name="intervalNote"></param>
        /// <param name="intervalInstrument"></param>
        public Individu(int[] intervalNote, int[] intervalInstrument)
        {
            this.getNomAuteurETChanson();
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
        public Individu()
        {
            this.notation = 5;
        }

        private Individu(int[] noteDeMusique, int typeInstrument)
        {
            this.getNomAuteurETChanson();
            this.notesMusique = noteDeMusique;
            this.notation = 5;
            this.typeInstrument = typeInstrument;
        }
        /// <summary>
        /// La vie est plein de mystère, un parent peut donner un enfant qui ressemblera légerement à son père-mère
        /// </summary>
        /// <returns></returns>
        public Individu GetEnfantMuter(int mutation)
        {
            randomizerNote = new Random();
            int[] notesDeMusiqueMuter = new int[NumberNote];
            int txMutation;

            for (int i = 0; i < NumberNote; i++)
            {
                txMutation = randomizerNote.Next(-mutation, mutation);
                notesDeMusiqueMuter[i] = this.notesMusique[i] + txMutation;
            }
            return new Individu(notesDeMusiqueMuter, this.typeInstrument);
        }
        /// <summary>
        /// Deux parent qui s'aiment donnent un enfant qui aura des propriété de ses deux parents selon le taux de fusion et la parent dominant
        /// </summary>
        /// <param name="secondParent"></param>
        /// <param name="tauxFusion"></param>
        /// <returns></returns>
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
        public void getNomAuteurETChanson()
        {
            string[][] aAuteurETChanson = AuteurETChansonFixture.GetFixture();
            int indexAleatoire = randomizerNote.Next(0, aAuteurETChanson.Length-1);
            int indexChanson = randomizerNote.Next(1, aAuteurETChanson[indexAleatoire].Length-1);
            this.auteur = aAuteurETChanson[indexAleatoire][0];
            this.nomChanson = aAuteurETChanson[indexAleatoire][indexChanson];
        }
    }
}
