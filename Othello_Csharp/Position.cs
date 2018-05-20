namespace Othello_Csharp
{
    public class Position
    {
        public readonly byte row;
        public readonly byte column;
        public static Position[,] board = new Position[8,8];

        private Position(byte row, byte column)
        {
            this.row = row;
            this.column = column;
        }

        public static void initialPos() {
            for (byte row = 0; row < 8; row++)
            {
                for (byte column = 0; column < 8; column++)
                {
                    board[row, column] = new Position(row, column);
                }
            }
        }

    }
}
