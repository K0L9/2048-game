using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

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
}
