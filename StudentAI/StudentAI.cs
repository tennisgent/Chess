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

        List<ChessMove> GetAllMoves(ChessBoard currentBoard, ChessColor myColor)
        {
            List<ChessMove> allMoves = new List<ChessMove>();
            for (int Y = 1; Y < ChessBoard.NumberOfRows - 1; Y++)
            {
                for (int X = 0; X < ChessBoard.NumberOfColumns; X++)
                {
                    switch (currentBoard[X, Y])
                    {
                        case ChessPiece.BlackPawn:
                        case ChessPiece.WhitePawn:
                            allMoves = GetAllPawnMoves(allMoves, currentBoard, X, Y, myColor);
                            break;
                        case ChessPiece.BlackRook:
                        case ChessPiece.WhiteRook:
                            allMoves = GetAllRookMoves(allMoves, currentBoard, X, Y, myColor);
                            break;
                        case ChessPiece.BlackKnight:
                        case ChessPiece.WhiteKnight:
                            //allMoves = GetAllKnightMoves(allMoves, currentBoard, X, Y, myColor);
                            break;
                        case ChessPiece.BlackBishop:
                        case ChessPiece.WhiteBishop:
                            //allMoves = GetAllBishopMoves(allMoves, currentBoard, X, Y, myColor);
                            break;
                        case ChessPiece.BlackQueen:
                        case ChessPiece.WhiteQueen:
                            allMoves = GetAllRookMoves(allMoves, currentBoard, X, Y, myColor);
                            //allMoves = GetAllBishopMoves(allMoves, currentBoard, X, Y, myColor);
                            break;
                        case ChessPiece.BlackKing:
                        case ChessPiece.WhiteKing:
                            allMoves = GetAllRookMoves(allMoves, currentBoard, X, Y, myColor, 1);
                            //allMoves = GetAllBishopMoves(allMoves, currentBoard, X, Y, myColor, 1);
                            break;
                        default:
                            break;
                    }
                }
            }


            return null;
        }


        public bool IsColor(ChessColor myColor, ChessPiece piece)
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

        public bool IsNotOffBoard(int X, int Y)
        {
            return (X > 0 && X < ChessBoard.NumberOfColumns) && (Y > 0 && Y < ChessBoard.NumberOfRows);
        }

        public int GetPoints(ChessPiece piece)
        {
            switch (piece)
            {
                case ChessPiece.BlackPawn:
                case ChessPiece.WhitePawn:
                    return 1;
                case ChessPiece.BlackRook:
                case ChessPiece.WhiteRook:
                    return 3;
                case ChessPiece.BlackKnight:
                case ChessPiece.WhiteKnight:
                    return 5;
                case ChessPiece.BlackBishop:
                case ChessPiece.WhiteBishop:
                    return 7;
                case ChessPiece.BlackQueen:
                case ChessPiece.WhiteQueen:
                    return 10;
                case ChessPiece.BlackKing:
                case ChessPiece.WhiteKing:
                    return 1000000;
                default:
                    return 0;
            }
        }






        public List<ChessMove> GetAllPawnMoves(List<ChessMove> allMoves, ChessBoard board, int X, int Y, ChessColor myColor)
        {
            
            if (myColor == ChessColor.White)
            {
                if (IsNotOffBoard(X,Y-1) && (board[X, Y - 1] == ChessPiece.Empty) && (board[X, Y] == ChessPiece.WhitePawn))
                {
                    // Generate a move to move my pawn 1 tile forward
                    allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X, Y - 1)));
                }
                if (Y == 6 && (board[X, Y - 2] == ChessPiece.Empty) && (board[X, Y] == ChessPiece.WhitePawn))
                {
                    // Generate a move to move my pawn 2 tiles forward
                    allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X, Y - 2)));
                }
                if (IsNotOffBoard(X-1,Y-1) && IsColor(ChessColor.Black, board[X - 1, Y - 1]) && (board[X, Y] == ChessPiece.WhitePawn))
                {
                    // Generate a move to take a piece to the left
                    allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X - 1, Y - 1)));
                }
                if (IsNotOffBoard(X+1,Y-1) && IsColor(ChessColor.Black, board[X + 1, Y - 1]) && (board[X, Y] == ChessPiece.WhitePawn))
                {
                    // Generate a move to take a piece to the right
                    allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X + 1, Y - 1)));
                }
            }
            else // myColor is black
            {
                if (board[X, Y] == ChessPiece.BlackPawn)
                {
                    if (IsNotOffBoard(X, Y+1) && (board[X, Y + 1] == ChessPiece.Empty))
                    {
                        // Generate a move to move my pawn 1 tile forward
                        allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X, Y + 1)));
                    }
                    if (Y == 1 && (board[X, Y + 2] == ChessPiece.Empty))
                    {
                        // Generate a move to move my pawn 2 tiles forward
                        allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X, Y + 2)));
                    }
                    if (IsNotOffBoard(X+1,Y+1) && IsColor(ChessColor.White, board[X + 1, Y + 1]))
                    {
                        // Generate a move to take a piece to the right
                        allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X + 1, Y + 1)));
                    }
                    if (IsNotOffBoard(X-1,Y+1) && IsColor(ChessColor.White, board[X - 1, Y + 1]))
                    {
                        // Generate a move to take a piece to the left
                        allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X - 1, Y + 1)));
                    }
                }
            }

            return allMoves;
        }

        public List<ChessMove> GetAllRookMoves(List<ChessMove> allMoves, ChessBoard board, int X, int Y, ChessColor myColor, int maxDist = ChessBoard.NumberOfColumns-1)
        {
            bool up = true, right = true, down = true, left = true;

            // spiral outward from current location while checking available moves
            for (int i = 1; i <= maxDist; i++)
            {
                //If we should check up and the next spot isn't off the board and the next spot is empty || opposite color, then add move
                if (up && IsNotOffBoard(X, Y - i) && !IsColor(myColor, board[X, Y - i]))
                {
                    int points = GetPoints(board[X, Y - i]);
                    up = (points == 0) && IsNotOffBoard(X, Y - i);
                    allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X, Y - i)));
                }
                //If we should check right and the next spot isn't off the board and the next spot is empty || opposite color, then add move
                if (right && IsNotOffBoard(X + i, Y) && !IsColor(myColor, board[X + i, Y]))
                {
                    int points = GetPoints(board[X + i, Y]);
                    right = (points == 0) && IsNotOffBoard(X + i, Y);
                    allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X + i, Y)));
                }
                //If we should check down and the next spot isn't off the board and the next spot is empty, then add move
                if (down && IsNotOffBoard(X, Y + i) && !IsColor(myColor, board[X, Y + i]))
                {
                    int points = GetPoints(board[X, Y + i]);
                    down = (points == 0) && IsNotOffBoard(X, Y + i);
                    allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X, Y + i)));
                }
                //If we should check left and the next spot isn't off the board and the next spot is empty, then add move
                if (left && IsNotOffBoard(X - i, Y) && !IsColor(myColor, board[X - i, Y]))
                {
                    int points = GetPoints(board[X - i, Y]);
                    left = (points == 0) && IsNotOffBoard(X - i, Y);
                    allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X - i, Y)));
                }
                // If we have stopped looking in all directions, then stop looping and return
                if (!up && !right && !down && !left)
                {
                    return allMoves;
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
