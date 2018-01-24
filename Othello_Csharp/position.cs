namespace Othello_Csharp
{
    class Position
    {
        private byte row;
        private byte column;
        private static Position[,] board = new Position[8,8];

        private Position(byte row, byte column)
        {
            this.row = row;
            this.column = column;
        }

        public static initialPos() {
            for (byte row = 0; row++; row < 8)
            {
                for (byte column = 0; column++; column < 8 )
                {
                    board[row,column] = new Position(row,column)
                }
            }
        }

        public static Position position(int r, int c) {
            return board[r,c];
        }
    }
}
