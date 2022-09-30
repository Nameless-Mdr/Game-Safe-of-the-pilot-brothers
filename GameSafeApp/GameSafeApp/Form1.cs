using System;
using System.Windows.Forms;

namespace GameSafeApp
{
    public partial class Form1 : Form
    {
        private int _sumElems;

        private int _sizeSafe;

        private Button[,] _btnMass;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || !int.TryParse(textBox1.Text, out _sizeSafe) || _sizeSafe < 2)
            {
                textBox1.Text = "";
                return;
            }

            var rand = new Random();
            var temp = rand.Next(2);

            _sumElems = _sizeSafe * _sizeSafe * temp;

            _btnMass = new Button[_sizeSafe, _sizeSafe];

            int left = 10;

            for (int i = 0; i < _sizeSafe; i++)
            {
                int top = 10;

                for (int j = 0; j < _sizeSafe; j++)
                {

                    _btnMass[i, j] = new Button();
                    _btnMass[i, j].Left = left;
                    _btnMass[i, j].Top = top;

                    _btnMass[i, j].Name = Convert.ToString(i * _sizeSafe + j);
                    _btnMass[i, j].Click += ButtonOnClick;

                    _btnMass[i, j].Text = "" + temp;
                    _sumElems += temp;

                    this.panel1.Controls.Add(_btnMass[i, j]);
                    top += _btnMass[i, j].Height + 2;
                }

                left += 80;
            }

            CreateSafe();
        }

        private void ButtonOnClick(object sender, EventArgs eventArgs)
        {
            if (sender is Button button)
            {
                var n = Convert.ToInt32(button.Name);

                int _i = n / _sizeSafe, _j = n % _sizeSafe;

                ChangeButtons(_i, _j);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            //var result = 0;

            //for(int i = 0; i < )
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            textBox1.Text = "";
        }

        private bool SafeIsOpen()
        {
            var sum = SumOfSafe();

            return sum == 0 || sum == _sizeSafe * _sizeSafe;
        }

        private int SumOfSafe()
        {
            var sum = 0;

            for (int i = 0; i < _sizeSafe; i++)
            {
                for (int j = 0; j < _sizeSafe; j++)
                {
                    sum += Convert.ToInt32(_btnMass[i, j].Text);
                }
            }

            return sum;
        }

        private void CreateSafe()
        {
            var rnd = new Random();

            var k = 0;

            //var sum = SumOfSafe();

            while (k < 15 || _sumElems == _btnMass.Length || _sumElems == 0)
            {
                var _i = rnd.Next(_sizeSafe);
                var _j = rnd.Next(_sizeSafe);

                ChangeButtons(_i, _j);

                k++;
            }
        }

        private void ChangeButtons(int _i, int _j)
        {
            for (int i = 0; i < _sizeSafe; i++)
            {
                if (_btnMass[i, _j].Text == "0")
                {
                    _btnMass[i, _j].Text = "1";
                    _sumElems += 1;
                }
                else if (_btnMass[i, _j].Text == "1")
                {
                    _btnMass[i, _j].Text = "0";
                    _sumElems -= 1;
                }

                if (_btnMass[_i, i].Text == "0" && _j != i)
                {
                    _btnMass[_i, i].Text = "1";
                    _sumElems += 1;
                }
                else if (_btnMass[_i, i].Text == "1" && _j != i)
                {
                    _btnMass[_i, i].Text = "0";
                    _sumElems -= 1;
                }
            }
        }
    }
}
