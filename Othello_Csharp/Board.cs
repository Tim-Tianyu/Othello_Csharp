using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello_Csharp
{
    class Board
    {
        static enum pieces
        {
            blank = 0,
            one = 1,
            two = 2
        };

        private pieces[,] layout;
        private pieces player;
        private byte step = 0;
        private Dictionary<Position, List<Position>> posAble;
        private Boolean calculated;

        public Board(pieces[,] layout, pieces player)
        {
            this.calculated = false;
            this.layout = new pieces[8, 8];
            if (layout != null && player != pieces.blank)
            {
                this.player = player;
                for (byte i = 0; i < 8; i++)
                {
                    for (byte j = 0; j < 8; j++)
                    {
                        this.layout[i,j] = layout[i,j];
                    }
                }
            }
            else
            {
                this.player = pieces.one;
                byte i, j;
                for (i = 0; i < 8; i++)
                {
                    for (j = 0; j < 8; j++)
                        this.layout[i, j] = pieces.blank;
                }
                this.layout[3, 4] = this.layout[4, 3] = pieces.one;
                this.layout[4, 4] = this.layout[3, 3] = pieces.one;
            }
        }

        public static Board Copy(Board b)
        {
            Board new = Board(b.layout,b.player);
            new.player = b.player
            //get posAble done later
        }

        public static Dictionary<Position, List<Position>>.KeyCollection getPosAble(Board b)
        {
            return b.posAble.Keys;
        }

        public static List<Position> play (Position p, Board b)
        {
            try
            {
                List<Position> flipPos = b.posAble.Item[p];
            }
            catch (KeyNotFoundException e )
            {
                Console.WriteLine("wrong position");
                throw;
            }

            b.step++;

            foreach (Position p in flipPos)
            {
                layout[p.row,p.column] = b.player;
            }

            if (step == 64)
            {
                return;//game end
            }

            if (b.player == pieces.one)
            {
                b.player = pieces.two;
            }
            else
            {
                b.player = pieces.one;
            }
            b.calculated = false;
            b.calculate();

            if (b.posAble = 0) // skip one player
            {
                if (b.player == pieces.one)
                {
                    b.player = pieces.two;
                }
                else
                {
                    b.player = pieces.one;
                }
                b.calculated = false;
                b.calculate();

                if (b.posAble = 0)
                {
                    return; //game end (special)
                }
            }
        }

        public Boolean isCalculated()
        {
            return calculated;
        }

        public static Boolean calculate(Board b)//写成static应该更省内存
        {
            if (!b.calculated)
            {
                pieces[,] layout = b.layout;
                pieces player = b.player;
                pieces opponent;
                if (player == pieces.one){
                    opponent = pieces.two;
                }else{
                    opponent = pieces.one;
                }
                Dictionary<Position, List<Position>> posAble = new Dictionary<Position,List<Position>>();
                byte row, column;
                for (row = 0; row < 8; row++)
                {
                    for (column = 0; column < 8; column++)
                    {
                        if (layout[row, column] == pieces.blank)
                        {
                            //从空格开始依次向8个方向延伸，判段该空格是否可行棋，并将每个方向上能翻的敌方棋子记录在posFilp内
                            List<Position> posFlip = new List<Position>();
                            #region  for
                            byte i;
                            Boolean flag = false;
                            List<Position> posCandidate = new List<Position>();
                            pieces thisPos;
                            for (i = 1; i < 8 - row; i++)//向下延伸
                            {
                                thisPos = layout[row + i, column];
                                if (thisPos == opponent)//遇到敌方棋子，将敌方棋子位置记录并继续在该方向延伸
                                {
                                    posCandidate.Add(Position.position ( row + i, column ));
                                    continue;
                                }
                                else if (thisPos == pieces.blank) break;//遇到空格，该方向无效
                                else if (i != 1)//是我方棋子并且不是紧靠的棋子代表该空格可以行棋，
                                {
                                    flag = true;//该方向成立
                                    break;
                                }
                                else break;//紧靠的棋子是我方棋子，该方向无效
                            }
                            if (flag)//如果该方向成立
                            {
                                posFlip.AddRange(posCandidate);//将暂时记录下来的可翻棋子永久记录下来
                                flag = false;
                            }

                            //重置
                            posCandidate.Clear();
                            for (i = 1; i < row + 1; i++)//上
                            {
                                thisPos = layout[row - i, column];
                                if (thisPos == opponent)
                                {
                                    posCandidate.Add(Position.position ( row - i, column ));
                                    continue;
                                }
                                else if (thisPos == pieces.blank) break;
                                else if (i != 1)
                                {
                                    flag = true;
                                    break;
                                }
                                else break;
                            }
                            if (flag)
                            {
                                posFlip.AddRange(posCandidate);
                                flag = false;
                            }

                            posCandidate.Clear();
                            for (i = 1; i < 8 - column; i++)//右
                            {
                                thisPos = layout[row, column + i];
                                if (thisPos == opponent)
                                {
                                    posCandidate.Add(Position.position ( row, column + i));
                                    continue;
                                }
                                else if (thisPos == pieces.blank) break;
                                else if (i != 1)
                                {
                                    flag = true;
                                    break;
                                }
                                else break;
                            }
                            if (flag)
                            {
                                posFlip.AddRange(posCandidate);
                                flag = false;
                            }

                            posCandidate.Clear();
                            for (i = 1; i < column + 1; i++)//左
                            {
                                thisPos = layout[row, column - i];
                                if (thisPos == opponent)
                                {
                                    posCandidate.Add(Position.position ( row, column - i ));
                                    continue;
                                }
                                else if (thisPos == pieces.blank) break;
                                else if (i != 1)
                                {
                                    flag = true;
                                    break;
                                }
                                else break;
                            }
                            if (flag)
                            {
                                posFlip.AddRange(posCandidate);
                                flag = false;
                            }

                            byte boundLeftUp, boundLeftDown, boundRightUp, boundRightDown;
                            if (row < column)//空格更靠右上，先碰到右边界和上边界
                            {
                                boundLeftUp = row + 1;
                                boundRightDown = 8 - column;
                            }
                            else//空格更靠左下，先碰到左边界和下边界
                            {
                                boundLeftUp = column + 1;
                                boundRightDown = 8 - row;
                            }
                            if (row < 7 - column)//空格更靠左上
                            {
                                boundLeftDown = column + 1;
                                boundRightUp = row + 1;
                            }
                            else//空格更靠右下
                            {
                                boundLeftDown = 8 - row;
                                boundRightUp = 8 - column;
                            }

                            posCandidate.Clear();
                            for (i = 1; i < boundLeftUp; i++)//左上
                            {
                                thisPos = layout[row - i, column - i];
                                if (thisPos == opponent)
                                {
                                    posCandidate.Add(Position.position ( row - i, column - i ));
                                    continue;
                                }
                                else if (thisPos == pieces.blank) break;
                                else if (i != 1)
                                {
                                    flag = true;
                                    break;
                                }
                                else break;
                            }
                            if (flag)
                            {
                                posFlip.AddRange(posCandidate);
                                flag = false;
                            }

                            posCandidate.Clear();
                            for (i = 1; i < boundRightDown; i++)//右下
                            {
                                thisPos = layout[row + i, column + i];
                                if (thisPos == opponent)
                                {
                                    posCandidate.Add(Position.position ( row + i, column + i ));
                                    continue;
                                }
                                else if (thisPos == pieces.blank) break;
                                else if (i != 1)
                                {
                                    flag = true;
                                    break;
                                }
                                else break;
                            }
                            if (flag)
                            {
                                posFlip.AddRange(posCandidate);
                                flag = false;
                            }

                            posCandidate.Clear();
                            for (i = 1; i < boundLeftDown; i++)//左下
                            {
                                thisPos = layout[row + i, column - i];
                                if (thisPos == opponent)
                                {
                                    posCandidate.Add(Position.position ( row + i, column - i ));
                                    continue;
                                }
                                else if (thisPos == pieces.blank) break;
                                else if (i != 1)
                                {
                                    flag = true;
                                    break;
                                }
                                else break;
                            }
                            if (flag)
                            {
                                posFlip.AddRange(posCandidate);
                                flag = false;
                            }

                            posCandidate.Clear();
                            for (i = 1; i < boundRightUp; i++)//右上
                            {
                                thisPos = layout[row - i, column + i];
                                if (thisPos == opponent)
                                {
                                    posCandidate.Add(Position.position ( row - i, column + i ));
                                    continue;
                                }
                                else if (thisPos == pieces.blank) break;
                                else if (i != 1)
                                {
                                    flag = true;
                                    break;
                                }
                                else break;
                            }
                            if (flag)
                            {
                                posFlip.AddRange(posCandidate);
                                flag = false;
                            }
                            #endregion
                            if (posFlip.Count != 0)
                                posAble.Add(Position.position ( row, column ), posFlip);
                        }
                    }
                }
                b.posAble = posAble;
                return b.calculated = true;
            }
            return false;
        }
    }
}
