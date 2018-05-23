using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello_Csharp
{
    abstract class State
    {
        protected Board board;
        public static Board.pieces playAs;
        public static byte init = 0;
        public static byte limit = 6;
        public State(Board board) 
        {
            this.board = board;
        }

        public Dictionary<Position, List<Position>>.KeyCollection actions()
        {
            Board.calculate(board);
            return Board.getPosAble(board);
        }

        public Board.pieces getPlayer()
        {
            return Board.getCurrentPlayer(board);
        }

        public bool isCutOff() 
        {
            return board.step == init + limit || Board.getCurrentPlayer(board) == Board.pieces.blank;
        }

        abstract public State next(Position p);

        abstract public int eval();

        public static Position minmax(State s)
        {
            State.init = s.board.step;
            if (s.isCutOff()) return null;
            if (s.getPlayer() != State.playAs) return null;
            int max = -64;
            Position action = null;
            foreach (Position a in s.actions())
            {
                State next = s.next(a);
                int value = minmaxValue(next);
                if (value > max)
                {
                    max = value;
                    action = a;
                }
            }
            return action;
        }

        private static int minmaxValue(State s)
        {
            if (s.isCutOff()) return s.eval();
            if (s.getPlayer() == State.playAs) {
                int max = -64;
                foreach (Position a in s.actions())
                {
                    int value = minmaxValue(s.next(a));
                    if (value > max)
                    {
                        max = value;
                    }
                }
                return max;
            }
            else
            {
                int min = 64;
                foreach (Position a in s.actions())
                {
                    int value = minmaxValue(s.next(a));
                    if (value < min)
                    {
                        min = value;
                    }
                }
                return min;
            }
        }
    }
}
