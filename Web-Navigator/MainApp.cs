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
        methods meto = new methods();
        public MainApp()
        {
            InitializeComponent();
            backgroundContainer.BackColor = meto.getColor("#00e1c0");
            //dataGridView1.ColumnCount = 3;
            //dataGridView1.ColumnHeadersVisible = true;
            //dataGridView1.AutoSizeRowsMode =DataGridViewAutoSizeRowsMode.AllCells;
           
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                string txt = textBox1.Text;
                meto.mainSearchLow(txt);
            }
            else
            {
                MessageBox.Show("Don't leave space in blank");
            }
           
    }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //   this.Hide();
            Statistics windows = new Statistics();
            windows.Show();
            windows.jumpWindows(this, meto.resultsFoundParallel, meto.resultsFoundSequential);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                string txt = textBox1.Text;
                meto.mainSearchFast(txt);
                showResults(1);
            }
            else
            {
                MessageBox.Show("Don't leave space in blank");
            }
            showResults(1);
        }

        private void backgroundContainer_Paint(object sender, PaintEventArgs e)
        {

        }
        public void showResults(int option)
        {
            ArrayList list;
            if (option == 1)
            {
                list = meto.resultsFoundParallel;
            }
            else
            {
                list = meto.resultsFoundSequential;
            }
            for (int i = 0; i < list.Count; i++)
            {
                Information aux = (Information)list[i];
                string word = aux.word;
                string synopsis = aux.synopsis;
                string incidences =aux.word+": "+aux.numIncedences+" Found";
                string link = aux.url;
                list.RemoveAt(i);
                for (int j = 0; j < list.Count; j++)
                {
                    Information repeat = (Information)list[j];
                    if (repeat.url.Equals(link))
                    {
                        if(!word.Equals(repeat.word))
                        {
                            incidences +=" "+ repeat.word + ": " + repeat.numIncedences + " Found";
                            list.RemoveAt(j);
                        }
                    }
                }

                dataGridView1.Rows.Add(1);
                int last = dataGridView1.RowCount-1;
                dataGridView1[0, last].Value = incidences;
                dataGridView1[1, last].Value = synopsis;
                dataGridView1[2, last].Value = link;
            }
        }

        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                Process.Start((string)dataGridView1[e.ColumnIndex,e.RowIndex].Value);
            }
        }
    }
}
