using System;
using System.Collections.Generic;
using System.Linq;
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

namespace GUIClient
{
    /// <summary>
    /// Interaction logic for MazeControl.xaml
    /// </summary>
    public partial class MazeControl : UserControl
    {
        private string playerPos;
        private Rectangle[][] gameBoard;
        private string serializedGame;
        private int rows;
        private int cols;
        private string initialPos;
        private string goalPos;

        private BitmapImage player;
        private BitmapImage exit;
        private SolidColorBrush blockBrush;
        private SolidColorBrush freeBrush;
        private ImageBrush playerBrush;
        private ImageBrush exitBrush;

        public int Rows
        {
            get { return this.rows; }
            set
            {
                this.rows = value;
            }
        }

        public int Cols
        {
            get { return this.cols; }
            set
            {
                this.cols = value;
            }
        }

        public string InitialPos
        {
            get { return this.initialPos; }
            set
            {
                this.initialPos = value;
            }
        }

        public string GoalPos
        {
            get { return this.goalPos; }
            set
            {
                this.goalPos = value;
                this.Draw();
            }
        }

        public string SerializedGame
        {
            get { return this.serializedGame; }
            set
            {
                this.serializedGame = value;
            }
        }

        public static readonly DependencyProperty rowsD = DependencyProperty.Register("Rows", typeof(int), typeof(MazeControl), new PropertyMetadata(RowsChanges));
        public static readonly DependencyProperty colsD = DependencyProperty.Register("Cols", typeof(int), typeof(MazeControl), new PropertyMetadata(ColsChanges));
        public static readonly DependencyProperty gameD = DependencyProperty.Register("SerializedGame", typeof(string), typeof(MazeControl), new PropertyMetadata(SerializedGameChanges));
        public static readonly DependencyProperty initD = DependencyProperty.Register("InitialPos", typeof(string), typeof(MazeControl), new PropertyMetadata(InitialPosChanges));
        public static readonly DependencyProperty goalD = DependencyProperty.Register("GoalPos", typeof(string), typeof(MazeControl), new PropertyMetadata(GoalPosChanges));

        public static void RowsChanges(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MazeControl mC = (MazeControl)d;

            mC.Rows = (int) e.NewValue;
        }

        public static void ColsChanges(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MazeControl mC = (MazeControl)d;

            mC.Cols = (int)e.NewValue;
        }

        public static void SerializedGameChanges(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MazeControl mC = (MazeControl)d;

            mC.SerializedGame = (string)e.NewValue;
        }

        public static void InitialPosChanges(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MazeControl mC = (MazeControl)d;

            mC.InitialPos = (string)e.NewValue;
        }

        public static void GoalPosChanges(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MazeControl mC = (MazeControl)d;

            mC.GoalPos = (string)e.NewValue;
        }

        public MazeControl()
        {
            InitializeComponent();
            this.player = new BitmapImage(new Uri("pack://application:,,,/GUIClient;component/Resources/player.png"));

            this.exit = new BitmapImage(new Uri("pack://application:,,,/GUIClient;component/Resources/exit.png"));

            this.playerBrush = new ImageBrush(this.player);
            this.exitBrush = new ImageBrush(this.exit);
        
            this.blockBrush = new SolidColorBrush(Colors.Black);
            this.freeBrush = new SolidColorBrush(Colors.White);
        }

        private void Draw()
        {
            int rowDel = (int)Board.Height / this.rows;
            int colDel = (int)Board.Width / this.cols;
            this.gameBoard = new Rectangle[this.rows][];
            this.serializedGame = this.serializedGame.Replace("\r\n", string.Empty);

            for(int i = 0; i < this.rows; i++)
            {
                gameBoard[i] = new Rectangle[this.cols];
                for (int j = 0; j < this.cols; j++)
                {
                    int k = (i * this.rows) + j;

                    gameBoard[i][j] = new Rectangle();
                    gameBoard[i][j].Width = colDel;
                    gameBoard[i][j].Height = rowDel;

                    switch (serializedGame[k])
                    {
                        case '1': gameBoard[i][j].Fill = blockBrush; break;
                        case '0': gameBoard[i][j].Fill = freeBrush; break;
                    }

                    Board.Children.Add(gameBoard[i][j]);
                    Canvas.SetLeft(gameBoard[i][j], j * colDel);
                    Canvas.SetTop(gameBoard[i][j], i * rowDel);
                }
            }

            // setting player on board
            string[] temp = this.initialPos.Split(',');
            this.playerPos = this.initialPos;
            int x, y;

            x = Int32.Parse(temp[0]);
            y = Int32.Parse(temp[1]);
            gameBoard[x][y].Fill = playerBrush;

            // setting exit icon on board
            temp = this.goalPos.Split(',');

            x = Int32.Parse(temp[0]);
            y = Int32.Parse(temp[1]);
            gameBoard[x][y].Fill = exitBrush;
        }

        private void MovePlayer(string direction)
        {

        }
    }
}
