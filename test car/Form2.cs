using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test_car
{
    public partial class Form2 : Form
    {
        public Form1 f;
        public Form2(Form1 f1)
        {
            f=f1;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Введите название машины");
            }
            else
            {
                try
                {
                    f.delNode(f.treeView1.Nodes[0]);
                }
                    //На случай, если дерева нет
                catch (ArgumentOutOfRangeException)
                {}
                f.NewCar = new Car(textBox1.Text, (int)numericUpDown1.Value, (int)numericUpDown2.Value);
                f.LoadTree();

                Hide();
            }
        }
    }
}
