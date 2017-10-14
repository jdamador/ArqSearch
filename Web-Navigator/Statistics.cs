using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MethodSearch;
using System.Collections;

namespace Web_Navigator
{
    public partial class Statistics : Form
    {
        Form beforeWindow;
        int countURL = 0;
        public ArrayList resultsFoundParallel = new ArrayList(); //Save Information of Parallel
        public ArrayList resultsFoundSequential = new ArrayList();// Save Information of Secuencial
        public Statistics()
        {
            InitializeComponent();

            panelMenu.BackColor = Color.FromArgb(27, 32, 41);
            panelView.BackColor = Color.FromArgb(36, 45, 60);
        }

        //Methods to Jump Windows
        public void jumpWindows(Form beforeWin, ArrayList parallel, ArrayList sequential)
        {
            beforeWin.Hide();
            this.beforeWindow = beforeWin;
            if (parallel != null && sequential != null)
            {
                this.resultsFoundParallel = parallel;
                this.resultsFoundSequential = sequential;
            }


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            showChats();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void panelView_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (countURL != 0)
            {
                this.countURL--;

            }
            else
            {
                this.countURL = 0;
            }
            showChats();
        }
        private void showChats()
        {
            if (resultsFoundSequential.Count > 0 && resultsFoundParallel.Count > 0)
            {
                Information infoSequential = (Information)resultsFoundSequential[countURL];
                Information infoParallel = null;
                Parallel.For(0, resultsFoundParallel.Count, i =>
                {
                    Information aux = (Information)resultsFoundParallel[i];
                    if (aux.url.Equals(infoSequential.url))
                    {
                        infoParallel = aux;
                    }
                });
                if (infoParallel != null)
                {
                   

                    //Chart Parallel
                    chartInsidenceParallel.Titles.Clear(); //Clear title
                    chartInsidenceParallel.Titles.Add(infoParallel.url); //Title url
                    chartInsidenceParallel.Series.Clear(); //Clear Series
                    chartInsidenceParallel.Series.Add(infoParallel.word); //Word Search
                    chartInsidenceParallel.Series[infoParallel.url].Points.AddXY("NumberIncidences", infoParallel.numIncedences);// Num Incedinces of word

                    //Chart TimeSequential
                    chartTimeSequential.Titles.Clear(); //Clear title
                    chartTimeSequential.Titles.Add("Time Sequential");
                    chartTimeSequential.Series.Clear(); //Clear Series
                    chartTimeSequential.Series.Add("Time Sequential Process");
                    chartTimeSequential.Series["Time Sequential Process"].Points.AddXY("Time MilliSeg", infoSequential.timeDuration);

                    


                }
                // Chart Sequential
                chartInsidenceSequential.Titles.Clear(); //Clear title
                chartInsidenceSequential.Titles.Add(infoSequential.url); //Title url
                chartInsidenceSequential.Series.Clear(); //Clear Series
                chartInsidenceSequential.Series.Add(infoSequential.word); //Word Search
                chartInsidenceSequential.Series[infoSequential.url].Points.AddXY("NumberIncidences", infoSequential.numIncedences);// Num Incedinces of word
                                                                                                                                   //Chart TimeParallel
                chartTimeParallel.Titles.Clear(); //Clear title
                chartTimeParallel.Titles.Add("Time Parallel");
                chartTimeParallel.Series.Clear(); //Clear Series
                chartTimeParallel.Series.Add("Time Parallel Process");
                chartTimeParallel.Series["Time Parallel Process"].Points.AddXY("Time MilliSeg", infoParallel.timeDuration);

            }
        }

        private void btnSig_Click(object sender, EventArgs e)
        {
            if (countURL != (resultsFoundSequential.Count - 1))
            {
                this.countURL++;
            }
            else
            {
                this.countURL = resultsFoundSequential.Count - 1;
            }
            showChats();
        }

        private void Statistics_Load(object sender, EventArgs e)
        {

        }
    }
}
