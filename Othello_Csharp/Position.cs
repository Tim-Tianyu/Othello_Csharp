namespace Othello_Csharp
{
    class Position
    {
        public readonly byte row;
        public readonly byte column;
        private static Position[,] board = new Position[8,8];

        private Position(byte row, byte column)
        {
            this.row = row;
            this.column = column;
        }

        public static void initialPos() {
            for (byte row = 0; row++; row < 8)
            {
                for (byte column = 0; column++; column < 8 )
                {
                    board[row,column] = new Position(row,column);
                }
            }
        }

    }
}
