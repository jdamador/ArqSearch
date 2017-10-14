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
using MethodSearch;
namespace Web_Navigator
{
    public partial class Statistics : Form
    {
        Form beforeWindow;
        int countURL = 0;
         ArrayList resultsFoundParallel; //Save Information of Parallel
         ArrayList resultsFoundSequential;// Save Information of Secuencial
        Methods meto;
        public Statistics()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized; /*Open this windows in full screen*/
            panelMenu.BackColor = Color.FromArgb(27, 32, 41);
            panelViewSearch.BackColor = Color.FromArgb(36, 45, 60);
            panelViewClock.BackColor = Color.FromArgb(36, 45, 60);
            panelViewSearch.Visible = false;
            panelViewClock.Visible = false;
        }

        //Methods to Jump Windows
        public void JumpWindows(Form beforeWin,Object aux)
        {
            meto = (Methods)aux;
            beforeWin.Hide();
            this.beforeWindow = beforeWin;
            if (meto.resultsFoundParallel != null)
            {

                resultsFoundParallel = meto.resultsFoundParallel;
            }
            if (meto.resultsFoundSequential != null)
                this.resultsFoundSequential = meto.resultsFoundSequential;


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Show Charts in the first position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            panelViewClock.Visible = false;
            panelViewSearch.Visible = true;
            showCharts();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            panelViewSearch.Visible = false;
            panelViewClock.Visible = true;
            showChartProcess();
            
        }
        private void  showChartProcess() {
            chartTimeProcess.Series.Clear();

            chartTimeProcess.Series.Add("Total Time Parallel");
            chartTimeProcess.Series.Add("Total Time Sequential");
            chartTimeProcess.Series["Total Time Parallel"].Points.AddXY("Parallel",meto.timeParallel);
            chartTimeProcess.Series["Total Time Sequential"].Points.AddXY("Sequential", meto.timeSequential);
            chartTimeProcess.Series["Total Time Parallel"].Points[0].AxisLabel = "Parallel";
            chartTimeProcess.Series["Total Time Sequential"].Points[0].AxisLabel = "Sequential";
        }
        private void panelView_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (panelViewSearch.Enabled == true)
            {


                if (countURL != 0)
                {
                    this.countURL--;
                }
                else
                {
                    this.countURL = 0;
                }

                showCharts();
            }
        }
        /// <summary>
        /// Shows Chats 
        /// </summary>
        private void showCharts()
        {
            Console.WriteLine("Esta apunto de iniciar");
            Console.WriteLine("Contador del Secuencial: " + resultsFoundSequential.Count);
            Console.WriteLine("Contador del Parallel: " + resultsFoundParallel.Count);
            if (resultsFoundSequential.Count > 0 || resultsFoundParallel.Count > 0)
            {

                Console.WriteLine("Entro al show");
                Information infoSequential = null;
                Information infoParallel = null;
                if (resultsFoundSequential.Count > resultsFoundParallel.Count)
                {
                    infoSequential = (Information)resultsFoundSequential[countURL];
                    infoParallel = null;

                    Parallel.For(0, resultsFoundParallel.Count, i =>
                    {
                        Information aux = (Information)resultsFoundParallel[i];
                        if (aux.url.Equals(infoSequential.url))
                        {
                            infoParallel = aux;
                            return;
                        }
                    });
                }
                else
                {
                    infoSequential = null;
                    infoParallel = (Information)resultsFoundParallel[countURL]; ;

                    Parallel.For(0, resultsFoundSequential.Count, i =>
                    {
                        Information aux = (Information)resultsFoundSequential[i];
                        if (aux.url.Equals(infoParallel.url))
                        {
                            infoSequential = aux;
                            return;
                        }
                    });
                }

                if (infoParallel != null)
                {
                    
                    //Chart Parallel
                    chartInsidenceParallel.Titles.Clear(); //Clear title
                    chartInsidenceParallel.Titles.Add(infoParallel.url); //Title url
                    chartInsidenceParallel.Series.Clear(); //Clear Series
                    chartInsidenceParallel.Series.Add(infoParallel.word); //Word Search
                
                    chartInsidenceParallel.ChartAreas[0].AxisY.Interval = 5;
                    chartInsidenceParallel.Series[infoParallel.word].Points.AddXY("NumberIncidences", infoParallel.numIncedences);// Num Incedinces of word

                    //Chart Time Parallel
                    chartTimeParallel.Titles.Clear(); //Clear title
                    chartTimeParallel.Titles.Add("Time Parallel");
                    chartTimeParallel.Series.Clear(); //Clear Series
                    chartTimeParallel.Series.Add("Time Parallel Process");
                    chartTimeParallel.Series.Add("Time CPU Process");
                    chartTimeParallel.ChartAreas[0].AxisY.Interval = 10000;
                    chartTimeParallel.Series["Time Parallel Process"].Points.AddXY("Time MilliSeg", infoParallel.timeDuration);
                    chartTimeParallel.Series["Time CPU Process"].Points.AddXY("Time CPU", infoParallel.timeCPU);

                }
                if (infoSequential != null)
                {
                    // Chart Sequential
                    chartInsidenceSequential.Titles.Clear(); //Clear title
                    chartInsidenceSequential.Titles.Add(infoSequential.url); //Title url
                    chartTimeParallel.Series.Clear(); //Clear Series                                                         
                    chartInsidenceSequential.Series.Add(infoSequential.word); //Word Search
                    chartInsidenceSequential.ChartAreas[0].AxisY.Interval = 5;
                    chartInsidenceSequential.Series[infoSequential.word].Points.AddXY("NumberIncidences", infoSequential.numIncedences);// Num Incedinces of word

                    //Chart TimeSequential
                    chartTimeSequential.Titles.Clear(); //Clear title
                    chartTimeSequential.Titles.Add("Time Sequential");
                    chartTimeSequential.Series.Clear(); //Clear Series
                    chartTimeSequential.Series.Add("Time CPU Process");
                    chartTimeSequential.Series.Add("Time Sequential Process");
                    chartTimeSequential.ChartAreas[0].AxisY.Interval = 10000;
                    chartTimeSequential.Series["Time Sequential Process"].Points.AddXY("Time MilliSeg", infoSequential.timeDuration);
                    chartTimeSequential.Series["Time CPU Process"].Points.AddXY("Time CPU", infoSequential.timeCPU);

                }
            }
        }
        /// <summary>
        /// Button sig
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSig_Click(object sender, EventArgs e)
        {
            if (panelViewSearch.Enabled == true)
            {
                int max = 0;
                if ((resultsFoundParallel.Count - 1) >= (resultsFoundSequential.Count - 1))
                    max = (resultsFoundParallel.Count - 1);
                else
                    max = (resultsFoundSequential.Count - 1);

                if (countURL != max)
                {
                    this.countURL++;
                }
                else
                {
                    this.countURL = max;
                }

                showCharts();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            beforeWindow.Show();
        }
    }
}
