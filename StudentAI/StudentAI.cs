using System;
using System.Collections.Generic;
using System.Text;
using UvsChess;

namespace StudentAI
{
    public class StudentAI : IChessAI
    {
        PriorityQueue<ChessMove> ourMoves = new PriorityQueue<ChessMove>();
        PriorityQueue<ChessMove> theirMoves = new PriorityQueue<ChessMove>();
        bool wasInCheckLastTurn = false;
        //You should be able to implement it using PriorityQueue<ChessMove> queue = new PriorityQueue<ChessMove>(); and then you just add things to it using the built in queue.Add(move, points); method.  It should be pretty straight forward.
        //#region IChessAI Members that are implemented by the Student

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
            ourMoves = new PriorityQueue<ChessMove>();
            //with priority queue this will return best move
            ourMoves = GetAllMoves(board,myColor);
            Console.WriteLine(ourMoves.ToString());
            ChessMove ourMove = ourMoves.Pop();
            ChessMove ourOriginalMove = ourMove;
            ChessMove theirMove;
            ChessBoard tempBoard = board.Clone();
            tempBoard.MakeMove(ourMove);
            bool wereInCheck = true;
            //figure out best move that won't put us in check or as last resort make best move if all moves will put in check
            while (wereInCheck)
            {
                wereInCheck = false;
                if (myColor == ChessColor.White)
                    theirMoves = GetAllMoves(tempBoard, ChessColor.Black);
                else
                    theirMoves = GetAllMoves(tempBoard, ChessColor.White);
                theirMove = theirMoves.Pop();
                if (GetPoints(tempBoard[theirMove.To.X, theirMove.To.Y]) > 1000)
                {
                    if (ourMoves.Count != 0)
                        ourMove = ourMoves.Pop();
                    else
                        return ourOriginalMove;
                    theirMoves = new PriorityQueue<ChessMove>();
                    tempBoard = board.Clone();
                    tempBoard.MakeMove(ourMove);
                    wereInCheck = true;
                }
            }
            tempBoard = board.Clone();
            tempBoard.MakeMove(ourMove);
            PriorityQueue<ChessMove> nextMoves = new PriorityQueue<ChessMove>();
            nextMoves = GetAllMoves(tempBoard,myColor);
            ChessMove ourNextMove = nextMoves.Pop();
            for (int i = 0; i <= 7; i++)
            {
                if(tempBoard[i,0] == ChessPiece.WhitePawn)
                    tempBoard[i,0] = ChessPiece.WhiteQueen;
                if(tempBoard[i,7] == ChessPiece.BlackPawn)
                    tempBoard[i,7] = ChessPiece.BlackQueen;
            }
            if (GetPoints(tempBoard[ourNextMove.To.X, ourNextMove.To.Y]) > 1000)
            {
                theirMoves = new PriorityQueue<ChessMove>();
                tempBoard = board.Clone();
                tempBoard.MakeMove(ourMove);
                if (myColor == ChessColor.White)
                    theirMoves = GetAllMoves(tempBoard, ChessColor.Black);
                else
                    theirMoves = GetAllMoves(tempBoard, ChessColor.White);
                while (true)
                {
                    if(theirMoves.Count == 0)
                    {
                        ourMove.Flag = ChessFlag.Checkmate;
                        break;
                    }
                    theirMove = theirMoves.Pop();
                    ChessBoard tempBoard2 = tempBoard.Clone();
                    tempBoard2.MakeMove(theirMove);
                    ourMoves = new PriorityQueue<ChessMove>();
                    ourMoves = GetAllMoves(tempBoard2, myColor);
                    ChessMove temp = ourMoves.Pop();
                    if (!(GetPoints(tempBoard2[temp.To.X, temp.To.Y]) > 1000))
                    {
                        ourMove.Flag = ChessFlag.Check;
                        break;
                    }
                }
            }
            return ourMove;
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
            //once we have set of moves check it
            return true;
        }



        #region Functions implemented by Tristan and Zack

        PriorityQueue<ChessMove> GetAllMoves(ChessBoard currentBoard, ChessColor myColor)
        {
            PriorityQueue<ChessMove> allMoves = new PriorityQueue<ChessMove>();
            for (int Y = 0; Y < ChessBoard.NumberOfRows; Y++)
            {
                for (int X = 0; X < ChessBoard.NumberOfColumns; X++)
                {
                    switch (currentBoard[X, Y])
                    {
                        case ChessPiece.BlackPawn:
                        case ChessPiece.WhitePawn:
                            if(IsColor(myColor,currentBoard[X, Y]))
                            allMoves = GetAllPawnMoves(allMoves, currentBoard, X, Y, myColor);
                            break;
                        case ChessPiece.BlackRook:
                        case ChessPiece.WhiteRook:
                            if (IsColor(myColor, currentBoard[X, Y]))
                            allMoves = GetAllRookMoves(allMoves, currentBoard, X, Y, myColor);
                            break;
                        case ChessPiece.BlackKnight:
                        case ChessPiece.WhiteKnight:
                            if (IsColor(myColor, currentBoard[X, Y]))
                            allMoves = GetAllKnightMoves(allMoves, currentBoard, X, Y, myColor);
                            break;
                        case ChessPiece.BlackBishop:
                        case ChessPiece.WhiteBishop:
                            if (IsColor(myColor, currentBoard[X, Y]))
                            allMoves = GetAllBishopMoves(allMoves, currentBoard, X, Y, myColor);
                            break;
                        case ChessPiece.BlackQueen:
                        case ChessPiece.WhiteQueen:
                            if (IsColor(myColor, currentBoard[X, Y]))
                            allMoves = GetAllQueenMoves(allMoves, currentBoard, X, Y, myColor);
                            break;
                        case ChessPiece.BlackKing:
                        case ChessPiece.WhiteKing:
                            if (IsColor(myColor, currentBoard[X, Y]))
                            allMoves = GetAllKingMoves(allMoves, currentBoard, X, Y, myColor);
                            break;
                        default:
                            break;
                    }
                }
            }


            return allMoves;
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
            return (X >= 0 && X < ChessBoard.NumberOfColumns) && (Y >= 0 && Y < ChessBoard.NumberOfRows);
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

        public PriorityQueue<ChessMove> GetAllKingMoves(PriorityQueue<ChessMove> allMoves, ChessBoard board, int X, int Y, ChessColor myColor)
        {
            allMoves = GetAllBishopMoves(allMoves, board, X, Y, myColor, 1);
            allMoves = GetAllRookMoves(allMoves, board, X, Y, myColor, 1);
            return allMoves;
        }

        public PriorityQueue<ChessMove> GetAllKnightMoves(PriorityQueue<ChessMove> allMoves, ChessBoard board, int X, int Y, ChessColor myColor)
        {
                if (X + 2 <= 7 && Y + 1 <= 7)
                    if (!(IsColor(myColor, board[X + 2, Y + 1])))
                    {
                        allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X + 2, Y + 1)),GetPoints(board[X + 2, Y + 1]));
                    }

                if (X + 2 <= 7 && Y - 1 >= 0)
                    if (!(IsColor(myColor,board[X + 2, Y - 1])))
                    {
                            allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X + 2, Y - 1)), GetPoints(board[X + 2, Y - 1]));
                    }

                if (X + 1 <= 7 && Y + 2 <= 7)
                    if (!(IsColor(myColor, board[X + 1, Y + 2])))
                    {
                            allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X + 1, Y + 2)), GetPoints(board[X + 1, Y + 2]));
                    }

                if (X + 1 <= 7 && Y - 2 >= 0)
                    if (!(IsColor(myColor, board[X + 1, Y - 2])))
                    {
                            allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X + 1, Y - 2)), GetPoints(board[X + 1, Y - 2]));
                    }

                if (X - 1 >= 0 && Y + 2 <= 7)
                    if (!(IsColor(myColor, board[X - 1, Y + 2])))
                    {
                            allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X - 1, Y + 2)), GetPoints(board[X - 1, Y + 2]));
                    }
                if (X - 1 >= 0 && Y - 2 >= 0)
                    if (!(IsColor(myColor, board[X - 1, Y - 2])))
                    {
                            allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X - 1, Y - 2)), GetPoints(board[X - 1, Y - 2]));
                    }
                if (X - 2 >= 0 && Y - 1 >= 0)
                    if (!(IsColor(myColor, board[X - 2, Y - 1])))
                    {
                            allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X - 2, Y - 1)), GetPoints(board[X - 2, Y - 1]));
                    }
                if (X - 2 >= 0 && Y + 1 <= 7)
                    if (!(IsColor(myColor, board[X - 2, Y + 1])))
                    {
                            allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X - 2, Y + 1)), GetPoints(board[X - 2, Y + 1]));
                    } 
            return allMoves;
        }

        public PriorityQueue<ChessMove> GetAllQueenMoves(PriorityQueue<ChessMove> allMoves, ChessBoard board, int X, int Y, ChessColor myColor)
        {
            allMoves = GetAllBishopMoves(allMoves,board,X,Y,myColor);
            allMoves = GetAllRookMoves(allMoves,board,X,Y,myColor);
            return allMoves;
        }

        public PriorityQueue<ChessMove> GetAllBishopMoves(PriorityQueue<ChessMove> allMoves, ChessBoard board, int X, int Y, ChessColor myColor,int maxDist = ChessBoard.NumberOfColumns-1)
        {
            // the maximum number of moves from corner to corner is 7
            // flag1 represents up and right, flag 2 right and down flag 3 down and left flag 4 up and left
            bool flag1 = false;
            bool flag2 = false;
            bool flag3 = false;
            bool flag4 = false;
            for (int i = 0; i <= maxDist; i++)
            {
                if (!flag1)
                {
                    if (!(IsColor(myColor,board[X + i, Y + i])))
                        allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X + i, Y + i)),GetPoints(board[X + i, Y + i]));
                    if ((int)board[X + i, Y + i] != (int)ChessPiece.Empty)
                    {
                        flag1 = true;
                    }
                }
                if (!flag2)
                {
                    if (!(IsColor(myColor, board[X + i, Y - i])))
                        allMoves.Add(new ChessMove(new ChessLocation(X,Y), new ChessLocation(X + i, Y - i)),GetPoints(board[X + i, Y - i]));
                    if ((int)board[X + i, Y - i] != (int)ChessPiece.Empty)
                    {
                        flag2 = true;
                    }
                }
                if (!flag3)
                {
                    if (!(IsColor(myColor, board[X - i, Y - i])))
                        allMoves.Add(new ChessMove(new ChessLocation(X,Y), new ChessLocation(X - i, Y - i)),GetPoints(board[X - i, Y - i]));
                    if ((int)board[X - i, Y - i] != (int)ChessPiece.Empty)
                    {
                        flag3 = true;
                    }
                }
                if (!flag4)
                {
                    if (!(IsColor(myColor, board[X - i, Y + i])))
                        allMoves.Add(new ChessMove(new ChessLocation(X,Y), new ChessLocation(X - i, Y + i)),GetPoints(board[X - i, Y + i]));
                    if ((int)board[X - i, Y + i] != (int)ChessPiece.Empty)
                    {
                        flag4 = true;
                    }
                }
            }
            return allMoves;
        }
       
        public PriorityQueue<ChessMove> GetAllPawnMoves(PriorityQueue<ChessMove> allMoves, ChessBoard board, int X, int Y, ChessColor myColor)
        {
            
            if (myColor == ChessColor.White)
            {
                if (IsNotOffBoard(X,Y-1) && (board[X, Y - 1] == ChessPiece.Empty) && (board[X, Y] == ChessPiece.WhitePawn))
                {
                    // Generate a move to move my pawn 1 tile forward
                    allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X, Y - 1)),GetPoints(board[X,Y-1]));
                }
                if (Y == 6 && (board[X, Y - 2] == ChessPiece.Empty) && (board[X, Y] == ChessPiece.WhitePawn))
                {
                    // Generate a move to move my pawn 2 tiles forward
                    allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X, Y - 2)),GetPoints(board[X, Y -2]));
                }
                if (IsNotOffBoard(X-1,Y-1) && IsColor(ChessColor.Black, board[X - 1, Y - 1]) && (board[X, Y] == ChessPiece.WhitePawn))
                {
                    // Generate a move to take a piece to the left
                    allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X - 1, Y - 1)),GetPoints(board[X - 1, Y - 1]));
                }
                if (IsNotOffBoard(X+1,Y-1) && IsColor(ChessColor.Black, board[X + 1, Y - 1]) && (board[X, Y] == ChessPiece.WhitePawn))
                {
                    // Generate a move to take a piece to the right
                    allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X + 1, Y - 1)), GetPoints(board[X + 1, Y - 1]));
                }
            }
            else // myColor is black
            {
                if (board[X, Y] == ChessPiece.BlackPawn)
                {
                    if (IsNotOffBoard(X, Y+1) && (board[X, Y + 1] == ChessPiece.Empty))
                    {
                        // Generate a move to move my pawn 1 tile forward
                        allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X, Y + 1)), GetPoints(board[X, Y + 1]));
                    }
                    if (Y == 1 && (board[X, Y + 2] == ChessPiece.Empty))
                    {
                        // Generate a move to move my pawn 2 tiles forward
                        allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X, Y + 2)), GetPoints(board[X, Y + 2]));
                    }
                    if (IsNotOffBoard(X+1,Y+1) && IsColor(ChessColor.White, board[X + 1, Y + 1]))
                    {
                        // Generate a move to take a piece to the right
                        allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X + 1, Y + 1)), GetPoints(board[X + 1, Y + 1]));
                    }
                    if (IsNotOffBoard(X-1,Y+1) && IsColor(ChessColor.White, board[X - 1, Y + 1]))
                    {
                        // Generate a move to take a piece to the left
                        allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X - 1, Y + 1)), GetPoints(board[X - 1, Y + 1]));
                    }
                }
            }

            return allMoves;
        }
        
        public PriorityQueue<ChessMove> GetAllRookMoves(PriorityQueue<ChessMove> allMoves, ChessBoard board, int X, int Y, ChessColor myColor, int maxDist = ChessBoard.NumberOfColumns-1)
        {
            bool up = true, right = true, down = true, left = true;

            // spiral outward from current location while checking available moves
            for (int i = 0; i <= maxDist; i++)
            {
                //If we should check up and the next spot isn't off the board and the next spot is empty || opposite color, then add move
                if (up && IsNotOffBoard(X, Y - i) && !IsColor(myColor, board[X, Y - i]))
                {
                    int points = GetPoints(board[X, Y - i]);
                    up = (points == 0) && IsNotOffBoard(X, Y - i);
                    allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X, Y - i)), GetPoints(board[X, Y - i]));
                }
                else
                up = false;
                //If we should check right and the next spot isn't off the board and the next spot is empty || opposite color, then add move
                if (right && IsNotOffBoard(X + i, Y) && !IsColor(myColor, board[X + i, Y]))
                {
                    int points = GetPoints(board[X + i, Y]);
                    right = (points == 0) && IsNotOffBoard(X + i, Y);
                    allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X + i, Y)), GetPoints(board[X + i, Y]));
                }
                right = false;
                //If we should check down and the next spot isn't off the board and the next spot is empty, then add move
                if (down && IsNotOffBoard(X, Y + i) && !IsColor(myColor, board[X, Y + i]))
                {
                    int points = GetPoints(board[X, Y + i]);
                    down = (points == 0) && IsNotOffBoard(X, Y + i);
                    allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X, Y + i)), GetPoints(board[X, Y + i]));
                }
                else
                down = false;
                //If we should check left and the next spot isn't off the board and the next spot is empty, then add move
                if (left && IsNotOffBoard(X - i, Y) && !IsColor(myColor, board[X - i, Y]))
                {
                    int points = GetPoints(board[X - i, Y]);
                    left = (points == 0) && IsNotOffBoard(X - i, Y);
                    allMoves.Add(new ChessMove(new ChessLocation(X, Y), new ChessLocation(X - i, Y)), GetPoints(board[X - i, Y]));
                }
                else
                left = false;
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
