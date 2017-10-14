using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_Navigator
{
    class Information
    {
       public  string url;
       public string word;
       public string synopsis;
       public int numIncedences;
       public long timeDuration;
<<<<<<< HEAD
        public double timeCPU;
        public Information(string url, string word, string synopsis, int numIncedences,long timeDuration,double timeCPU) {
=======
        public Information(string url, string word, string synopsis, int numIncedences,long timeDuration) {
>>>>>>> master
            this.url = url;
            this.word = word;
            this.synopsis = synopsis;
            this.numIncedences = numIncedences;
            this.timeDuration = timeDuration;
<<<<<<< HEAD
            this.timeCPU = timeCPU;
=======
>>>>>>> master
        }
    }
}
