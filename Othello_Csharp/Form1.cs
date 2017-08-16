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
        public void InitialBoard()//设置初始的四个棋子位置，以及初始的空白边界12个
        {
            int i, j;
            for(i = 0; i < 8; i++)
            {
                for (j = 0; j < 8; j++)
                    board[i, j] = 0;
            }
            board[3, 4] = board[4, 3] = 1;
            board[4, 4] = board[3, 3] = 2;
            pos_border_blank_num = 12;
            pos_border_blank[0] = new int[] { 2, 2 };
            pos_border_blank[1] = new int[] { 2, 3 };
            pos_border_blank[2] = new int[] { 2, 5 };
            pos_border_blank[3] = new int[] { 2, 4 };
            pos_border_blank[4] = new int[] { 3, 2 };
            pos_border_blank[5] = new int[] { 3, 5 };
            pos_border_blank[6] = new int[] { 4, 2 };
            pos_border_blank[7] = new int[] { 4, 5 };
            pos_border_blank[8] = new int[] { 5, 2 };
            pos_border_blank[9] = new int[] { 5, 3 };
            pos_border_blank[10] = new int[] { 5, 4 };
            pos_border_blank[11] = new int[] { 5, 5 };
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
        public void Getpos(int PlayerX)//获得可行棋的位置
        {
            int PlayerY;
            int[][] pos_border = new int[32][];
            int pos_border_num = 0;
            int lean_posG1, lean_posG2, lean_negG1, lean_negG2;

            if (PlayerX == 1)
                PlayerY = 2;
            else
                PlayerY = 1;

            int i, j;
            int[] pos = new int [2];
            int row;
            int column;
            for (i = 0; i < pos_border_blank_num; i++)//获得所有靠近空位的敌方棋子
            {
                pos = pos_border_blank [i];
                row = pos[0];
                column = pos[1];
                //依次判断该空格的八个方向，每个方向都要先判断一下位置避免indexOutOfBound
                //如果该空格旁边有敌方棋子则将该空格位置放入pos_border作为可行棋位置（pos_able）的候选
                #region if
                if (row < 7)
                {
                    if (board[row + 1, column] == PlayerY)
                    {
                        pos_border[pos_border_num] = new int[] { row, column };
                        pos_border_num++;
                        continue;
                    }
                }

                if (row > 0)
                {
                    if (board[row - 1, column] == PlayerY)
                    {
                        pos_border[pos_border_num] = new int[] { row, column };
                        pos_border_num++;
                        continue;
                    }
                }

                if (column > 0)
                {
                    if (board[row, column - 1] == PlayerY)
                    {
                        pos_border[pos_border_num] = new int[] { row, column };
                        pos_border_num++;
                        continue;
                    }
                }

                if (column < 7)
                {
                    if (board[row, column + 1] == PlayerY)
                    {
                        pos_border[pos_border_num] = new int[] { row, column };
                        pos_border_num++;
                        continue;
                    }
                }

                if (row > 0 && column > 0)
                {
                    if (board[row - 1, column - 1] == PlayerY)
                    {
                        pos_border[pos_border_num] = new int[] { row, column };
                        pos_border_num++;
                        continue;
                    }
                }

                if (row > 0 && column < 7)
                {
                    if (board[row - 1, column + 1] == PlayerY)
                    {
                        pos_border[pos_border_num] = new int[] { row, column };
                        pos_border_num++;
                        continue;
                    }
                }
                if (row < 7 && column > 0)
                {
                    if (board[row + 1, column -1] == PlayerY)
                    {
                        pos_border[pos_border_num] = new int[] { row, column };
                        pos_border_num++;
                        continue;
                    }
                }
                if (row < 7 && column < 7)
                {
                    if (board[row + 1, column + 1] == PlayerY)
                    {
                        pos_border[pos_border_num] = new int[] { row, column };
                        pos_border_num++;
                        continue;
                    }
                }
                #endregion
            }
            Console.WriteLine(pos_border_num);
            Console.Read();

            int flip_num = 0;
            //从空格开始依次向8个方向延伸，判段该空格是否可行棋，并将每个方向上能翻的敌方棋子记录在pos_filp内
            for (i = 0; i < pos_border_num; i++)
            {
                pos = pos_border [i];
                row = pos[0];
                column = pos[1];
                #region for
                for (j = 1; j < 8 - row; j++)//向上延伸
                {
                    if (board[row + j, column] == PlayerY)//遇到敌方棋子，将敌方棋子位置记录并继续在该方向延伸
                    {
                        pos_flip[pos_able_num][flip_num] = new int[] { row + j, column };
                        flip_num++;
                        continue;
                    }
                    else if (board[row + j, column] == 0)//遇到空格，该方向无效
                        break;
                    else if (j != 1)//是我方棋子并且不是紧靠的棋子代表该空格可以行棋，
                    {
                        pos_able[pos_able_num] = new int[] { row, column };
                        pos_flip_num[pos_able_num] = flip_num;
                        pos_able_num++;
                        break;
                    }
                    else//紧靠的棋子是我方棋子，该方向无效
                        break;
                }
                flip_num = 0;//重置，开始下一个方向上的延伸
                for (j = 1; j < row + 1; j++)//向下延伸
                {
                    if (board[row - j, column] == PlayerY)
                    {
                        pos_flip[pos_able_num][flip_num] = new int[] { row - j, column };
                        flip_num++;
                        continue;
                    }
                    else if (board[row - j, column] == 0)
                        break;
                    else if (j != 1)
                    {
                        pos_able[pos_able_num] = new int[] { row, column };
                        pos_flip_num[pos_able_num] = flip_num;
                        pos_able_num++;
                        break;
                    }
                    else
                        break;
                }
                flip_num = 0;
                for (j = 1; j < 8 - column; j++)//向右延伸
                {
                    if (board[row, column + j] == PlayerY)
                    {
                        pos_flip[pos_able_num][flip_num] = new int[] { row, column + j };
                        flip_num++;
                        continue;
                    }
                    else if (board[row, column + j] == 0)
                        break;
                    else if (j != 1)
                    {
                        pos_able[pos_able_num] = new int[] { row, column };
                        pos_flip_num[pos_able_num] = flip_num;
                        pos_able_num++;
                        break;
                    }
                    else
                        break;
                }
                flip_num = 0;
                for (j = 1; j < column + 1; j++)//向左延伸
                {
                    if (board[row, column - j] == PlayerY)
                    {
                        pos_flip[pos_able_num][flip_num] = new int[] { row, column - j };
                        flip_num++;
                        continue;
                    }
                    else if (board[row, column - j] == 0)
                        break;
                    else if (j != 1)
                    {
                        pos_able[pos_able_num] = new int[] { row, column };
                        pos_flip_num[pos_able_num] = flip_num;
                        pos_able_num++;
                        break;
                    }
                    else
                        break;
                }
                flip_num = 0;

                //斜率为-1的方向上的延伸
                if (row < column)//空格更靠左上，先碰到左边界和上边界
                {
                    lean_posG1 = row;
                    lean_posG2 = column;
                }
                else//空格更靠右下，先碰到右边界和下边界
                {
                    lean_posG1 = column;
                    lean_posG2 = row;
                }
                for (j = 1; j < lean_posG1 + 1; j++)//向右上延伸
                {
                    if (board[row - j, column - j] == PlayerY)
                    {
                        pos_flip[pos_able_num][flip_num] = new int[] { row - j, column - j };
                        flip_num++;
                        continue;
                    }
                    else if (board[row - j, column - j] == 0)
                        break;
                    else if (j != 1)
                    {
                        pos_able[pos_able_num] = new int[] { row, column };
                        pos_flip_num[pos_able_num] = flip_num;
                        pos_able_num++;
                        break;
                    }
                    else
                        break;
                }
                flip_num = 0;
                for (j = 1; j < 8 - lean_posG2; j++)//向左下
                {
                    if (board[row + j, column + j] == PlayerY)
                    {
                        pos_flip[pos_able_num][flip_num] = new int[] { row + j, column + j };
                        flip_num++;
                        continue;
                    }
                    else if (board[row + j, column + j] == 0)
                        break;
                    else if (j != 1)
                    {
                        pos_able[pos_able_num] = new int[] { row, column };
                        pos_flip_num[pos_able_num] = flip_num;
                        pos_able_num++;
                        break;
                    }
                    else
                        break;
                }
                flip_num = 0;

                if (8 - row < column +1)
                {
                    lean_negG1 = 8-row;
                }
                else
                {
                    lean_negG1 = column+1;
                }

                for (j = 1; j < lean_negG1; j++)//右下
                {
                    if (board[row + j, column - j] == PlayerY)
                    {
                        pos_flip[pos_able_num][flip_num] = new int[] { row + j, column - j };
                        flip_num++;
                        continue;
                    }
                    else if (board[row + j, column - j] == 0)
                        break;
                    else if (j != 1)
                    {
                        pos_able[pos_able_num] = new int[] { row, column };
                        pos_flip_num[pos_able_num] = flip_num;
                        pos_able_num++;
                        break;
                    }
                    else
                        break;
                }
                flip_num = 0;

                if (8 - column  < row + 1)
                {
                    lean_negG2 = 8 - column;
                }
                else
                {
                    lean_negG2 = row + 1;
                }
                for (j = 1; j < lean_negG2; j++)//左上
                {
                    if (board[row - j, column + j] == PlayerY)
                    {
                        pos_flip[pos_able_num][flip_num] = new int[] { row - j, column + j };
                        flip_num++;
                        continue;
                    }
                    else if (board[row - j, column + j] == 0)
                        break;
                    else if (j != 1)
                    {
                        pos_able[pos_able_num] = new int[] { row, column };
                        pos_flip_num[pos_able_num] = flip_num;
                        pos_able_num++;
                        break;
                    }
                    else
                        break;
                }
                flip_num = 0;
                #endregion
            }

            if (pos_able_num == 0)//我方无法行动
            {
                if (unable)//双方都无法行动（敌方上回合无法行动，我方这回合无法行动，陷入江局）
                {
                    finish();//结束游戏
                }
                else//只是我方无法行动
                {
                    unable = true;//记录该回合的无法行动
                    if (Player == 1)
                        Player = 2;
                    else
                        Player = 1;
                    Getpos(Player);//直接进入另一方的回合
                }

            }
            else//可以行动
            {
                unable = false;//记录该回合可以行动
                for (i = 0; i < pos_able_num; i++)
                {
                    row = pos_able[i][0];
                    column = pos_able[i][1];
                    projection[row, column].Enabled = true;//在界面上标出可以行动的位置
                    projection[row, column].BackColor = SystemColors.ButtonFace;
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

        public int[][] updateborder()//行棋后更新边界
        {
            int[][] temp = new int[8][];
            int temp_num = 0;

            int[][] pos_new_border_blank = new int[32][];//新边界
            int pos_new_border_blank_num = 0;

            int row_click = pos_click[0], column_click = pos_click[1];
            for (int i = 0; i < pos_border_blank_num; i++) //从边界中筛除掉新增的棋子位置
            {
                int row = pos_border_blank[i][0], column = pos_border_blank[i][1];
                if (row != row_click || column != column_click)
                {
                    pos_new_border_blank[pos_new_border_blank_num] = new int[] { row, column };
                    pos_new_border_blank_num++;
                }
            }
            #region 新增棋子八个方向上判断是否为空格,空格位置暂存入temp
            if (row_click<7 && column_click <7 && board[row_click + 1, column_click + 1] == 0)
            {
                temp[temp_num] = new int[] { row_click+1, column_click+1};
                temp_num++;
            }
            if (row_click < 7 && column_click >0 && board[row_click + 1, column_click - 1] == 0)
            {
                temp[temp_num] = new int[] { row_click+1, column_click-1 };
                temp_num++;
            }
            if (row_click < 7 && board[row_click + 1, column_click] == 0)
            {
                temp[temp_num] = new int[] { row_click+1, column_click };
                temp_num++;
            }
            if (row_click >0 && column_click < 7 && board[row_click - 1, column_click + 1] == 0)
            {
                temp[temp_num] = new int[] { row_click-1, column_click+1 };
                temp_num++;
            }
            if (row_click >0 && column_click >0 && board[row_click - 1, column_click - 1] == 0)
            {
                temp[temp_num] = new int[] { row_click-1, column_click-1 };
                temp_num++;
            }
            if (row_click >0 && board[row_click - 1, column_click] == 0)
            {
                temp[temp_num] = new int[] { row_click-1, column_click };
                temp_num++;
            }
            if (column_click <7 && board[row_click, column_click + 1] == 0)
            {
                temp[temp_num] = new int[] { row_click, column_click+1 };
                temp_num++;
            }
            if (column_click >0 && board[row_click, column_click - 1] == 0)
            {
                temp[temp_num] = new int[] { row_click, column_click-1 };
                temp_num++;
            }
            #endregion

            for (int i = 0; i < temp_num; i++){
                Boolean flag = true;//识别空格位置是否不存在于新边界里
                int row = temp[i][0], column = temp[i][1];
                for (int j = 0; j < pos_new_border_blank_num; j++)
                {
                    if (row == pos_new_border_blank[j][0] && column == pos_new_border_blank[j][1])
                    {
                        flag = false;//存在于新边界中
                        break;
                    }
                }
                if (flag)//不存在于新边界中
                {
                    pos_new_border_blank[pos_new_border_blank_num] = new int[] { row, column };
                    pos_new_border_blank_num++;
                }
            }

            pos_border_blank_num = pos_new_border_blank_num;
            pos_border_blank = pos_new_border_blank;
            return pos_new_border_blank;
        }
    }
}
