using System;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using Web_Navigator;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Threading;

namespace MethodSearch
{
    class Methods
    {
        int incidences = 0;
        Boolean flag;
        string synopsis;
        Stopwatch timeFastMethod; // Time Parallel
        public long timeParallel;
        public long timeSequential;
        Stopwatch timeLowMethods; //Time Secuencial
        Stopwatch timeSearch; // Time all of link
        ArrayList temp = new ArrayList(); // Backup List url
        string[] listURL; // Main list url
        string[] words; // Main list to search word
        PerformanceCounter cpuCounter;

        PerformanceCounter ramCounter;
        public ArrayList resultsFoundParallel = new ArrayList(); //Save Information of Parallel
        public ArrayList resultsFoundSequential = new ArrayList();// Save Information of Secuencial
        int contProcessor; //Num of Procesor
        string line; 

        public Methods()
        {
            addListURL(); //Add list of Urls
            contProcessor = Environment.ProcessorCount; //Count Processors
            listURL = new string[temp.Count]; 
            for (int i = 0; i < temp.Count; i++)
            {
                listURL[i] = (string)temp[i];
            }
        }
        #region Methods that use parallel 
        /*
          *Case parrallel number #1
          */
        //Main method to Search word in Parallel
        public void MainSearchFast(String search) {
            string[] results = search.Split(';');
            timeFastMethod = Stopwatch.StartNew();
            ParallelOptions pOptions = new ParallelOptions();
            pOptions.MaxDegreeOfParallelism = contProcessor;
            Parallel.ForEach(results, pOptions, (i) =>
            {
                
                searchURLFast(i);

            });
            timeParallel = timeFastMethod.ElapsedMilliseconds;
            Console.WriteLine(timeFastMethod.ElapsedMilliseconds + "");
        }

        /*
        *Case Parallel number #2
        */
        //Main method to pass Url in Parallel
        public void searchURLFast(String aux)

        {


            timeSearch = Stopwatch.StartNew();
            ParallelOptions pOptions = new ParallelOptions();
            pOptions.MaxDegreeOfParallelism = contProcessor;
            Parallel.ForEach(listURL, pOptions, (i) =>
            {
                cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                ramCounter = new PerformanceCounter("Memory", "Available MBytes");
                ResponseURLFast(i, aux);
                Console.WriteLine("------------------------------------------------------------------------------\n" + "Uso de cpu: " + getCurrentCpuUsage() + " Uso de memoria " + getAvailableRAM());
            });
        }
        /*
        *Case Parallel number #2
        */
        //Method to Response URL and get html
        private void ResponseURLFast(String url, String aux)
        {
            try
            {
                // Create a request for the URL. 
                WebRequest request = WebRequest.Create(url);
                // If required by the server, set the credentials.
                // If required by the server, set the credentials.
                request.Credentials = CredentialCache.DefaultCredentials;
                // Get the response.
                WebResponse response = request.GetResponse();
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                //Call Case #3
                initSearchFast(responseFromServer, aux,url);
            }
            catch (Exception)
            {

            }
        }
        /*
         *Case parrallel number #3
         */
        //Main method to search word in the page html and save in the list corresponds
        public void initSearchFast(string page, string textToSearch, string url)
        {
            flag = true;
            incidences = 0;
            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = contProcessor;
            //Divide de page in slides
            string[] slides = deleteTag(page.Split('>'));
            Parallel.ForEach(slides, options, (i) =>
            {
                search(i, textToSearch, flag);
            });
            Console.WriteLine("Url: " + url + " incidencias: " + incidences + " word: " + textToSearch);
            if (incidences > 0)
            {
                long duration = timeSearch.ElapsedMilliseconds; //Time duration 
                timeSearch.Restart();
                Console.WriteLine(duration + "- Duracion");
                Information info = new Information(url, textToSearch, this.synopsis, incidences, duration, getCurrentCpuUsage());
                Console.WriteLine("Agregar " + info.url);
                resultsFoundParallel.Add(info); //Add in the list
                

            }
            return;
        }
        #endregion

        #region Methods that use sequential proccess
        /*
         *Case Secuencial number #1
         */
        //Main method to Search word in Secuencial
        public void mainSearchLow(String search)
        {
            string[] results = search.Split(';');
            words = results;
            timeLowMethods = Stopwatch.StartNew();
            for (int i = 0; i < words.Length; i++)
            {
                searchURLLow(words[i]);
            }
            timeSequential = timeLowMethods.ElapsedMilliseconds;
            Console.WriteLine(timeLowMethods.ElapsedMilliseconds + "");
        }
        /*
       *Case Secuencial number #2
       */
        //Method to Response URL and get html
        private void ResponseURLLow(String url, String aux)
        {
              try
                {
                    // Create a request for the URL. 
                    WebRequest request = WebRequest.Create(url);
                    // If required by the server, set the credentials.
                    // If required by the server, set the credentials.
                    request.Credentials = CredentialCache.DefaultCredentials;
                    // Get the response.
                    WebResponse response = request.GetResponse();
                    // Get the stream containing content returned by the server.
                    Stream dataStream = response.GetResponseStream();
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.
                    string responseFromServer = reader.ReadToEnd();
                    //Call Case#3
                    initSearchLow(responseFromServer, aux,url);
                }
                catch (Exception)
                {

                }
        }
        /*
        *Case Secuencial number #3
        */
        //Main method to search word in the page html and save in the list corresponds
        public void initSearchLow(string page, string textToSearch,string url)
        {
            flag = true;
            incidences = 0;
            //Divide de page in slides
            string[] slides = page.Split('>');
            for (int i = 0; i < slides.Length; i++)
            {
                search(slides[i], textToSearch,flag);
            }
            if (incidences > 0)
            {
                long duration = timeSearch.ElapsedMilliseconds; //Time duration 

                timeSearch.Restart();
                Console.WriteLine(duration + "- Duracion");
                Information info = new Information(url, textToSearch, this.synopsis, incidences, duration, getCurrentCpuUsage());
                Console.WriteLine("Agregar " + info.url);
                resultsFoundSequential.Add(info); //Add in the 

            }
            return;
        }

        #endregion


        //Method to check if contain word in the text
        public void search(string analyze, string textToSearch,Boolean getSinopsis)
        {   
            if (analyze.Contains(textToSearch))
            {
                if (getSinopsis)
                {
                    this.synopsis = analyze;
                    flag = false;
                }
                incidences++;
                int index = analyze.IndexOf(textToSearch);
                if (index + 1 != analyze.Length)
                {
                    string text = analyze.Substring(index + 2, analyze.Length - (index + 2));
                    search(text, textToSearch, flag);
                }
            }

        }

        
        /*
         *Case Secuencial number #2
         */
        //Main method to pass Url in Secuencial
        public void searchURLLow(String aux)

        {

            for (int i = 0; i < listURL.Length; i++)
            {
                ResponseURLLow(listURL[i], aux);
            }

        }
        //Principal method to add url in the list 
        private void addListURL()
        {
            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(@"./listadoLinks.txt"); //Direction of txt
            //Add line in the listURL

            while ((line = file.ReadLine()) != null)
            {
                temp.Add(line);

            }
            // Close file
            file.Close();
        }
        //conver color from hexadecimal to rgb
        public Color getColor(string hexadecimal)
        {
            try
            {
                Color color = ColorTranslator.FromHtml(hexadecimal);
                int r = Convert.ToInt16(color.R);
                int g = Convert.ToInt16(color.G);
                int b = Convert.ToInt16(color.B);

                return Color.FromArgb(r, g, b);
            }
            catch(Exception)
            {
                return Color.FromArgb(0, 0, 0);
            }
        }


        public double getCurrentCpuUsage()
        {
            return cpuCounter.NextValue();
        }

        public double getAvailableRAM()
        {
            return ramCounter.NextValue();
        }
       
        


        public string[] deleteTag(string[] vector)
        {

            Parallel.For(0, (vector.Length - 1), index =>
               {
                   if (vector[index].Contains("<"))
                   {
                       int i = vector[index].IndexOf('<');
                       if (i >= 0 && i < vector[index].Length)
                       {
                           vector[index] = vector[index].Substring(0, i);
                       }



                   }
               });
                return vector;
        }

        }

}
