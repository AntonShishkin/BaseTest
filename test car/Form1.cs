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

    public partial class Form1 : Form
    {
        public static List<Car.Wheel.Nut> nutG= new List<Car.Wheel.Nut>();//Лист для новых гаек
        public List<string> txt=new List<string>();//коллекция для многострочного текста
        public Car NewCar;

        public Form1()
        {
            InitializeComponent();
        }

        public void setTextBox(string s)
        {
            txt.Add(s);
        }

        private void button1_Click(object sender, EventArgs e)
        {//Кнопка "Движение"
            if (NewCar == null)
            {
                MessageBox.Show("Сначала создайте машину");
            }
            else
            {
                txt.RemoveRange(0, (int)txt.LongCount());
                //Двигаем колёса
                foreach (Car.Wheel a in NewCar.wheel)
                {
                    a.Moove(this);
                }
                //Двигаем машину
                if (NewCar.body1!=null)
                {
                NewCar.body1.Moove(this);
                }

                string[] txt1 = new string[(int)txt.LongCount()];
                int is1 = 0;
                foreach (string a in txt)
                {
                    txt1[is1] = a;
                    is1++;
                }
                richTextBox1.Lines = txt1;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {//Окно для ввода новой машины
            Form2 fo2 = new Form2(this);
            fo2.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {//Расчёт массы
            if (NewCar == null)
            {
                MessageBox.Show("Сначала создайте машину");
            }
            else
            {
                richTextBox1.Text = "Общая масса машины "+NewCar.WeightAll()+" кг.";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {//Открывание дверей
            if (NewCar == null)
            {
                MessageBox.Show("Сначала создайте машину");
            }
            else
            {
                //Если выбран 0
                    if (numericUpDown1.Value == 0)
                    {
                        richTextBox1.Text = NewCar.body1.Open(this);
                    }
                    else
                    {//Иначе ищем дверь
                        bool find = false;
                        for (int i = 0; i < (int)NewCar.door.LongCount();i++ )
                            if ((int)numericUpDown1.Value == NewCar.door[i].PorNum)
                            {
                                richTextBox1.Text = NewCar.door[i].Open(this);
                                find = true;
                                break;
                            }

                        if (!find)
                        {richTextBox1.Text = "Выбранной двери не существует";
                        }
                    }
                }
            
        }

        public void LoadTree()
        {//Построение дерева
            TreeNode CarNode = new TreeNode(NewCar.Name);
            CarNode.Nodes.Add(new TreeNode(NewCar.body1.Name));

            for (int i = 0; i < NewCar.wheel.LongCount(); i++)
            {
                CarNode.Nodes.Add(new TreeNode(NewCar.wheel[i].Name));

                for (int i1 = 0; i1 < NewCar.wheel[i].nut.LongCount() ; i1++)
                {
                    CarNode.Nodes[i+1].Nodes.Add(new TreeNode(NewCar.wheel[i].nut[i1].Name));
                }
            }
            for (int i = 0; i < NewCar.door.LongCount(); i++)
            {
                CarNode.Nodes.Add(new TreeNode(NewCar.door[i].Name ));
            }

            treeView1.Nodes.Add(CarNode);
        }

        private void button5_Click(object sender, EventArgs e)
        {//Удалить
            delNode(treeView1.SelectedNode);
        }

        public void delNode(TreeNode DelN)
        {//Процедура удаления узлов
            try
            {
                if (DelN != null)
                {
                    if ((NewCar != null) && (DelN.Text == NewCar.Name))
                    {
                        NewCar = null;
                    }
                    else
                    {
                        string NameS = DelN.Text.Remove(4);
                        switch (NameS)
                        {
                            case "Рама":
                                {
                                    NewCar.body1 = null;
                                    break;
                                }
                            case "Двер":
                                {
                                    for (int i = 0; i < (int)NewCar.door.LongCount(); i++)
                                    {
                                        if (NewCar.door[i].Name == DelN.Text)
                                        {
                                            NewCar.door.RemoveAt(i);
                                            break;
                                        }
                                    }
                                    break;
                                }
                            case "Коле":
                                {
                                    for (int i = 0; i < (int)NewCar.wheel.LongCount(); i++)
                                    {
                                        if (NewCar.wheel[i].Name == DelN.Text)
                                        {
                                            NewCar.wheel.RemoveAt(i);
                                            break;
                                        }
                                    }
                                    break;
                                }
                            case "Гайк":
                                {
                                    bool fnd = false;
                                    for (int i = 0; i < (int)NewCar.wheel.LongCount(); i++)
                                    {

                                        for (int j = 0; j < (int)NewCar.wheel[i].nut.LongCount(); j++)
                                        {
                                            if (NewCar.wheel[i].nut[j].Name == DelN.Text)
                                            {
                                                NewCar.wheel[i].nut.RemoveAt(j);
                                                fnd = true;
                                                break;
                                            }
                                        }

                                    }

                                    if (!fnd)
                                    {
                                        for (int i = 0; i < (int)nutG.LongCount(); i++)
                                        {
                                            if (nutG[i].Name == DelN.Text)
                                            {
                                                nutG.RemoveAt(i);
                                                break;
                                            }
                                        }
                                    }
                                    break;
                                }
                        }
                    }
                    //Повторяем для всех вложенных узлов
                    TreeNodeCollection LNode = DelN.Nodes;
                    if (LNode != null)
                    {
                        for (int i = 0; i < LNode.Count; i++)
                        {
                            delNode(LNode[i]);
                        }
                    }

                    DelN.Remove();
                }
            }
            catch (NullReferenceException)
            {
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {//добавить дверь
            if (treeView1.SelectedNode != null)
            {
                TreeNode baz = treeView1.SelectedNode;
                int whe = (int)NewCar.door.LongCount();
                NewCar.door.Add(new Car.Door(++Car.Door.Dnum));
                NewCar.door[whe].Name = "Дверь " + Car.Door.Dnum;
                NewCar.door[whe].Weight = (double)NewCar.rnd.Next(2500, 2600) / 100;
                baz.Nodes.Add(new TreeNode(NewCar.door[whe].Name));
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {//Добавить колесо
            if (treeView1.SelectedNode != null)
            {
                TreeNode baz = treeView1.SelectedNode;
                int whe = (int)NewCar.wheel.LongCount();
                NewCar.wheel.Add(new Car.Wheel(++Car.Wheel.Wnum));
                NewCar.wheel[whe].Name = "Колесо " + Car.Wheel.Wnum;
                NewCar.wheel[whe].Weight = (double)NewCar.rnd.Next(2500, 2600) / 100;

                baz.Nodes.Add(new TreeNode(NewCar.wheel[whe].Name));
                for (int i1 = 0; i1 < NewCar.wheel[whe].nut.LongCount(); i1++)
                {
                    int num1 = baz.Nodes.Count;
                    baz.Nodes[num1 - 1].Nodes.Add(new TreeNode(NewCar.wheel[whe].nut[i1].Name));
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {//Добавить гайку
            if (treeView1.SelectedNode != null)
            {
                TreeNode baz = treeView1.SelectedNode;

                nutG.Add(new Car.Wheel.Nut());
                nutG[(int)nutG.LongCount() - 1].Name = "Гайка " + (++Car.Wheel.Nut.Nutnum);
                nutG[(int)nutG.LongCount() - 1].Weight = (double)NewCar.rnd.Next(1, 2) / 100;
                baz.Nodes.Add(new TreeNode(nutG[(int)nutG.LongCount() - 1].Name));
            }
        }
    }
}
