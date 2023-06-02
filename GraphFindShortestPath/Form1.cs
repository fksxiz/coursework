using Graph;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphFindShortestPath
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //graphEditor1.VertexName = VertexNameTextBox.Text.ToString();
            if (graphEditor1.IsVertexAddMode)
            {
                graphEditor1.IsVertexAddMode = false;
            }
            else
            {
                graphEditor1.IsVertexAddMode = true;
            }
             
        }

        private void graphEditor1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (srcTextBox.Text != ""&& dstTextBox.Text != ""&& weightTextBox.Text != "")
            {
                try
                {
                graphEditor1.addEdge(srcTextBox.Text, dstTextBox.Text, int.Parse(weightTextBox.Text));
                }
                catch
                {
                    MessageBox.Show("Вес должен быть указан в целочисленном формате.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void FindShortestPathButton_Click(object sender, EventArgs e)
        {
            if (shortestSrcTextBox.Text != "" && shortestDstTextBox.Text != "")
            {
                    graphEditor1.FindShortestPath(shortestSrcTextBox.Text, shortestDstTextBox.Text);
            }
            else
            {
                MessageBox.Show("Заполните все поля.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void stateButton_Click(object sender, EventArgs e)
        {
            graphEditor1.State();
        }

        private void graphEditor1_ChangeState(object sender, EventArgs e)
        {
            EventsListBox.Items.Add("Изменение состояния");
        }

        private void graphEditor1_EdgeAdd(object sender, EventArgs e)
        {
            EventsListBox.Items.Add("Добавлено ребро");
        }

        private void graphEditor1_VertexAdd(object sender, EventArgs e)
        {
            EventsListBox.Items.Add("Добавлена вершина");
        }

        private void graphEditor1_FindedShortestPath(object sender, EventArgs e)
        {
            EventsListBox.Items.Add("Найден кротчайший путь");
        }

        private void graphEditor1_ResetGraph(object sender, EventArgs e)
        {
            EventsListBox.Items.Add("Сброс графа");
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            graphEditor1.Reset();
        }
    }
}
