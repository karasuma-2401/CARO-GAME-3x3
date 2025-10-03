using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CaroGame
{
    public partial class MainWindow : Window
    {
        private const int size = 3;
        private Button[,] buttons = new Button[size, size];
        private int[,] board = new int[size, size];
        private bool isXTurn = true; // true = X, false = O

        public MainWindow()
        {
            InitializeComponent();
            InitBoard();
        }

        private void InitBoard()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Button btn = new Button
                    {
                        FontSize = 18,
                        FontWeight = FontWeights.Bold,
                        Background = Brushes.White
                    };
                    btn.Click += Btn_Click;
                    buttons[i, j] = btn;
                    BoardGrid.Children.Add(btn);
                }
            }
        }
        static int cnt = 0;
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int row = -1, col = -1;

            // tìm vị trí trong mảng
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (buttons[i, j] == btn)
                    {
                        row = i; col = j;
                        break;
                    }

            if (board[row, col] != 0) return; // đã có quân

            if (isXTurn)
            {
                btn.Content = "X";
                //btn.FontFamily = new FontFamily("TimeNewsRoman");
                btn.Foreground = Brushes.Red;
                board[row, col] = 1;
            }
            else
            {
                btn.Content = "O";
                btn.Foreground = Brushes.Blue;
                board[row, col] = 2;
            }
            cnt++;
            if (cnt >= 5)
            {
                if (CheckWin(row, col))
                {
                    MessageBox.Show((isXTurn ? "X" : "O") + " wins!");
                    ResetBoard();
                    return;
                }
                else if (cnt == size * size)
                {
                    MessageBox.Show("It's a draw!");
                    ResetBoard();
                    cnt = 0;
                    return;
                }
            }
            isXTurn = !isXTurn;
        }

        private bool CheckWin(int r, int c)
        {
            int player = board[r, c];
            int[][] directions =
            {
                new []{0,1}, // ngang
                new []{1,0}, // dọc
                new []{1,1}, // chéo chính
                new []{1,-1} // chéo phụ
            };

            foreach (var d in directions)
            {
                int count = 1;
                count += Count(r, c, d[0], d[1], player);
                count += Count(r, c, -d[0], -d[1], player);
                if (count >= 3) return true;
            }
            return false;
        }

        private int Count(int r, int c, int dr, int dc, int player)
        {
            int cnt = 0;
            while (true)
            {
                r += dr; c += dc;
                if (r < 0 || r >= size || c < 0 || c >= size) break;
                if (board[r, c] == player) cnt++;
                else break;
            }
            return cnt;
        }

        private void ResetBoard()
        {
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    board[i, j] = 0;
                    buttons[i, j].Content = "";
                }
            isXTurn = true;
        }
    }
}
