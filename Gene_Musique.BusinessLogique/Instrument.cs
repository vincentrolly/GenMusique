using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gene_Musique.BusinessLogique
{
   public class Instrument
    {
        private int _indexMidi;
        private string _libelle;
        public int indexMidi { get { return this._indexMidi; } set { this._indexMidi=value; } }
        public string libelle { get { return this._libelle; } set { this._libelle = value; } }
    }
}
