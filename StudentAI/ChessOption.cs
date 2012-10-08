using System;
using System.Text;
using UvsChess;

namespace StudentAI
{
    class ChessOption
    {
        public ChessMove move { get; set; }
        public int points {get; set;}

        public ChessOption(ChessMove move, int score)
        {
            this.move = move;
            this.points = score;
        }
    }
}
