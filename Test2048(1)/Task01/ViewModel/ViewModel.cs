using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace Task01
{
    class ViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Brick> bricks = new ObservableCollection<Brick>();
        static Random rnd = new Random();
        private int score = 0;
        private int highScore = -1;
        public int Score
        {
            get => score;
            set
            {
                score = value;
                OnPropertyChanged();
            }
        }
        public string HighScore
        {
            get
            {
                if (highScore == -1)
                    return "-";

                return highScore.ToString();
            }
            set
            {
                highScore = int.Parse(value);
                OnPropertyChanged();
            }
        }

        public ViewModel()
        {
            for (int i = 0; i < 16; i++)
                bricks.Add(new Brick());

            StartGame();
        }

        public void WriteHighScore()
        {
            File.WriteAllText("HighScore.txt", highScore.ToString());
        }
        public void ReadHighScore()
        {
            if (!File.Exists("HighScore.txt"))
                HighScore = "-1";

            int newHighScore = -1;
            try
            {
                newHighScore = int.Parse(File.ReadAllText("HighScore.txt"));
            }
            catch (Exception) { }
            finally
            {
                HighScore = newHighScore.ToString();
            }
        }

        public bool AddOneBrick()
        {
            if (bricks.Count(x => x.Number != 0) == 16)
                return false;

            bool isTrueIndex = false;
            int rndIndex = 0;

            while (!isTrueIndex)
            {
                rndIndex = rnd.Next(0, 16);
                if (bricks[rndIndex].Number == 0)
                    isTrueIndex = true;
            }

            if (rnd.Next(100) <= 10)
                bricks[rndIndex].Number = 4;
            else
                bricks[rndIndex].Number = 2;

            bricks[rndIndex].IsNewBrick = true;

            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private bool MoveDownAndRight(ObservableCollection<Brick> bricksLine)
        {
            bool isOneMove = false;
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
                        bricksLine[currIndInDown].IsNewBrick = true;
                        Score += bricksLine[currIndInDown].Number;
                        bricksLine[i].Number = 0;
                        clearIndex = currIndInDown - 1;
                        currNumberInDown = -1;
                        currIndInDown = -1;
                        isOneMove = true;
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
                        isOneMove = true;
                        clearIndex--;
                    }

                    else
                    {
                        currNumberInDown = bricksLine[i].Number;
                        currIndInDown = i;
                        isShouldToWriteNewDownNumber = false;
                    }
                }
            }

            return isOneMove;
        }
        private bool MoveLeftAndUp(ObservableCollection<Brick> bricksLine)
        {
            bool isOneMove = false;
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
                        bricksLine[currIndInUp].IsNewBrick = true;
                        Score += bricksLine[currIndInUp].Number;
                        bricksLine[i].Number = 0;
                        clearIndex = currIndInUp + 1;
                        currNumberInUp = -1;
                        currIndInUp = -1;
                        isOneMove = true;
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
                        isOneMove = true;
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
            return isOneMove;
        }

        public void SetNotNewBricks()
        {
            foreach (var el in bricks)
                el.IsNewBrick = false;
        }

        public bool MoveRight()
        {
            bool isOneMove = false;
            int currIndex = 0;
            for (int row = 0; row < 4; row++)
            {
                ObservableCollection<Brick> bricksLine = new ObservableCollection<Brick>();

                for (int i = 0; i < 4; i++)
                    bricksLine.Add(bricks[currIndex++]);

                if (bricksLine.Count(x => x.Number == 0) == 4)
                    continue;

                if (MoveDownAndRight(bricksLine))
                    isOneMove = true;
            }

            return isOneMove;
        }
        public bool MoveLeft()
        {
            bool isOneMove = false;
            int currIndex = 0;
            for (int row = 0; row < 4; row++)
            {
                ObservableCollection<Brick> bricksLine = new ObservableCollection<Brick>();

                for (int i = 0; i < 4; i++)
                    bricksLine.Add(bricks[currIndex++]);

                if (MoveLeftAndUp(bricksLine))
                    isOneMove = true;
            }
            return isOneMove;
        }
        public bool MoveUp()
        {
            bool isOneMove = false;
            int currIndex = 0;
            for (int col = 0; col < 4; col++, currIndex = col)
            {
                ObservableCollection<Brick> bricksLine = new ObservableCollection<Brick>();

                for (int i = 0; i < 4; i++, currIndex += 4)
                    bricksLine.Add(bricks[currIndex]);

                if (MoveLeftAndUp(bricksLine))
                    isOneMove = true;
            }

            return isOneMove;
        }
        public bool MoveDown()
        {
            bool isOneMove = false;
            int currIndex = 0;
            for (int col = 0; col < 4; col++, currIndex = col)
            {
                ObservableCollection<Brick> bricksLine = new ObservableCollection<Brick>();

                for (int i = 0; i < 4; i++, currIndex += 4)
                    bricksLine.Add(bricks[currIndex]);

                if (bricksLine.Count(x => x.Number == 0) == 4)
                    continue;

                if (MoveDownAndRight(bricksLine))
                    isOneMove = true;
            }
            return isOneMove;
        }

        public bool CheckIsEnd()
        {
            if (bricks.Count(x => x.Number == 0) > 0)
                return false;

            List<Brick> bricksLine = new List<Brick>();

            int currentIndex = 0;
            for (int col = 0; col < 4; col++, currentIndex = col)
            {
                bricksLine.Clear();

                for (int i = 0; i < 4; i++, currentIndex += 4)
                    bricksLine.Add(bricks[currentIndex]);

                for (int i = 0; i < 3; i++)
                    if (bricksLine[i].Number == bricksLine[i + 1].Number)
                        return false;
            }

            currentIndex = 0;
            for (int row = 0; row < 4; row++)
            {
                bricksLine.Clear();

                for (int i = 0; i < 4; i++)
                    bricksLine.Add(bricks[currentIndex++]);

                for (int i = 0; i < 3; i++)
                    if (bricksLine[i].Number == bricksLine[i + 1].Number)
                        return false;
            }

            return true;
        }
        public bool CheckIsWin()
        {
            foreach (var el in bricks)
                if (el.Number == 2048)
                    return true;

            return false;
        }

        public bool EndOfGame()
        {
            int highScoreThis = 0;
            try
            {
                highScoreThis = int.Parse(HighScore);
            }
            catch (Exception) { }
            finally
            {
                if (Score > highScoreThis)
                {
                    MessageBox.Show("NEW HIGH RECORD", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    highScore = score;
                    WriteHighScore();
                }
            }

            if (MessageBox.Show("Do you want to play again?", "Choose", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                return true;
            else
                return false;
        }
        public void StartGame()
        {
            Score = 0;
            foreach (var el in bricks)
                el.Number = 0;
            AddOneBrick();
            AddOneBrick();
            AddOneBrick();
            ReadHighScore();
        }
        public bool EscapeGame()
        {
            if (EndOfGame())
                StartGame();
            return false;
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
        bool isNewBrick = false;
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
                OnPropertyChanged(nameof(isNewBrick));
            }
        }
        public Brush BackgroundColor
        {
            get => brush;
            set => brush = value;
        }
        public bool IsNewBrick
        {
            get => isNewBrick;
            set
            {
                isNewBrick = value;
                OnPropertyChanged();
            }
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
