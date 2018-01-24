using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Othello_Csharp
{
    public partial class Main : Form
    {
        int[,] board = new int[8, 8];//棋盘  (0，1，2) 代表 (空，player1,player2)
        Button[,] projection = new Button[8, 8];//对应的界面上的按钮（棋盘）
        int[][] pos_able = new int[30][];//可以行棋的位置
        int pos_able_num = 0;
        int[][][] pos_flip = new int[30][][];//每个行棋位置的每个方向上所能翻的敌方棋子位置
        int[] pos_flip_num = new int[30];
        int[] pos_click = new int[2];//被点击的位置
        int[][] pos_border_blank = new int[32][];//紧靠棋子的空白位置（边界）
        int pos_border_blank_num = 0;
        int Player = 1;//标注行棋选手，1或2
        int AI = 0; //0:不启用，1:AI先行 2:AI后行
        bool unable = false;//标注对方上回合是否无法行棋
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
            InitialBoard();
            RefrashBoard();
            int i;
            for (i = 0; i < 30; i++)//初始化pos_flip
            {
                pos_flip[i] = new int[20][];
            }
            Getpos(Player);//开始
        }
        public void disableBoard()//将所有可以行棋的位置设为无法点击，并改变颜色，用户点击后使用
        {
            int i;
            for (i = 0; i < pos_able_num; i++)
            {
                projection[pos_able[i][0], pos_able[i][1]].Enabled = false;
                projection[pos_able[i][0], pos_able[i][1]].BackColor = SystemColors.ButtonShadow;
            }
        }

        public void RefrashBoard()//重绘棋子位置
        {
            int i, j;
            for (i = 0; i < 8; i++)
            {
                for (j = 0; j < 8; j++)
                {
                    if (board[i, j] == 1)
                        projection[i, j].BackColor = Color.LightGreen;
                    else if (board[i, j] == 2)
                        projection[i, j].BackColor = Color.LightPink;
                }
            }
        }

        private void Button_Click(object sender, EventArgs e)//棋盘被点击
        {
            disableBoard();
            int i, j;
            Button clicked = (Button)sender;//被点击的按钮
            for (i = 0;i < 8;i++)
            {
                for (j = 0;j < 8; j++)
                {
                    if (projection[i,j] == clicked)
                    {
                        pos_click[0] = i;
                        pos_click[1] = j;
                        board[i, j] = Player;
                        i = 7;
                        break;
                    }
                }
            }
            flip();
            RefrashBoard();
            count();
            if (Player == 1)
                Player = 2;
            else
                Player = 1;
            pos_able_num = 0;
            updateborder();
            Getpos(Player);//进入另一方的回合
        }
        public void flip()//更新行棋后的棋盘
        {
            int i, j, row, column;
            for (i = 0; i < pos_able_num; i++)
            {
                if (pos_able[i][0] == pos_click[0] && pos_able[i][1] == pos_click[1])
                {
                    for (j = 0; j<pos_flip_num[i]; j++)
                    {
                        row = pos_flip[i][j][0];
                        column = pos_flip[i][j][1];
                        board[row, column] = Player;
                    }
                }
            }
        }
        public void finish()//游戏结束
        {
            int one, two;
            int[] temp = new int[2];
            temp = count();
            one = temp[0];
            two = temp[1];

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
        public int[] count()//数棋子
        {
            int one = 0, two = 0, i, j;
            for (i = 0; i < 8; i++)
            {
                for (j = 0; j < 8; j++)
                {
                    if (board[i, j] == 1)
                        one++;
                    else if (board[i, j] == 2)
                        two++;
                }
            }
            num1.Text = string.Format("{0:D}", one);
            num2.Text = string.Format("{0:D}", two);

            return new int[]{one, two};
        }
    }
}
