using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gene_Musique.BusinessLogique
{
    public class GenerateurAleatoireSingleTon
    {
        private string[] aChansons;
        public string [] GetFixture()
        {
            string[] fixture =  new string[] { "Dark side of the moon", "Hurricane","Welcome to the machine","Do you wanna know",
        "Carry on my wayard"};
            return fixture;
        }
    }

    

}
