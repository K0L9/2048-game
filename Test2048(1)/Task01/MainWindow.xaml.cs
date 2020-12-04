using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
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
        static Random rnd = new Random();
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = viewModel;
            field.ItemsSource = viewModel.bricks;
            field.DisplayMemberPath = nameof(Brick.GetNumber);
        }

        private void addOneBrick_Click(object sender, RoutedEventArgs e)
        {
            if (!AddOneBrick())
                MessageBox.Show("You are looser");
        }

        private bool AddOneBrick()
        {
            if (viewModel.bricks.Count(x => x.Number != 0) == 16)
                return false;

            bool isTrueIndex = false;
            int rndIndex = 0;

            while (!isTrueIndex)
            {
                rndIndex = rnd.Next(0, 16);
                if (viewModel.bricks[rndIndex].Number == 0)
                    isTrueIndex = true;
            }

            if (rnd.Next(100) <= 10)
                viewModel.bricks[rndIndex].Number = 4;
            else
                viewModel.bricks[rndIndex].Number = 2;

            return true;
        }

        private void moveDown_Click(object sender, RoutedEventArgs e)
        {
            int currIndex = 0;
            for (int col = 0; col < 4; col++, currIndex = col)
            {
                ObservableCollection<Brick> bricksLine = new ObservableCollection<Brick>();

                for (int i = 0; i < 4; i++, currIndex += 4)
                    bricksLine.Add(viewModel.bricks[currIndex]);

                if (bricksLine.Count(x => x.Number == 0) == 4)
                    continue;

                int clearIndex = -1;
                int currNumberInDown = -1, currIndInDown = -1;
                bool isShouldToWriteNewDownNumber = false;

                for (int i = bricksLine.Count - 1; i >= 0; i--)
                {
                    if (bricksLine[i].Number == 0)
                    {
                        if (clearIndex == -1)
                            clearIndex = i;
                        continue;
                    }
                    else
                    {
                        if (currNumberInDown == bricksLine[i].Number)
                        {
                            bricksLine[currIndInDown].Number *= 2;
                            bricksLine[i].Number = 0;
                            clearIndex = currIndInDown - 1;
                            currNumberInDown = -1;
                            currIndInDown = -1;
                            continue;
                        }
                        else
                            isShouldToWriteNewDownNumber = true;

                        if (clearIndex != -1)
                        {
                            if (isShouldToWriteNewDownNumber)
                            {
                                currNumberInDown = bricksLine[i].Number;
                                currIndInDown = clearIndex;
                                isShouldToWriteNewDownNumber = false;
                            }
                            bricksLine[clearIndex].Number = bricksLine[i].Number;
                            bricksLine[i].Number = 0;
                            clearIndex--;
                            // Баг пов'язаний з тим, що при опусканні елементів які стоять разом, вони не додаються, якщо ставити clearIndex = i, то вони не опускаються разом
                        }

                        else
                        {
                            currNumberInDown = bricksLine[i].Number;
                            currIndInDown = i;
                            isShouldToWriteNewDownNumber = false;
                        }
                    }
                }
            }
        }

        private void moveUp_Click(object sender, RoutedEventArgs e)
        {
            int currIndex = 0;
            for (int col = 0; col < 4; col++, currIndex = col)
            {
                ObservableCollection<Brick> bricksLine = new ObservableCollection<Brick>();

                for (int i = 0; i < 4; i++, currIndex += 4)
                    bricksLine.Add(viewModel.bricks[currIndex]);

                int clearIndex = -1;
                int currNumberInUp = -1, currIndInUp = -1;
                bool isShouldToWriteNewUpNumber = false;

                for (int i = 0; i < bricksLine.Count; i++)
                {
                    if (bricksLine[i].Number == 0)
                    {
                        if (clearIndex == -1)
                            clearIndex = i;
                        continue;
                    }
                    else
                    {
                        if (currNumberInUp == bricksLine[i].Number)
                        {
                            bricksLine[currIndInUp].Number *= 2;
                            bricksLine[i].Number = 0;
                            clearIndex = currIndInUp + 1;
                            currNumberInUp = -1;
                            currIndInUp = -1;
                            continue;
                        }
                        else
                            isShouldToWriteNewUpNumber = true;

                        if (clearIndex != -1)
                        {
                            if (isShouldToWriteNewUpNumber)
                            {
                                currNumberInUp = bricksLine[i].Number;
                                currIndInUp = clearIndex;
                                isShouldToWriteNewUpNumber = false;
                            }
                            bricksLine[clearIndex].Number = bricksLine[i].Number;
                            bricksLine[i].Number = 0;
                            clearIndex++;
                        }

                        else
                        {
                            currNumberInUp = bricksLine[i].Number;
                            currIndInUp = i;
                            isShouldToWriteNewUpNumber = false;
                        }
                    }
                }
            }
        }

        private void MoveLeft_Click(object sender, RoutedEventArgs e)
        {
            int currIndex = 0;
            for (int row = 0; row < 4; row++)
            {
                ObservableCollection<Brick> bricksLine = new ObservableCollection<Brick>();

                for (int i = 0; i < 4; i++)
                    bricksLine.Add(viewModel.bricks[currIndex++]);

                int clearIndex = -1;
                int currNumberInLeft = -1, currIndInLeft = -1;
                bool isShouldToWriteNewLeftNumber = false;

                for (int i = 0; i < bricksLine.Count; i++)
                {
                    if (bricksLine[i].Number == 0)
                    {
                        if (clearIndex == -1)
                            clearIndex = i;
                        continue;
                    }
                    else
                    {
                        if (currNumberInLeft == bricksLine[i].Number)
                        {
                            bricksLine[currIndInLeft].Number *= 2;
                            bricksLine[i].Number = 0;
                            clearIndex = currIndInLeft + 1;
                            currNumberInLeft = -1;
                            currIndInLeft = -1;
                            continue;
                        }
                        else
                            isShouldToWriteNewLeftNumber = true;

                        if (clearIndex != -1)
                        {
                            if (isShouldToWriteNewLeftNumber)
                            {
                                currNumberInLeft = bricksLine[i].Number;
                                currIndInLeft = clearIndex;
                                isShouldToWriteNewLeftNumber = false;
                            }
                            bricksLine[clearIndex].Number = bricksLine[i].Number;
                            bricksLine[i].Number = 0;
                            clearIndex++;
                        }

                        else
                        {
                            currNumberInLeft = bricksLine[i].Number;
                            currIndInLeft = i;
                            isShouldToWriteNewLeftNumber = false;
                        }
                    }
                }
            }
        }

        private void MoveRight_Click(object sender, RoutedEventArgs e)
        {
            int currIndex = 0;
            for (int row = 0; row < 4; row++)
            {
                ObservableCollection<Brick> bricksLine = new ObservableCollection<Brick>();

                for (int i = 0; i < 4; i++)
                    bricksLine.Add(viewModel.bricks[currIndex++]);

                if (bricksLine.Count(x => x.Number == 0) == 4)
                    continue;

                int clearIndex = -1;
                int currNumberInRight = -1, currIndInRight = -1;
                bool isShouldToWriteNewRightNumber = false;

                for (int i = bricksLine.Count - 1; i >= 0; i--)
                {
                    if (bricksLine[i].Number == 0)
                    {
                        if (clearIndex == -1)
                            clearIndex = i;
                        continue;
                    }
                    else
                    {
                        if (currNumberInRight == bricksLine[i].Number)
                        {
                            bricksLine[currIndInRight].Number *= 2;
                            bricksLine[i].Number = 0;
                            clearIndex = currIndInRight - 1;
                            currNumberInRight = -1;
                            currIndInRight = -1;
                            continue;
                        }
                        else
                            isShouldToWriteNewRightNumber = true;

                        if (clearIndex != -1)
                        {
                            if (isShouldToWriteNewRightNumber)
                            {
                                currNumberInRight = bricksLine[i].Number;
                                currIndInRight = clearIndex;
                                isShouldToWriteNewRightNumber = false;
                            }
                            bricksLine[clearIndex].Number = bricksLine[i].Number;
                            bricksLine[i].Number = 0;
                            clearIndex--;
                            // Баг пов'язаний з тим, що при опусканні елементів які стоять разом, вони не додаються, якщо ставити clearIndex = i, то вони не опускаються разом
                        }

                        else
                        {
                            currNumberInRight = bricksLine[i].Number;
                            currIndInRight = i;
                            isShouldToWriteNewRightNumber = false;
                        }
                    }
                }
            }
        }

        private void field_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.bricks[field.SelectedIndex].Number = 2;
        }
    }
    class Brick : INotifyPropertyChanged
    {
        int number;
        public int Number
        {
            get => number;
            set
            {
                number = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(GetNumber));
            }
        }

        public Brick()
        {
            number = 0;
        }
        public string GetNumber => number != 0 ? number.ToString() : String.Empty;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    class ViewModel
    {
        public ObservableCollection<Brick> bricks = new ObservableCollection<Brick>();

        public ViewModel()
        {
            for (int i = 0; i < 16; i++)
                bricks.Add(new Brick());
        }
    }
}
