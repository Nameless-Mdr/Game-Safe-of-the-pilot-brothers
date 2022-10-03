using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace GameSafeApp
{
    public partial class Form1 : Form
    {
        private int _sumElems;

        private int _sizeSafe;

        private Button[,] _btnMass;

        private readonly Bitmap _imageVertical = GameSafeApp.Image.Handle_Vertical;
        private readonly Bitmap _imageHorizon = GameSafeApp.Image.Handle_Horizon;

        private Stopwatch _stopWatch;

        public Form1()
        {
            InitializeComponent();

            _stopWatch = new Stopwatch();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            panel1.BackgroundImage = null;

            if (_sizeSafe > 2)
            {
                textBox1.Text = "Сейф уже создан!";
                return;
            }

            if (!int.TryParse(textBox1.Text, out _sizeSafe) || _sizeSafe < 2)
            {
                textBox1.Text = "";
                return;
            }

            var rand = new Random();
            var temp = rand.Next(2);

            Bitmap image = temp == 0 ? _imageVertical : _imageHorizon;

            _btnMass = new Button[_sizeSafe, _sizeSafe];

            _sumElems = _btnMass.Length * temp;

            int left = 10;

            for (int i = 0; i < _sizeSafe; i++)
            {
                int top = 10;

                for (int j = 0; j < _sizeSafe; j++)
                {
                    _btnMass[i, j] = new Button();

                    _btnMass[i, j].Width = 70;
                    _btnMass[i, j].Height = 70;

                    _btnMass[i, j].Left = left;
                    _btnMass[i, j].Top = top;

                    _btnMass[i, j].Name = Convert.ToString(i * _sizeSafe + j);

                    _btnMass[i, j].BackgroundImage = image;
                    _btnMass[i, j].BackgroundImageLayout = ImageLayout.Stretch;

                    _btnMass[i, j].Click += ButtonOnClick;

                    panel1.Controls.Add(_btnMass[i, j]);

                    top += _btnMass[i, j].Height + 2;
                }

                left += 70;
            }

            CreateSafe();

            _stopWatch.Start();
        }

        private void ButtonOnClick(object sender, EventArgs eventArgs)
        {
            if (sender is Button button)
            {
                var n = Convert.ToInt32(button.Name);

                int _i = n / _sizeSafe, _j = n % _sizeSafe;

                ChangeButtons(_i, _j);

                if (SafeIsOpen())
                {
                    _stopWatch.Stop();

                    panel1.Controls.Clear();
                    panel1.BackgroundImage = GameSafeApp.Image._1650921531_25_vsegda_pomnim_com_p_gora_deneg_foto_27;
                    var lab = new Label();
                    lab.Text = "Congratulate!";
                    panel1.Controls.Add(lab);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            panel1.BackgroundImage = null;
            textBox1.Text = "";
            _btnMass = null;
            _sizeSafe = 0;
            _sumElems = 0;
            _stopWatch.Reset();
        }

        private bool SafeIsOpen()
        {
            return _sumElems == 0 || _sumElems == _btnMass.Length;
        }

        private void CreateSafe()
        {
            var rnd = new Random();

            var k = 0;
            
            while (k < _sizeSafe || _sumElems == _btnMass.Length || _sumElems == 0)
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
                if (_btnMass[i, _j].BackgroundImage == _imageVertical)
                {
                    _btnMass[i, _j].BackgroundImage = _imageHorizon;
                    _sumElems += 1;
                }
                else if (_btnMass[i, _j].BackgroundImage == _imageHorizon)
                {
                    _btnMass[i, _j].BackgroundImage = _imageVertical;
                    _sumElems -= 1;
                }

                if (_btnMass[_i, i].BackgroundImage == _imageVertical && _j != i)
                {
                    _btnMass[_i, i].BackgroundImage = _imageHorizon;
                    _sumElems += 1;
                }
                else if (_btnMass[_i, i].BackgroundImage == _imageHorizon && _j != i)
                {
                    _btnMass[_i, i].BackgroundImage = _imageVertical;
                    _sumElems -= 1;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = _stopWatch.Elapsed.ToString("hh':'mm':'ss':'fff");
        }
    }
}
