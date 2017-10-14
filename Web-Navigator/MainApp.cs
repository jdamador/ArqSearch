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
using System.Diagnostics;
using System.Collections;

namespace Web_Navigator
{
    public partial class MainApp : Form
    {
        private Methods meto = new Methods();
        Information[] list;
        public MainApp()
        {
            this.WindowState = FormWindowState.Maximized; /*Open this windows in full screen*/
            InitializeComponent();
            //Set color and size
            backgroundContainer.Size = this.Size;
            backgroundContainer.BackColor = meto.getColor("#00e1c0");
            this.BackColor = meto.getColor("#00e1c0");
            dataGridView1.ColumnHeadersVisible = true;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;


        }
        /// <summary>
        /// 
        /// Make search with sequential system
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                string txt = textBox1.Text;
                meto.mainSearchLow(txt);
                showResults(2);
            }
            else
            {
                MessageBox.Show("Don't leave space in blank");
            }
           
    }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 
        /// Show the graphs with the information about of the results
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click_1(object sender, EventArgs e)
        {
            //   this.Hide();
            Statistics windows = new Statistics();
            windows.Show();
            windows.JumpWindows(this,meto);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Make search with parallel system
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click_2(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                string txt = textBox1.Text;
                meto.MainSearchFast(txt);
                showResults(1);
            }
            else
            {
                MessageBox.Show("Don't leave space in blank");
            }
           
        }

        private void backgroundContainer_Paint(object sender, PaintEventArgs e)
        {

        }
        /// <summary>
        /// Show information in dataView
        /// </summary>
        /// <param name="option"></param>
        public void showResults(int option)
        {
            ArrayList help;

            if (option == 1)
            {
                help = meto.resultsFoundParallel;
            }
            else
            {
                help = meto.resultsFoundSequential;
            }
            list = new Information[help.Count];
            for (int x = 0; x < help.Count; x++)
            {
                list[x] = (Information)help[x];
            }

            ArrayList words = new ArrayList();

            for (int j = 0; j < list.Length; j++)
            {

                Information aux = list[j];
                string word = aux.word;
                string synopsis = aux.synopsis;
                string link = aux.url;
                string incidences = aux.word + ": " + aux.numIncedences + " Found" + seachDoubles(link, word);
                Console.WriteLine("Link: " + link);
                words.Add(link);
                dataGridView1.Rows.Add(1);
                int last = dataGridView1.RowCount - 1;
                dataGridView1[0, last].Value = incidences;
                dataGridView1[1, last].Value = synopsis;
                dataGridView1[2, last].Value = link;

            }

        }
        public string seachDoubles(string link, string word)
        {
            string incidences = "";
            for (int i = 0; i < list.Length; i++)
            {
                Information repeat = list[i];
                if (repeat.url.Equals(link))
                {
                    if (!word.Equals(repeat.word))
                    {
                        incidences += " " + repeat.word + ": " + repeat.numIncedences + " Found";
                    }
                }
            }
            return incidences;
        }

        /// <summary>
        /// Go to the link
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                Process.Start((string)dataGridView1[e.ColumnIndex,e.RowIndex].Value);
            }
        }
    }
}
