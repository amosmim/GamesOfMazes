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

        // add player position property and solution property

        public delegate bool NotifyMove(string direction);
        public event NotifyMove DoNotifyMove;

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
                // for setting correct player starting position
                this.playerPos = this.initialPos;
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

        public string PlayerPosition
        {
            get { return this.playerPos; }
            set {
                string oldPos = this.playerPos;
                this.playerPos = value;

                this.MovePlayer(oldPos);
                }
        }

        // Using a DependencyProperty as the backing store for PlayPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty posD = DependencyProperty.Register("PlayerPosition", typeof(string), typeof(MazeControl), new PropertyMetadata(PlayerPositionChanges));
        public static readonly DependencyProperty rowsD = DependencyProperty.Register("Rows", typeof(int), typeof(MazeControl), new PropertyMetadata(RowsChanges));
        public static readonly DependencyProperty colsD = DependencyProperty.Register("Cols", typeof(int), typeof(MazeControl), new PropertyMetadata(ColsChanges));
        public static readonly DependencyProperty gameD = DependencyProperty.Register("SerializedGame", typeof(string), typeof(MazeControl), new PropertyMetadata(SerializedGameChanges));
        public static readonly DependencyProperty initD = DependencyProperty.Register("InitialPos", typeof(string), typeof(MazeControl), new PropertyMetadata(InitialPosChanges));
        public static readonly DependencyProperty goalD = DependencyProperty.Register("GoalPos", typeof(string), typeof(MazeControl), new PropertyMetadata(GoalPosChanges));

        public static void PlayerPositionChanges(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MazeControl mC = (MazeControl)d;

            mC.PlayerPosition = (string)e.NewValue;
        }

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

            FocusManager.SetFocusedElement(this, Board);
            Keyboard.Focus(Board);
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

            int x, y;

            // setting exit icon on board
            string[] temp = this.goalPos.Split(',');

            x = Int32.Parse(temp[0]);
            y = Int32.Parse(temp[1]);
            gameBoard[x][y].Fill = exitBrush;
        }

        private void MovePlayer(string oldPos)
        {
            string[] oldPosArr = oldPos.Split(',');
            string[] newPosArr = this.playerPos.Split(',');

            int oldX = Convert.ToInt32(oldPosArr[0]);
            int oldY = Convert.ToInt32(oldPosArr[1]);
            int newX = Convert.ToInt32(newPosArr[0]);
            int newY = Convert.ToInt32(newPosArr[1]);

            // change old position to free space
            gameBoard[oldX][oldY].Fill = this.freeBrush;

            // new position to player brush
            gameBoard[newX][newY].Fill = this.playerBrush;
        }

        private void MazeControlElement_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Focus();
        }

        public void BoardKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.S:
                    this.DoNotifyMove?.Invoke("down");
                    break;
                case Key.A:
                    this.DoNotifyMove?.Invoke("left");
                    break;
                case Key.D:
                    this.DoNotifyMove?.Invoke("right");
                    break;
                case Key.W:
                    this.DoNotifyMove?.Invoke("up");
                    break;
                default:
                    return;
            }
        }
    }
}
