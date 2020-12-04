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

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (viewModel.EndOfGame())
            {
                viewModel.StartGame();
                e.Cancel = true;
                return;
            }
            else
                e.Cancel = false;
        }
    }
}
