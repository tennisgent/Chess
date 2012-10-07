using System;
using System.Collections.Generic;
using System.Text;
using UvsChess;

namespace StudentAI
{
    public class StudentAI : IChessAI
    {
        #region IChessAI Members that are implemented by the Student

        /// <summary>
        /// The name of your AI
        /// </summary>
        public string Name
        {
#if DEBUG
            get { return "StudentAI (Debug)"; }
#else
            get { return "StudentAI"; }
#endif
        }

        /// <summary>
        /// Evaluates the chess board and decided which move to make. This is the main method of the AI.
        /// The framework will call this method when it's your turn.
        /// </summary>
        /// <param name="board">Current chess board</param>
        /// <param name="yourColor">Your color</param>
        /// <returns> Returns the best chess move the player has for the given chess board</returns>
        public ChessMove GetNextMove(ChessBoard board, ChessColor myColor)
        {
            throw (new NotImplementedException());
        }

        /// <summary>
        /// Validates a move. The framework uses this to validate the opponents move.
        /// </summary>
        /// <param name="boardBeforeMove">The board as it currently is _before_ the move.</param>
        /// <param name="moveToCheck">This is the move that needs to be checked to see if it's valid.</param>
        /// <param name="colorOfPlayerMoving">This is the color of the player who's making the move.</param>
        /// <returns>Returns true if the move was valid</returns>
        public bool IsValidMove(ChessBoard boardBeforeMove, ChessMove moveToCheck, ChessColor colorOfPlayerMoving)
        {
            throw (new NotImplementedException());
        }

        #endregion


        #region Functions implemented by Tristan and Zack

        public bool IsPiece(ChessColor myColor, ChessPiece piece)
        {
            if ((myColor == ChessColor.White && (int)piece > (int)ChessPiece.Empty) || (myColor == ChessColor.Black && (int)piece < (int)ChessPiece.Empty))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<ChessMove> GetAllPawnMoves(ChessBoard board, int X, int Y, ChessColor myColor)
        {
            List<ChessMove> pawnMoves = new List<ChessMove>();
            if (myColor == ChessColor.White)
            {
                if (Y != 0 && (board[X, Y - 1] == ChessPiece.Empty) && (board[X, Y] == ChessPiece.WhitePawn))
                {
                    // Generate a move to move my pawn 1 tile forward
                    pawnMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X, Y - 1)));
                }
                if (Y == 6 && (board[X, Y - 2] == ChessPiece.Empty) && (board[X, Y] == ChessPiece.WhitePawn))
                {
                    // Generate a move to move my pawn 1 tile forward
                    pawnMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X, Y - 2)));
                }
                if (IsPiece(ChessColor.Black, board[X - 1, Y - 1]) && (board[X, Y] == ChessPiece.WhitePawn))
                {
                    // Generate a move to move my pawn 1 tile forward
                    pawnMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X - 1, Y - 1)));
                }
                if (IsPiece(ChessColor.Black, board[X + 1, Y - 1]) && (board[X, Y] == ChessPiece.WhitePawn))
                {
                    // Generate a move to move my pawn 1 tile forward
                    pawnMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X + 1, Y - 1)));
                }
            }
            else // myColor is black
            {
                if (board[X, Y] == ChessPiece.BlackPawn)
                {

                    if (Y != 7 && (board[X, Y + 1] == ChessPiece.Empty))
                    {
                        // Generate a move to move my pawn 1 tile forward
                        pawnMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X, Y + 1)));
                    }
                    if (Y == 1 && (board[X, Y + 2] == ChessPiece.Empty))
                    {
                        // Generate a move to move my pawn 2 tiles forward
                        pawnMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X, Y + 2)));
                    }
                    if (IsPiece(ChessColor.White, board[X + 1, Y + 1]))
                    {
                        // Generate a move to move my pawn 1 tile forward
                        pawnMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X + 1, Y + 1)));
                    }
                    if (IsPiece(ChessColor.White, board[X - 1, Y + 1]))
                    {
                        // Generate a move to move my pawn 1 tile forward
                        pawnMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X - 1, Y + 1)));
                    }
                }
            }

            return pawnMoves;
        }

        public List<ChessMove> GetAllRookMoves(ChessBoard board, ChessColor myColor)
        {
            List<ChessMove> allMoves = new List<ChessMove>();

            int rookCount = 2;  // TODO Need some way of keeping track of how many rooks we have...

            for (int Y = 1; Y < ChessBoard.NumberOfRows - 1; Y++)
            {
                for (int X = 0; X < ChessBoard.NumberOfColumns; X++)
                {
                    if ((myColor == ChessColor.White && board[X, Y] == ChessPiece.WhiteRook) || (myColor == ChessColor.Black && board[X, Y] == ChessPiece.BlackRook))
                    {
                        bool up = true, right = true, down = true, left = true;

                        // spiral outward from current location while checking available moves
                        for (int i = 0; i < ChessBoard.NumberOfColumns; i++)
                        {
                            //If we should check up and the next spot isn't off the board and the next spot is empty, then add move
                            if (up && (Y - i) > 0 && board[X, Y - i] == ChessPiece.Empty)
                            {
                                allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X, Y - i)));
                            }
                            else
                            {
                                up = false;
                            }
                            //If we should check right and the next spot isn't off the board and the next spot is empty, then add move
                            if (right && (X + i) < ChessBoard.NumberOfColumns && board[X + i, Y] == ChessPiece.Empty)
                            {
                                allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X + i, Y)));
                            }
                            else
                            {
                                right = false;
                            }
                            //If we should check down and the next spot isn't off the board and the next spot is empty, then add move
                            if (down && (Y + i) < ChessBoard.NumberOfRows && board[X, Y + i] == ChessPiece.Empty)
                            {
                                allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X, Y + i)));
                            }
                            else
                            {
                                down = false;
                            }
                            //If we should check left and the next spot isn't off the board and the next spot is empty, then add move
                            if (left && (X - i) > 0 && board[X - i, Y] == ChessPiece.Empty)
                            {
                                allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X - i, Y)));
                            }
                            else
                            {
                                left = false;
                            }
                            // If we have stopped looking in all directions, then stop looping
                            if (!up && !right && !down && !left)
                            {
                                break;
                            }
                        }
                        rookCount--;
                    }
                    if (rookCount == 0)
                    {
                        return allMoves;
                    }
                }
            }

            return allMoves;
        }

        public List<ChessMove> GetAllKingMoves(ChessBoard board, ChessColor myColor)
        {
            List<ChessMove> allMoves = new List<ChessMove>();

            int kingCount = 1;  // TODO Need some way of keeping track of how many kings we have...

            for (int Y = 1; Y < ChessBoard.NumberOfRows - 1; Y++)
            {
                for (int X = 0; X < ChessBoard.NumberOfColumns; X++)
                {
                    if ((myColor == ChessColor.White && board[X, Y] == ChessPiece.WhiteKing) || (myColor == ChessColor.Black && board[X, Y] == ChessPiece.BlackKing))
                    {
                        //If we should check up and the next spot isn't off the board and the next spot is empty, then add move
                        if ((Y - 1) > 0 && board[X, Y - 1] == ChessPiece.Empty)
                        {
                            allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X, Y - i)));
                        }
                        else
                        {
                            up = false;
                        }

                    }
                }
            }

            return allMoves;
        }







        #endregion
















        #region IChessAI Members that should be implemented as automatic properties and should NEVER be touched by students.
        /// <summary>
        /// This will return false when the framework starts running your AI. When the AI's time has run out,
        /// then this method will return true. Once this method returns true, your AI should return a 
        /// move immediately.
        /// 
        /// You should NEVER EVER set this property!
        /// This property should be defined as an Automatic Property.
        /// This property SHOULD NOT CONTAIN ANY CODE!!!
        /// </summary>
        public AIIsMyTurnOverCallback IsMyTurnOver { get; set; }

        /// <summary>
        /// Call this method to print out debug information. The framework subscribes to this event
        /// and will provide a log window for your debug messages.
        /// 
        /// You should NEVER EVER set this property!
        /// This property should be defined as an Automatic Property.
        /// This property SHOULD NOT CONTAIN ANY CODE!!!
        /// </summary>
        /// <param name="message"></param>
        public AILoggerCallback Log { get; set; }

        /// <summary>
        /// Call this method to catch profiling information. The framework subscribes to this event
        /// and will print out the profiling stats in your log window.
        /// 
        /// You should NEVER EVER set this property!
        /// This property should be defined as an Automatic Property.
        /// This property SHOULD NOT CONTAIN ANY CODE!!!
        /// </summary>
        /// <param name="key"></param>
        public AIProfiler Profiler { get; set; }

        /// <summary>
        /// Call this method to tell the framework what decision print out debug information. The framework subscribes to this event
        /// and will provide a debug window for your decision tree.
        /// 
        /// You should NEVER EVER set this property!
        /// This property should be defined as an Automatic Property.
        /// This property SHOULD NOT CONTAIN ANY CODE!!!
        /// </summary>
        /// <param name="message"></param>
        public AISetDecisionTreeCallback SetDecisionTree { get; set; }
        #endregion
    }
}
