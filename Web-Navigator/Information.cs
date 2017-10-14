using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_Navigator
{
    /// <summary>
    /// This class save the information for each result found 
    /// </summary>
    /// <param name="url"></param>
    /// <param name="word"></param>
    /// <param name="synopsis"></param>
    /// <param name="numIncedences"></param>
    /// <param name="timeDuration"></param>
    /// <param name="timeCPU"></param>
    class Information
    {
       public  string url;
       public string word;
       public string synopsis;
       public int numIncedences;
       public long timeDuration;

        public double timeCPU;
        public Information(string url, string word, string synopsis, int numIncedences,long timeDuration,double timeCPU) {
        

            this.url = url;
            this.word = word;
            this.synopsis = synopsis;
            this.numIncedences = numIncedences;
            this.timeDuration = timeDuration;

            this.timeCPU = timeCPU;

        }
    }
}
