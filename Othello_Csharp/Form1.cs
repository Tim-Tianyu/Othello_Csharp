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
        int[,] board = new int[8, 8];
        Button[,] projection = new Button[8, 8];
        int[][] pos_able = new int[30][];
        int pos_able_num = 0;
        int[][][] pos_flip = new int[30][][];
        int[] pos_flip_num = new int[30];
        int[] pos_click = new int[2];
        int Player = 1;
        bool unable = false;
        public Main()
        {
            InitializeComponent();
            #region InitializeProjection
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
            for (i = 0; i < 30; i++)
            {
                pos_flip[i] = new int[20][];
            }
            Getpos(Player);
        }
        public void disableBoard()
        {
            int i;
            for (i = 0; i < pos_able_num; i++)
            {
                projection[pos_able[i][0], pos_able[i][1]].Enabled = false;
                projection[pos_able[i][0], pos_able[i][1]].BackColor = SystemColors.ButtonShadow;
            }
        }
        public void InitialBoard()
        {
            int i, j;
            for(i = 0; i < 8; i++)
            {
                for (j = 0; j < 8; j++)
                    board[i, j] = 0;
            }
            board[3, 4] = board[4, 3] = 1;
            board[4, 4] = board[3, 3] = 2;
        }
        public void RefrashBoard()
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
        public void Getpos(int PlayerX)
        {
            int PlayerY;
            int[][] pos_blank = new int[60][];
            int pos_blank_num = 0;
            int[][] pos_border = new int[32][];
            int pos_border_num = 0;
            int lean_posG1, lean_posG2, lean_negG1, lean_negG2;

            if (PlayerX == 1)
                PlayerY = 2;
            else
                PlayerY = 1;

            int i, j;
            for (i = 0; i < 8; i++)
            {
                for (j = 0; j < 8; j++)
                {
                    if (board[i, j] == 0)
                    {
                        pos_blank[pos_blank_num] = new int[] { i, j };
                        pos_blank_num++;
                    }
                }
            }

            int[] pos = new int [2];
            int row;
            int column;
            for (i = 0; i < pos_blank_num; i++)
            {
                pos = pos_blank [i];
                row = pos[0];
                column = pos[1];
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

            int flip_num = 0;
            for (i = 0; i < pos_border_num; i++)
            {
                pos = pos_border [i];
                row = pos[0];
                column = pos[1];
                #region for
                for (j = 1; j < 8 - row; j++)
                {
                    if (board[row + j, column] == PlayerY)
                    {
                        pos_flip[pos_able_num][flip_num] = new int[] { row + j, column };
                        flip_num++;
                        continue;
                    }
                    else if (board[row + j, column] == 0)
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
                for (j = 1; j < row + 1; j++)
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
                for (j = 1; j < 8 - column; j++)
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
                for (j = 1; j < column + 1; j++)
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

                if (row < column)
                {
                    lean_posG1 = row;
                    lean_posG2 = column;
                }
                else
                {
                    lean_posG1 = column;
                    lean_posG2 = row;
                }
                for (j = 1; j < lean_posG1 + 1; j++)
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
                for (j = 1; j < 8 - lean_posG2; j++)
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

                for (j = 1; j < lean_negG1; j++)
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
                for (j = 1; j < lean_negG2; j++)
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

            if (pos_able_num == 0)
            {
                if (unable)
                {
                    finish();
                }
                else
                {
                    unable = true;
                    if (Player == 1)
                        Player = 2;
                    else
                        Player = 1;
                    Getpos(Player);
                }

            }
            else
            {
                unable = false;
                for (i = 0; i < pos_able_num; i++)
                {
                    row = pos_able[i][0];
                    column = pos_able[i][1];
                    projection[row, column].Enabled = true;
                    projection[row, column].BackColor = SystemColors.ButtonFace;
                }
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            disableBoard();
            int i, j;
            Button clicked = (Button)sender;
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

        }
        public void flip()
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
            RefrashBoard();
            if (Player == 1)
                Player = 2;
            else
                Player = 1;
            pos_able_num = 0;
            Getpos(Player);
        }
        public void finish()
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
    }
}
