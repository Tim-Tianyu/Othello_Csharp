using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace Othello_Csharp
{
    public partial class Main : Form
    {
        Board board = new Board();
        Button[,] projection = new Button[8, 8];//对应的界面上的按钮(棋盘)
        Board.pieces player = Board.pieces.one;
        Position[] record = new Position[64];
        private object Locker = new object();
        private Position target;
        AI player1 = AI.off;
        AI player2 = AI.off;
        AI current = AI.off;
        //Board.pieces AI = Board.pieces.blank;

        BackgroundWorker workerThread = new BackgroundWorker();

        public enum AI
        {
            off = 0,
            minmax = 1,
            alphabeta = 2
        }
        bool isPaused = false;

        public Main()
        {
            InitializeComponent();
            #region InitializeProjection //将按钮对象放入矩阵中
            projection[0, 0] = Pos11;
            projection[0, 1] = Pos12;
            projection[0, 2] = Pos13;
            projection[0, 3] = Pos14;
            projection[0, 4] = Pos15;
            projection[0, 5] = Pos16;
            projection[0, 6] = Pos17;
            projection[0, 7] = Pos18;
            projection[1, 0] = Pos21;
            projection[1, 1] = Pos22;
            projection[1, 2] = Pos23;
            projection[1, 3] = Pos24;
            projection[1, 4] = Pos25;
            projection[1, 5] = Pos26;
            projection[1, 6] = Pos27;
            projection[1, 7] = Pos28;
            projection[2, 0] = Pos31;
            projection[2, 1] = Pos32;
            projection[2, 2] = Pos33;
            projection[2, 3] = Pos34;
            projection[2, 4] = Pos35;
            projection[2, 5] = Pos36;
            projection[2, 6] = Pos37;
            projection[2, 7] = Pos38;
            projection[3, 0] = Pos41;
            projection[3, 1] = Pos42;
            projection[3, 2] = Pos43;
            projection[3, 3] = Pos44;
            projection[3, 4] = Pos45;
            projection[3, 5] = Pos46;
            projection[3, 6] = Pos47;
            projection[3, 7] = Pos48;
            projection[4, 0] = Pos51;
            projection[4, 1] = Pos52;
            projection[4, 2] = Pos53;
            projection[4, 3] = Pos54;
            projection[4, 4] = Pos55;
            projection[4, 5] = Pos56;
            projection[4, 6] = Pos57;
            projection[4, 7] = Pos58;
            projection[5, 0] = Pos61;
            projection[5, 1] = Pos62;
            projection[5, 2] = Pos63;
            projection[5, 3] = Pos64;
            projection[5, 4] = Pos65;
            projection[5, 5] = Pos66;
            projection[5, 6] = Pos67;
            projection[5, 7] = Pos68;
            projection[6, 0] = Pos71;
            projection[6, 1] = Pos72;
            projection[6, 2] = Pos73;
            projection[6, 3] = Pos74;
            projection[6, 4] = Pos75;
            projection[6, 5] = Pos76;
            projection[6, 6] = Pos77;
            projection[6, 7] = Pos78;
            projection[7, 0] = Pos81;
            projection[7, 1] = Pos82;
            projection[7, 2] = Pos83;
            projection[7, 3] = Pos84;
            projection[7, 4] = Pos85;
            projection[7, 5] = Pos86;
            projection[7, 6] = Pos87;
            projection[7, 7] = Pos88;
            #endregion
            Position.initialPos();
            initialBoard();
            workerThread.DoWork += workerThread_DoWork;
            workerThread.ProgressChanged += workerThread_ProgressChanged;
            workerThread.RunWorkerCompleted += workerThread_RunWorkerCompleted;
            workerThread.WorkerReportsProgress = true;
        }
        public Color getColor(Board.pieces player){
            if (player == Board.pieces.one)
                return Color.LightPink;
            else if (player == Board.pieces.two)
                return Color.LightGreen;
            else
                return Color.White;
        }

        public void initialBoard()
        {
            Board.calculate(board);
            projection[3, 4].BackColor = Color.LightPink;
            projection[4, 3].BackColor = Color.LightPink;
            projection[4, 4].BackColor = Color.LightGreen;
            projection[3, 3].BackColor = Color.LightGreen;
            enableBoard();
        }

        public void disableBoard()//将所有可以行棋的位置设为无法点击，并改变颜色，用户点击后使用
        {
            foreach (Position p in Board.getPosAble(board))
            {
                projection[p.row, p.column].Enabled = false;
                projection[p.row, p.column].BackColor = SystemColors.ButtonShadow;
            }
        }

        public void disableBoard2()// without change color
        {
            foreach (Position p in Board.getPosAble(board))
            {
                projection[p.row, p.column].Enabled = false;
                projection[p.row, p.column].BackColor = Color.LightGray;
            }
        }

        public void enableBoard()
        {
            foreach (Position p in Board.getPosAble(board))
            {
                projection[p.row, p.column].Enabled = true;
                projection[p.row, p.column].BackColor = SystemColors.ButtonFace;
            }
        }

        private void Button_Click(object sender, EventArgs e)//棋盘被点击
        {
            int i, j;
            Button clicked = (Button)sender;//被点击的按钮
            Position p = null;
            for (i = 0;i < 8;i++)
            {
                for (j = 0;j < 8; j++)
                {
                    if (projection[i,j] == clicked)
                    {
                        p = Position.board[i, j];
                        i = 7;
                        break;
                    }
                }
            }
            play(p);
        }

        private void play(Position clicked)
        {
            disableBoard();
            List<Position> posFilp = Board.play(clicked, board);
            projection[clicked.row, clicked.column].BackColor = getColor(player);

            foreach (Position p in posFilp)
            {
                projection[p.row, p.column].BackColor = getColor(player);
            }

            Board.pieces nextPlayer = Board.getCurrentPlayer(board);
            if (player == nextPlayer)
            {
                MessageBox.Show("skip the turn");
            }
            player = nextPlayer;

            if (player == Board.pieces.blank)
            {
                finish();
            }
            else
            {
                int[] score = Board.count(board);
                num1.Text = string.Format("{0:D}", score[0]);
                num2.Text = string.Format("{0:D}", score[1]);
                enableBoard();
                turnAI();
            }
        }

        private void turnAI()
        {
            if (player == Board.pieces.blank) return;
            else if (player == Board.pieces.one) current = player1;
            else if (player == Board.pieces.two) current = player2;
            if (current == AI.off) return;

            disableBoard2();
            BT_switch.Enabled = false;
            State.playAs = player;

            workerThread.RunWorkerAsync();
            //target = State.minmax(new SimpleEval(board));
            //if (target != null)
            //    play(target);
        }

        public void finish()//游戏结束
        {
            player = Board.pieces.blank;
            int one, two;
            int[] temp = Board.count(board);
            one = temp[0];
            two = temp[1];
            num1.Text = string.Format("{0:D}", one);
            num2.Text = string.Format("{0:D}", two);

            if (one > two)
            {
                MessageBox.Show("player one win");
            }
            else if (one < two)
            {
                MessageBox.Show("player two win");
            }
            else
            {
                MessageBox.Show("0");
            }
        }


        private void BT_switch_Click(object sender, EventArgs e)
        {
            isPaused = !isPaused;
            RB_p1_AB.Enabled = isPaused;
            RB_p1_MM.Enabled = isPaused;
            RB_p1_Off.Enabled = isPaused;
            RB_p2_AB.Enabled = isPaused;
            RB_p2_MM.Enabled = isPaused;
            RB_p2_Off.Enabled = isPaused;
            if (isPaused) disableBoard2();
            else enableBoard();
            turnAI();
        }

        private void Player1_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == RB_p1_Off && RB_p1_Off.Checked)
            {
                player1 = AI.off;
            }
            else if (sender == RB_p1_MM && RB_p1_MM.Checked)
            {
                player1 = AI.minmax;
            }
            else if (sender == RB_p1_AB && RB_p1_AB.Checked)
            {
                player1 = AI.alphabeta;
            }
        }

        private void Player2_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == RB_p2_Off && RB_p2_Off.Checked)
            {
                player2 = AI.off;
            }
            else if (sender == RB_p2_MM && RB_p2_MM.Checked)
            {
                player2 = AI.minmax;
            }
            else if (sender == RB_p2_AB && RB_p2_AB.Checked)
            {
                player2 = AI.alphabeta;
            }
        }

        private void workerThread_DoWork(object sender, DoWorkEventArgs e)
        {
            if (current == AI.minmax) target = State.minmax(new SimpleEval(board));
            else if (current == AI.alphabeta) target = State.alphabeta(new SimpleEval(board));
            else throw new Exception("??");
            workerThread.ReportProgress(0);
            Thread.Sleep(1000);
            
        }

        private void workerThread_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            projection[target.row, target.column].BackColor = Color.Red;
        }

        private void workerThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BT_switch.Enabled = true;
            if (target != null)
                play(target);
        }
    }
}
