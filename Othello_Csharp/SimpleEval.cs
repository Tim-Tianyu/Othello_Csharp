using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello_Csharp
{
    class SimpleEval : State
    {
        public SimpleEval(Board b) : base(b) { }

        public override State next(Position p)
        {
            return new SimpleEval(Board.next(p, board));
        }

        public override int eval()
        {
            int[] count = Board.count(board);
            int value = count[0] - count[1];
            if (State.playAs == Board.pieces.one) return value;
            else return -value;
        }
    }
}
