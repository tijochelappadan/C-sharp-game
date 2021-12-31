using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameWindows.Snake
{
    public partial class ClassicSnake : Form
    {
        private List<Circle> snake = new List<Circle>();
        private Circle food = new Circle();

        int imaxWidth, imaxHeight, iScore, iHighScore = 0, iLevel;
        Random rand = new Random();
        bool bGoLeft, bGoRight, bGoUp, bGoDown, bPause, bGameOver;

        public ClassicSnake()
        {
            InitializeComponent();

            new Settings();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ClassicSnake_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                bGoLeft = true;
            }

            if (e.KeyCode == Keys.S)
            {
                bGoRight = true;
            }

            if (e.KeyCode == Keys.W)
            {
                bGoUp = true;
            }

            if (e.KeyCode == Keys.Z)
            {
                bGoDown = true;
            }
        }

        private void ClassicSnake_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                bGoLeft = false;
            }

            if (e.KeyCode == Keys.S)
            {
                bGoRight = false;
            }

            if (e.KeyCode == Keys.W)
            {
                bGoUp = false;
            }

            if (e.KeyCode == Keys.Z)
            {
                bGoDown = false;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void tGametimer_Tick(object sender, EventArgs e)
        {
            // set direction of the snake
            if (bGoLeft)
            {
                Settings.strDirections = "left";
            }

            if (bGoRight)
            {
                Settings.strDirections = "right";
            }

            if (bGoDown)
            {
                Settings.strDirections = "down";
            }

            if (bGoUp)
            {
                Settings.strDirections = "up";
            }            
            // end

            // set movement of the snake
            for (int i = snake.Count - 1; i >= 0; i--)
            {
                if (i == 0) // snake head movement
                {
                    switch (Settings.strDirections)
                    {
                        case "left":
                            snake[i].iXPos--;
                            break;
                        case "right":
                            snake[i].iXPos++;
                            break;
                        case "down":
                            snake[i].iYPos++;
                            break;
                        case "up":
                            snake[i].iYPos--;
                            break;                      
                    }

                    // snake move to corner to corner
                    if (snake[i].iXPos < 0)
                    {
                        snake[i].iXPos = imaxWidth;
                    }

                    if (snake[i].iXPos > imaxWidth)
                    {
                        snake[i].iXPos = 0;
                    }

                    if (snake[i].iYPos < 0)
                    {
                        snake[i].iYPos = imaxHeight;
                    }

                    if (snake[i].iYPos > imaxHeight)
                    {
                        snake[i].iYPos = 0;
                    }
                    // end

                    // eat food
                    if (snake[i].iXPos == food.iXPos && snake[i].iYPos == food.iYPos)
                    {
                        EatFood();
                    }
                    // end

                    // snake head hit any part of the body
                    for (int j = 1; j < snake.Count; j++)
                    {
                        if (snake[i].iXPos == snake[j].iXPos && snake[i].iYPos == snake[j].iYPos)
                        {
                            GameOver();
                        }
                    }
                    // end
                }
                else // snake body movement
                {
                    snake[i].iXPos = snake[i - 1].iXPos;
                    snake[i].iYPos = snake[i - 1].iYPos;
                }
            }
            // end

            pbGameCanvas.Invalidate();
        }

        private void pbGameCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            Brush snakecolor;

            for (int i = 0; i < snake.Count; i++)
            {
                if (i == 0) // set snake head color
                {
                    snakecolor = Brushes.SteelBlue;
                }
                else // set snake body color
                {
                    snakecolor = Brushes.DarkGreen;
                }

                canvas.FillEllipse(snakecolor, new Rectangle
                    (snake[i].iXPos * Settings.iWidth, 
                    snake[i].iYPos * Settings.iHeight, 
                    Settings.iWidth, Settings.iHeight
                    ));
            }

            // set food
            canvas.FillEllipse(Brushes.DarkRed, new Rectangle
                (food.iXPos * Settings.iWidth, 
                food.iYPos * Settings.iHeight, 
                Settings.iWidth, Settings.iHeight
                ));
        }

        private void RestartGame()
        {
            imaxWidth = pbGameCanvas.Width / Settings.iWidth - 1;
            imaxHeight = pbGameCanvas.Height / Settings.iHeight - 1;
            
            snake.Clear();
            btnStart.Enabled = false;
            iLevel = 1;
            lblLevel.Text = "Level : " + Environment.NewLine + iLevel;
            iScore = 0;
            lblScore.Text = "Score : " + Environment.NewLine + iScore;         
            lblHighScore.Text = "High Score : " + Environment.NewLine + iHighScore;
            bPause = false;
            bGameOver = false;
            tGametimer.Interval = 300;

            // set head of the snake
            Circle snakehead = new Circle { iXPos = 10, iYPos = 5 };
            snake.Add(snakehead);
            // end

            // set body of the snake
            for (int i = 0; i < 10; i++)
            {
                Circle snakebody = new Circle();
                snake.Add(snakebody);
            }
            // end

            // set food for snake
            food = new Circle { iXPos = rand.Next(2, imaxWidth), iYPos = rand.Next(2, imaxHeight) };
            tGametimer.Start();
            // end
        }

        private void EatFood()
        {
            iScore += 1;
            lblScore.Text = "Score : " + Environment.NewLine + iScore;

            // set level
            iLevel = (iScore / 10) + 1;
            lblLevel.Text = "Level : " + Environment.NewLine + iLevel;
            tGametimer.Interval = iLevel <= 6 ? (300 - ((iLevel - 1) * 50)) : 0;
            // end

            Circle snakebody = new Circle
            {
                iXPos = snake[snake.Count - 1].iXPos,
                iYPos = snake[snake.Count - 1].iYPos
            };

            snake.Add(snakebody);
            food = new Circle { iXPos = rand.Next(2, imaxWidth), iYPos = rand.Next(2, imaxHeight) };
        }

        private void GameOver()
        {
            tGametimer.Stop();            
            bGameOver = true;

            if (iScore > iHighScore)
            {
                iHighScore = iScore;
                lblHighScore.Text = "High Score" + Environment.NewLine + iHighScore;
            }

            MessageBox.Show("Game Over! Score : " + iScore, "Classic Snake", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnStart.Enabled = true;
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (!bGameOver)
            {
                if (bPause)
                {
                    tGametimer.Start();
                    bPause = false;
                }
                else
                {
                    tGametimer.Stop();
                    bPause = true;
                }
            }
        }
    }
}
