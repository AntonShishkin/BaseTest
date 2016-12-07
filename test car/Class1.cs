using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_car
{
    //Интерфейсы
    interface IRotatable
    {
        void Moove(Form1 f1);
    }

    interface IDoor
    {
        string Open(Form1 f1);
    }

    //Абстрактный класс
    public abstract class Detail
    {

        public double Weight { get; set; }
        public string Name { get; set; }

    }

    //Машина
    public class Car : Detail
    {

        public List<Wheel> wheel = new List<Wheel>();
        public List<Door> door = new List<Door>();
        public Body body1=new Body();
        public Random rnd = new Random();

        public Car(string name, int ndoors, int nwheel)
        {
            Wheel.Wnum = 0;
            Door.Dnum = 0;
            Wheel.Nut.Nutnum = 0;

            Name = name;
            body1.Name = "Рама ";
            body1.Weight = (double)rnd.Next(19500, 20500) / 100;

            for (int i = 0; i < nwheel; i++)
            {
                wheel.Add(new Wheel(i + 1));
                wheel[i].Name = "Колесо "+(++Wheel.Wnum);
                wheel[i].Weight = (double)rnd.Next(1400, 1500) / 100;
            }

            for (int i = 0; i < ndoors; i++)
            {
                door.Add(new Door(i + 1));
                door[i].Name = "Дверь "+(++Door.Dnum);
                door[i].Weight = (double)rnd.Next(2500, 2600) / 100;
            }
        }

        //Расчёт веса
        public double WeightAll()
        {
            double ves = 0;
            foreach (Wheel a in wheel)
            {
                ves += a.Weight;
                foreach (Wheel.Nut b in a.nut)
                {
                    ves += b.Weight;
                }
            }

            foreach (Door a in door)
            {
                ves += a.Weight;
            }
            if (body1!=null)
            {
            ves += body1.Weight;
            }

            foreach (Wheel.Nut a in Form1.nutG)
            {
                ves += a.Weight;
            }
            return ves;
        }

        //Колёса
        public class Wheel : Detail, IRotatable
        {
            static public int Wnum = 0;
            private Random rnd = new Random();

            public List<Nut> nut = new List<Nut>();
            public int PorNum;

            public Wheel(int pn)
            {

                PorNum = pn;

                for (int i = 0; i < 6; i++)
                {
                    nut.Add(new Nut());
                    nut[i].Name = "Гайка "+(++Nut.Nutnum);
                    nut[i].Weight = (double)rnd.Next(1, 2) / 100;
                }
            }

            public void Moove(Form1 f1)
            {
                f1.setTextBox("Колесо № "+PorNum+" вращается");
            }

            //болты
            public class Nut : Detail
            {
                static public int Nutnum=0;
            }
        }

        //Двери
        public class Door : Detail, IDoor
        {
            static public int Dnum = 0;
            public int PorNum;
            string Op_Cl = "закрыта";

            public string Open(Form1 f1)
            {

                if (Op_Cl == "закрыта")
                {
                    Op_Cl = "открыта";
                }
                else
                {
                    Op_Cl = "закрыта";
                }

                return Name+" "+Op_Cl;
            }

            public Door(int pn)
            {
                PorNum = pn;
            }
        }

        //Корпус
        public class Body : Detail, IDoor, IRotatable
        {

            public string Open(Form1 f1)
            {
                return "Увы, это не дверь";
            }

            public void Moove(Form1 f1)
            {
                f1.setTextBox("Машина едет");
            }


        }
    }
}
