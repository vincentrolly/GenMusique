using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gene_Musique.BusinessLogique
{
    public class GenerateurAleatoireSingleTon
    {
        static int compteur = 0;
        private static GenerateurAleatoireSingleTon instance;
        static Random random = new Random();
        private GenerateurAleatoireSingleTon()
        {
            
        }
        public static GenerateurAleatoireSingleTon GetInstance()
        {
            if(instance==null)
            {
                instance = new GenerateurAleatoireSingleTon();
                return instance;
            }
            else
            {
                compteur++;
                return instance;
            }
        }
        public int Next()
        {
            return random.Next();
        }
        public int Next(int min,int max)
        {
            
            return random.Next(min, max);
        }
    }

    

}
