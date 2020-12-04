using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Task01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel viewModel = new ViewModel();
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = viewModel;
            field.ItemsSource = viewModel.bricks;
            //field.DisplayMemberPath = nameof(Brick.GetNumber);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            bool isNextRound = false;
            switch (e.Key)
            {
                case Key.Left:
                    isNextRound = viewModel.MoveLeft();
                    break;
                case Key.Up:
                    isNextRound = viewModel.MoveUp();
                    break;
                case Key.Right:
                    isNextRound = viewModel.MoveRight();
                    break;
                case Key.Down:
                    isNextRound = viewModel.MoveDown();
                    break;

                case Key.A:
                    goto case Key.Left;
                case Key.S:
                    goto case Key.Down;
                case Key.W:
                    goto case Key.Up;
                case Key.D:
                    goto case Key.Right;

                case Key.Escape:
                    if (!viewModel.EscapeGame())
                        this.Close();
                    break;

                default:
                    break;
            }

            if (isNextRound)
                viewModel.AddOneBrick();

            else if (viewModel.CheckIsWin())
                MessageBox.Show("You are winer");

            else if (viewModel.CheckIsEnd())
            {
                MessageBox.Show("You are looser!!");
                if (viewModel.EndOfGame())
                    viewModel.StartGame();
                else
                    this.Close();
            }

        }
    }

    class GetColor
    {
        public static SolidColorBrush Col0 => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCDC1B3"));
        public static SolidColorBrush Col2 => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFEEE4DA"));
        public static SolidColorBrush Col4 => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFEDE0C8"));
        public static SolidColorBrush Col8 => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF2B179"));
        public static SolidColorBrush Col16 => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF59563"));
        public static SolidColorBrush Col32 => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF67C60"));
        public static SolidColorBrush Col64 => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF65E3B"));
        public static SolidColorBrush Col128 => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFEDCF73"));
        public static SolidColorBrush Col256 => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFEDCC62"));
        public static SolidColorBrush Col512 => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFEDC850"));
        public static SolidColorBrush Col1024 => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFEDC53F"));
        public static SolidColorBrush Col2048 => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFEDC22D"));
    }
    class Brick : INotifyPropertyChanged
    {
        int number;
        Brush brush;
        public int Number
        {
            get => number;
            set
            {
                number = value;
                UpdateColor();
                OnPropertyChanged();
                OnPropertyChanged(nameof(GetNumber));
                OnPropertyChanged(nameof(BackgroundColor));
            }
        }

        public Brush BackgroundColor
        {
            get => brush;
            set => brush = value;
        }

        private void UpdateColor()
        {
            switch (number)
            {
                case 0:
                    brush = GetColor.Col0;
                    break;
                case 2:
                    brush = GetColor.Col2;
                    break;
                case 4:
                    brush = GetColor.Col4;
                    break;
                case 8:
                    brush = GetColor.Col8;
                    break;
                case 16:
                    brush = GetColor.Col16;
                    break;
                case 32:
                    brush = GetColor.Col32;
                    break;
                case 64:
                    brush = GetColor.Col64;
                    break;
                case 128:
                    brush = GetColor.Col128;
                    break;
                case 256:
                    brush = GetColor.Col256;
                    break;
                case 512:
                    brush = GetColor.Col512;
                    break;
                case 1024:
                    brush = GetColor.Col1024;
                    break;
                case 2048:
                    brush = GetColor.Col2048;
                    break;

                default:
                    break;
            }
        }

        public Brick()
        {
            Number = 0;
        }
        public string GetNumber => number != 0 ? number.ToString() : String.Empty;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
