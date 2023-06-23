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
                modeLabel.Text = "-";
            }
            else
            {
                graphEditor1.IsVertexAddMode = true;
                modeLabel.Text = "Режим добавления вершин";
            }
             
        }

        private void graphEditor1_Click(object sender, EventArgs e)
        {

        }
        private object locker=new object();
        private void button2_Click(object sender, EventArgs e)
        {
            if (srcTextBox.Text != ""&& dstTextBox.Text != ""&& weightTextBox.Text != "")
            {
                try
                {
                    lock (locker)
                    {
                        graphEditor1.addEdge(srcTextBox.Text, dstTextBox.Text, int.Parse(weightTextBox.Text));
                    }
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
                    pathTextBox.Text = graphEditor1.FindShortestPath(shortestSrcTextBox.Text, shortestDstTextBox.Text);
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

        private void SoundsButton_Click(object sender, EventArgs e)
        {
            if (graphEditor1.SoundsOn)
            {
                graphEditor1.SoundsOn = false;
            }
            else
            {
                graphEditor1.SoundsOn = true;
            }
        }

        private void RemoveEdgeButton_Click(object sender, EventArgs e)
        {
            if (srcTextBox.Text != "" && dstTextBox.Text != "" && weightTextBox.Text != "")
            {
                //for (int i=0;i<5;i++) {
                    try
                    {
                        graphEditor1.removeEdge(srcTextBox.Text, dstTextBox.Text);
                    }
                    catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
               //}
            }
            else
            {
                MessageBox.Show("Заполните все поля.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void RemoveVertexButton_Click(object sender, EventArgs e)
        {
            if (srcTextBox.Text != "")
            {
                //for (int i = 0; i < 7; i++)
                //{
                    try
                    {
                        graphEditor1.removeVertex(srcTextBox.Text);
                    }
                    catch { }
                //}
                
            }
            else
            {
                MessageBox.Show("Заполните все поля.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void EdgeModeButton_Click(object sender, EventArgs e)
        {
            if (graphEditor1.IsEdgeAddMode)
            {
                graphEditor1.IsEdgeAddMode = false;
                modeLabel.Text = "-";
            }
            else
            {
                graphEditor1.IsEdgeAddMode = true;
                modeLabel.Text = "Режим добавления рёбер";
            }
        }

        private void DeleteModeButton_Click(object sender, EventArgs e)
        {
            if (graphEditor1.IsDeleteMode)
            {
                graphEditor1.IsDeleteMode = false;
                modeLabel.Text = "-";
            }
            else
            {
                graphEditor1.IsDeleteMode = true;
                modeLabel.Text = "Режим удаления";
            }
        }

        private void graphEditor1_Click_1(object sender, EventArgs e)
        {

        }

        private void graphEditor1_VertexRemove(object sender, EventArgs e)
        {
            EventsListBox.Items.Add("Вершина удалена");
        }

        private void graphEditor1_EdgeRemove(object sender, EventArgs e)
        {
            EventsListBox.Items.Add("Грань удалена");
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
