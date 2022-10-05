using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace GameSafeApp
{
    public partial class Form1 : Form
    {
        // Сумма элементов массива кнопок
        private int _sumElems;

        // Размер массива
        private int _sizeSafe;

        // Массив кнопок
        private Button[,] _btnMass;

        // Содержит объект изображения вертикального состояния кнопки (в массиве воспринимается как 0 для подсчета суммы элементов)
        private readonly Bitmap _imageVertical = GameSafeApp.Image.Handle_Vertical;

        // Содержит объект изображения горизонтального состояния кнопки (в массиве воспринимается как 1 для подсчета суммы элементов)
        private readonly Bitmap _imageHorizon = GameSafeApp.Image.Handle_Horizon;

        // Объект для секундомера
        private readonly Stopwatch _stopWatch;

        public Form1()
        {
            InitializeComponent();

            // Инициализируем объект секундомера при создании формы и далее используем только его
            _stopWatch = new Stopwatch();
        }

        // Кнопка для создания массива (сейфа)
        private void button2_Click(object sender, System.EventArgs e)
        {
            // Если сейф уже создан, то выскакивает сообщение, что поле нужно очистить
            if (_sizeSafe > 2)
            {
                textBox1.Text = "Сначала очистите поле!";
                return;
            }
            
            // Проверка на корректность введенных данных
            if (!int.TryParse(textBox1.Text, out _sizeSafe) || _sizeSafe < 2)
            {
                textBox1.Text = "";
                return;
            }

            // Переменная со случайной величиной 0 или 1
            var rand = new Random();
            var temp = rand.Next(2);

            // Определяем какая картинка будет начальной перед запутыванием сейфа (0 - вертикальное положение, 1 - вертикальное положение)
            Bitmap image = temp == 0 ? _imageVertical : _imageHorizon;

            // Инициализиурем массив
            _btnMass = new Button[_sizeSafe, _sizeSafe];

            // Считаем сумму элементов начального массива
            _sumElems = _btnMass.Length * temp;

            int left = 10;

            // Создаем кнопки, задаем определенное расположение, размер и изображение, а также имя кнопки
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

            // Начиаем отсчет секундомера
            _stopWatch.Start();
        }

        // Метод при нажатии на ручку сейфа
        private void ButtonOnClick(object sender, EventArgs eventArgs)
        {
            if (sender is Button button)
            {
                // Вычисляем индексы кнопки по его имени
                var n = Convert.ToInt32(button.Name);

                int _i = n / _sizeSafe, _j = n % _sizeSafe;

                // Поворачиваем ручки по правилу условия
                ChangeButtons(_i, _j);

                // Если сейф открывается (все ручки расположены парраллельно)
                if (SafeIsOpen())
                {
                    // Останавливаем секундомер
                    _stopWatch.Stop();

                    // Очищаем поле от кнопок
                    panel1.Controls.Clear();
                    // Задаем новое изображение для поля
                    panel1.BackgroundImage = GameSafeApp.Image._1650921531_25_vsegda_pomnim_com_p_gora_deneg_foto_27;

                    // Выводим текст о победе
                    var lab = new Label();

                    lab.Text = "Congratulate!";
                    lab.ForeColor = Color.Green;
                    lab.BackColor = Color.White;

                    panel1.Controls.Add(lab);
                }
            }
        }

        // Кнопка для очиски поля
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

        // Метод проверки открытого сейфа
        private bool SafeIsOpen()
        {
            // Сейф открывается тогда, когда сумма элементов равна 0 или количеству элементов
            return _sumElems == 0 || _sumElems == _btnMass.Length;
        }

        // Метод запутывания сейфа
        private void CreateSafe()
        {
            var rnd = new Random();

            var k = 0;
            
            // Цикл работает несколько раз, чтобы запутать сейф
            while (k < _sizeSafe || _sumElems == _btnMass.Length || _sumElems == 0)
            {
                // Задаем случайные значения для поворота ручек
                var _i = rnd.Next(_sizeSafe);
                var _j = rnd.Next(_sizeSafe);

                ChangeButtons(_i, _j);

                k++;
            }
        }

        // Метод поворота ручек
        private void ChangeButtons(int _i, int _j)
        {
            for (int i = 0; i < _sizeSafe; i++)
            {
                // Если ручка расположена вертикально (значение 0), меняем ее на горизонтальное положение (значение 1)
                // далее для всех условий соответствующие проверки
                if (_btnMass[i, _j].BackgroundImage == _imageVertical)
                {
                    _btnMass[i, _j].BackgroundImage = _imageHorizon;
                    _sumElems += 1;
                    // После смены положения в сумме элементов происходят соответствующие изменения
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

        // Метод для отсчета секундомера
        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = _stopWatch.Elapsed.ToString("hh':'mm':'ss':'fff");
        }
    }
}
