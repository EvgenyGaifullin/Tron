using System;
using System.Collections.Generic;
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

using System.Windows.Threading;

namespace Tron
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly Dictionary<GridValue, ImageSource> gridValToImage = new()
        {
            { GridValue.Empty, Images.Empty },
            { GridValue.OrangePlayer, Images.TailOrange },
            { GridValue.GreenPlayer, Images.TailGreen },
        };

        private readonly Dictionary<Direction, int> dirToRotation = new()
        {
            { Direction.Up, 0 },
            { Direction.Right, 90 },
            { Direction.Down, 180 },
            { Direction.Left, 270 }
        };

        private readonly int rows = 15, cols = 15;
        private readonly Image[,] gridImages;
        private GameState gameState;
        private bool gameRunning;

        public MainWindow()
        {
            InitializeComponent();
            gridImages = SetupGrid();
            gameState = new GameState(rows, cols);
        }

        private async Task RunGame()
        {
            Draw();
            await ShowCountDown();
            Overlay.Visibility = Visibility.Hidden;
            await GameLoop();
            await ShowGameOver();
            gameState = new GameState(rows, cols);
        }

        private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Overlay.Visibility == Visibility.Visible)
            {
                e.Handled = true;
            }

            if (!gameRunning)
            {
                gameRunning = true;
                await RunGame();
                gameRunning = false;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.Left:
                    gameState.ChangeOrangeDirection(Direction.Left);
                    break;
                case Key.Right:
                    gameState.ChangeOrangeDirection(Direction.Right);
                    break;
                case Key.Up:
                    gameState.ChangeOrangeDirection(Direction.Up);
                    break;
                case Key.Down:
                    gameState.ChangeOrangeDirection(Direction.Down);
                    break;
                case Key.A:
                    gameState.ChangeGreenDirection(Direction.Left);
                    break;
                case Key.D:
                    gameState.ChangeGreenDirection(Direction.Right);
                    break;
                case Key.W:
                    gameState.ChangeGreenDirection(Direction.Up);
                    break;
                case Key.S:
                    gameState.ChangeGreenDirection(Direction.Down);
                    break;
            }
        }

        private async Task GameLoop()
        {
            while (!gameState.GameOver)
            {
                await Task.Delay(100);
                gameState.MoveOrange();
                gameState.MoveGreen();
                Draw();
            }
        }

        private Image[,] SetupGrid()
        {
            Image[,] images = new Image[rows, cols];
            GameGrid.Rows = rows;
            GameGrid.Columns = cols;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Image image = new Image
                    {
                        Source = Images.Empty,
                        RenderTransformOrigin = new Point(0.5, 0.5)
                    };

                    images[r, c] = image;
                    GameGrid.Children.Add(image);
                }
            }

            return images;
        }

        public void Draw()
        {
            DrawGrid();
            DrawHeadOrange();
            DrawHeadGreen();
            redScoreText.Text = $"ORANGE {gameState.ScoreOrange}";
            blueScoreText.Text = $"GREEN {gameState.ScoreGreen}";
        }


        public void DrawGrid()
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    GridValue gridVal = gameState.Grid[r, c];
                    gridImages[r, c].Source = gridValToImage[gridVal];
                    gridImages[r, c].RenderTransform = Transform.Identity;
                }
            }
        }

        private void DrawHeadOrange()
        {
            Position headPos = gameState.GetOrangeHeadPos();
            Image image = gridImages[headPos.Row, headPos.Col];
            image.Source = Images.HeadOrange;

            int rotation = dirToRotation[gameState.DirOrange];
            image.RenderTransform = new RotateTransform(rotation);

        }
        
        private void DrawHeadGreen()
        {
            Position headPos = gameState.GetGreenHeadPos();
            Image image = gridImages[headPos.Row, headPos.Col];
            image.Source = Images.HeadGreen;

            int rotation = dirToRotation[gameState.DirGreen];
            image.RenderTransform = new RotateTransform(rotation);

        }

        private async Task ShowCountDown()
        {
            for (int i = 3; i >= 1; i--)
            {
                OverlayText.Text = i.ToString();
                await Task.Delay(500);
            }
        }
        private async Task ShowGameOver()
        {
            await Task.Delay(1000);
            Overlay.Visibility = Visibility.Visible;
            OverlayText.Text = "Нажмите для запуска игры!";
        }
    }
}
