using System;
using System.Collections.Generic;
using System.Text;
using UvsChess;

namespace StudentAI
{
    public class StudentAI : IChessAI
    {
        public static Node bestMoveSoFar = null;
        public ChessColor myColor;
        public int bestScore;


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
        /// 
        // checks if the passed in move will result in myColor being in check
        public bool IsCheck(ChessMove move, ChessBoard board, ChessColor myColor)
        {
            ChessPiece pieceToCheckAgainst = (myColor == ChessColor.White) ? ChessPiece.WhiteKing : ChessPiece.BlackKing;
            return (board[move.To.X, move.To.Y] == pieceToCheckAgainst);
        }

        /// <summary>
        /// Function checks to see if taking the proposedMove will put (or keep)
        /// myColor in check.
        /// 
        /// First it copies the currentBoard and applies the proposed move.
        /// Next it gathers all the possible moves for the opposing color
        /// and returns the best move for that color.
        /// Finally it checks to make sure that the best move does 
        /// not take myColor's king.  If it doesn't, then the proposedMove
        /// will get myColor out of check.
        /// </summary>
        /// <param name="proposedMove"></param>
        /// <param name="board"></param>
        /// <param name="myColor"></param>
        /// <returns></returns>
        public bool WillBeCheck(ChessMove proposedMove, ChessBoard currentBoard, ChessColor myColor)
        {
            ChessBoard tempBoard = currentBoard.Clone();
            tempBoard.MakeMove(proposedMove);
            List<ChessMove> possibleMoves = GetAllMoves(tempBoard, OppColor(myColor));
            if (possibleMoves.Count != 0)
            {
                return IsCheck(possibleMoves[possibleMoves.Count - 1], tempBoard, myColor);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// This function will return the opposite color of whatever color is passed in.
        /// For example, if you pass in White as 'color', you will get out Black and vice versa.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public ChessColor OppColor(ChessColor color)
        {
            return (color == ChessColor.Black) ? ChessColor.White : ChessColor.Black;
        }



        public ChessMove GetNextMove(ChessBoard board, ChessColor myColor)
        {
            bestMoveSoFar = null;
            this.myColor = myColor;
            Node root = new Node(OppColor(myColor), null, board, null);
            const int DEPTH_LIMIT = 2;
            for (int i = 0; i < DEPTH_LIMIT; i++)
            {
                Console.WriteLine(i);
                if (!IsMyTurnOver())
                {
                    MakeChildren(root);
                }
                else
                {
                    return bestMoveSoFar.GetActualMove();
                }
            }
            return bestMoveSoFar.GetActualMove();
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
            if (moveToCheck.Flag == ChessFlag.Checkmate)
            {
                boardBeforeMove.MakeMove(moveToCheck);
                List<ChessMove> possibleMoves = GetAllMoves(boardBeforeMove, OppColor(colorOfPlayerMoving));
                ChessMove bestMove = possibleMoves[possibleMoves.Count - 1];
                possibleMoves.RemoveAt(possibleMoves.Count - 1);
                while (WillBeCheck(bestMove, boardBeforeMove, OppColor(colorOfPlayerMoving)))
                {
                    if (possibleMoves.Count == 0)
                    {
                        return true;
                    }
                    else
                    {
                        bestMove = possibleMoves[possibleMoves.Count - 1];
                        possibleMoves.RemoveAt(possibleMoves.Count - 1);
                    }
                }
                return false;
            }
            else if (WillBeCheck(moveToCheck, boardBeforeMove, OppColor(colorOfPlayerMoving)) && moveToCheck.Flag != ChessFlag.Check)
            {
                return false;
            }
            else
            {
                //once we have set of moves check it
                return true;
            }
        }



        #region Functions implemented by Tristan and Zack

        public List<ChessMove> GetAllMoves(ChessBoard currentBoard, ChessColor myColor)
        {
            List<ChessMove> allMoves = new List<ChessMove>();
            for (int Y = 0; Y < ChessBoard.NumberOfRows; Y++)
            {
                for (int X = 0; X < ChessBoard.NumberOfColumns; X++)
                {
                    switch (currentBoard[X, Y])
                    {
                        case ChessPiece.BlackPawn:
                        case ChessPiece.WhitePawn:
                            if (IsColor(myColor, currentBoard[X, Y]))
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

        /// <summary>
        /// This returns the number of points awarded for the given ChessPiece
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        public int GetPoints(ChessPiece piece)
        {
            switch (piece)
            {
                case ChessPiece.Empty:
                    return 0;
                case ChessPiece.BlackPawn:
                case ChessPiece.WhitePawn:
                    return 1;
                case ChessPiece.BlackRook:
                case ChessPiece.WhiteRook:
                    return 3;
                case ChessPiece.BlackBishop:
                case ChessPiece.WhiteBishop:
                    return 3;
                case ChessPiece.BlackKnight:
                case ChessPiece.WhiteKnight:
                    return 5;
                case ChessPiece.BlackQueen:
                case ChessPiece.WhiteQueen:
                    return 10;
                case ChessPiece.BlackKing:
                case ChessPiece.WhiteKing:
                    return 1000;
                default:
                    return 0;
            }
        }

        public List<ChessMove> GetAllKingMoves(List<ChessMove> allMoves, ChessBoard board, int X, int Y, ChessColor myColor)
        {
            allMoves = GetAllBishopMoves(allMoves, board, X, Y, myColor, 1);
            allMoves = GetAllRookMoves(allMoves, board, X, Y, myColor, 1);
            return allMoves;
        }

        public List<ChessMove> GetAllKnightMoves(List<ChessMove> allMoves, ChessBoard board, int X, int Y, ChessColor myColor)
        {
            if (X + 2 <= 7 && Y + 1 <= 7)
                if (!(IsColor(myColor, board[X + 2, Y + 1])))
                {
                    ChessMove move = new ChessMove(new ChessLocation(X, Y), new ChessLocation(X + 2, Y + 1));
                    move.ValueOfMove = GetPoints(board[X + 2, Y + 1]);
                    allMoves.Add(move);
                }

            if (X + 2 <= 7 && Y - 1 >= 0)
                if (!(IsColor(myColor, board[X + 2, Y - 1])))
                {
                    ChessMove move = new ChessMove(new ChessLocation(X, Y), new ChessLocation(X + 2, Y - 1));
                    move.ValueOfMove = GetPoints(board[X + 2, Y - 1]);
                    allMoves.Add(move);
                }

            if (X + 1 <= 7 && Y + 2 <= 7)
                if (!(IsColor(myColor, board[X + 1, Y + 2])))
                {
                    ChessMove move = new ChessMove(new ChessLocation(X, Y), new ChessLocation(X + 1, Y + 2));
                    move.ValueOfMove = GetPoints(board[X + 1, Y + 2]);
                    allMoves.Add(move);
                }

            if (X + 1 <= 7 && Y - 2 >= 0)
                if (!(IsColor(myColor, board[X + 1, Y - 2])))
                {
                    ChessMove move = new ChessMove(new ChessLocation(X, Y), new ChessLocation(X + 1, Y - 2));
                    move.ValueOfMove = GetPoints(board[X + 1, Y - 2]);
                    allMoves.Add(move);
                }

            if (X - 1 >= 0 && Y + 2 <= 7)
                if (!(IsColor(myColor, board[X - 1, Y + 2])))
                {
                    ChessMove move = new ChessMove(new ChessLocation(X, Y), new ChessLocation(X - 1, Y + 2));
                    move.ValueOfMove = GetPoints(board[X - 1, Y + 2]);
                    allMoves.Add(move);
                }
            if (X - 1 >= 0 && Y - 2 >= 0)
                if (!(IsColor(myColor, board[X - 1, Y - 2])))
                {
                    ChessMove move = new ChessMove(new ChessLocation(X, Y), new ChessLocation(X - 1, Y - 2));
                    move.ValueOfMove = GetPoints(board[X - 1, Y - 2]);
                    allMoves.Add(move);
                }
            if (X - 2 >= 0 && Y - 1 >= 0)
                if (!(IsColor(myColor, board[X - 2, Y - 1])))
                {
                    ChessMove move = new ChessMove(new ChessLocation(X, Y), new ChessLocation(X - 2, Y - 1));
                    move.ValueOfMove = GetPoints(board[X - 2, Y - 1]);
                    allMoves.Add(move);
                }
            if (X - 2 >= 0 && Y + 1 <= 7)
                if (!(IsColor(myColor, board[X - 2, Y + 1])))
                {
                    ChessMove move = new ChessMove(new ChessLocation(X, Y), new ChessLocation(X - 2, Y + 1));
                    move.ValueOfMove = GetPoints(board[X - 2, Y + 1]);
                    allMoves.Add(move);
                }
            return allMoves;
        }

        public List<ChessMove> GetAllQueenMoves(List<ChessMove> allMoves, ChessBoard board, int X, int Y, ChessColor myColor)
        {
            allMoves = GetAllBishopMoves(allMoves, board, X, Y, myColor);
            allMoves = GetAllRookMoves(allMoves, board, X, Y, myColor);
            return allMoves;
        }

        public List<ChessMove> GetAllBishopMoves(List<ChessMove> allMoves, ChessBoard board, int X, int Y, ChessColor myColor, int maxDist = ChessBoard.NumberOfColumns-1)
        {
            // the maximum number of moves from corner to corner is 7
            // flag1 represents up and right, flag 2 right and down flag 3 down and left flag 4 up and left
            bool flag1 = false;
            bool flag2 = false;
            bool flag3 = false;
            bool flag4 = false;
            for (int i = 1; i <= maxDist; i++)
            {
                if (!flag1)
                {
                    if (IsNotOffBoard(X + i, Y + i))
                    {
                        if (!(IsColor(myColor, board[X + i, Y + i])))
                        {
                            ChessMove move = new ChessMove(new ChessLocation(X, Y), new ChessLocation(X + i, Y + i));
                            move.ValueOfMove = GetPoints(board[X + i, Y + i]);
                            allMoves.Add(move);
                        }
                        if (board[X + i, Y + i] != ChessPiece.Empty)
                        {
                            flag1 = true;
                        }
                    }
                    else
                    {
                        flag1 = true;
                    }
                }
                if (!flag2)
                {
                    if (IsNotOffBoard(X + i, Y - i))
                    {
                        if (!(IsColor(myColor, board[X + i, Y - i])))
                        {
                            ChessMove move = new ChessMove(new ChessLocation(X, Y), new ChessLocation(X + i, Y - i));
                            move.ValueOfMove = GetPoints(board[X + i, Y - i]);
                            allMoves.Add(move);
                        }
                        if (board[X + i, Y - i] != ChessPiece.Empty)
                        {
                            flag2 = true;
                        }
                    }
                    else
                    {
                        flag2 = true;
                    }
                }
                if (!flag3)
                {
                    if (IsNotOffBoard(X - i, Y - i))
                    {
                        if (!(IsColor(myColor, board[X - i, Y - i])))
                        {
                            ChessMove move = new ChessMove(new ChessLocation(X, Y), new ChessLocation(X - i, Y - i));
                            move.ValueOfMove = GetPoints(board[X - i, Y - i]);
                            allMoves.Add(move);
                        }
                        if (board[X - i, Y - i] != ChessPiece.Empty)
                        {
                            flag3 = true;
                        }
                    }
                    else
                    {
                        flag3 = true;
                    }
                }
                if (!flag4)
                {
                    if (IsNotOffBoard(X - i, Y + i))
                    {
                        if (!(IsColor(myColor, board[X - i, Y + i])))
                        {
                            ChessMove move = new ChessMove(new ChessLocation(X, Y), new ChessLocation(X - i, Y + i));
                            move.ValueOfMove = GetPoints(board[X - i, Y + i]);
                            allMoves.Add(move);
                        }
                        if (board[X - i, Y + i] != ChessPiece.Empty)
                        {
                            flag4 = true;
                        }
                    }
                    else
                    {
                        flag4 = true;
                    }
                }
            }
            return allMoves;
        }

        public List<ChessMove> GetAllPawnMoves(List<ChessMove> allMoves, ChessBoard board, int X, int Y, ChessColor myColor)
        {

            if (myColor == ChessColor.White)
            {
                if (IsNotOffBoard(X, Y - 1) && (board[X, Y - 1] == ChessPiece.Empty) && (board[X, Y] == ChessPiece.WhitePawn))
                {
                    // Generate a move to move my pawn 1 tile forward
                    ChessMove move = new ChessMove(new ChessLocation(X, Y), new ChessLocation(X, Y - 1));
                    move.ValueOfMove = GetPoints(board[X, Y - 1]);
                    allMoves.Add(move);
                }
                if (Y == 6 && (board[X, Y - 2] == ChessPiece.Empty) && (board[X, Y] == ChessPiece.WhitePawn) && board[X, Y - 1] == ChessPiece.Empty)
                {
                    // Generate a move to move my pawn 2 tiles forward
                    ChessMove move = new ChessMove(new ChessLocation(X, Y), new ChessLocation(X, Y - 2));
                    move.ValueOfMove = GetPoints(board[X, Y - 2]);
                    allMoves.Add(move);
                }
                if (IsNotOffBoard(X - 1, Y - 1) && IsColor(ChessColor.Black, board[X - 1, Y - 1]) && (board[X, Y] == ChessPiece.WhitePawn))
                {
                    // Generate a move to take a piece to the left
                    ChessMove move = new ChessMove(new ChessLocation(X, Y), new ChessLocation(X - 1, Y - 1));
                    move.ValueOfMove = GetPoints(board[X - 1, Y - 1]);
                    allMoves.Add(move);
                }
                if (IsNotOffBoard(X + 1, Y - 1) && IsColor(ChessColor.Black, board[X + 1, Y - 1]) && (board[X, Y] == ChessPiece.WhitePawn))
                {
                    // Generate a move to take a piece to the right
                    ChessMove move = new ChessMove(new ChessLocation(X, Y), new ChessLocation(X + 1, Y - 1));
                    move.ValueOfMove = GetPoints(board[X + 1, Y - 1]);
                    allMoves.Add(move);
                }
            }
            else // myColor is black
            {
                if (board[X, Y] == ChessPiece.BlackPawn)
                {
                    if (IsNotOffBoard(X, Y + 1) && (board[X, Y + 1] == ChessPiece.Empty))
                    {
                        // Generate a move to move my pawn 1 tile forward
                        ChessMove move = new ChessMove(new ChessLocation(X, Y), new ChessLocation(X, Y + 1));
                        move.ValueOfMove = GetPoints(board[X, Y + 1]);
                        allMoves.Add(move);
                    }
                    if (Y == 1 && (board[X, Y + 2] == ChessPiece.Empty) && board[X, Y + 1] == ChessPiece.Empty)
                    {
                        // Generate a move to move my pawn 2 tiles forward
                        ChessMove move = new ChessMove(new ChessLocation(X, Y), new ChessLocation(X, Y + 2));
                        move.ValueOfMove = GetPoints(board[X, Y + 2]);
                        allMoves.Add(move);
                    }
                    if (IsNotOffBoard(X + 1, Y + 1) && IsColor(ChessColor.White, board[X + 1, Y + 1]))
                    {
                        // Generate a move to take a piece to the right
                        ChessMove move = new ChessMove(new ChessLocation(X, Y), new ChessLocation(X + 1, Y + 1));
                        move.ValueOfMove = GetPoints(board[X + 1, Y + 1]);
                        allMoves.Add(move);
                    }
                    if (IsNotOffBoard(X - 1, Y + 1) && IsColor(ChessColor.White, board[X - 1, Y + 1]))
                    {
                        // Generate a move to take a piece to the left
                        ChessMove move = new ChessMove(new ChessLocation(X, Y), new ChessLocation(X - 1, Y + 1));
                        move.ValueOfMove = GetPoints(board[X - 1, Y + 1]);
                        allMoves.Add(move);
                    }
                }
            }

            return allMoves;
        }
       public void MakeChildren(Node parent)
        {
            if (parent.children == null)
            {
                parent.children = new List<Node>();
                List<ChessMove> moves = GetAllMoves(parent.board, OppColor(parent.color));
                foreach (ChessMove move in moves)
                {
                    ChessBoard tempBoard = parent.board.Clone();
                    tempBoard.MakeMove(move);
                    //IsValidMove(ChessBoard boardBeforeMove, ChessMove moveToCheck, ChessColor colorOfPlayerMoving)
                    if (parent.move != null)
                    {
                        if(parent.color != myColor)
                        {
                            move.ValueOfMove = parent.move.ValueOfMove + GetPoints(parent.board[move.To.X, move.To.Y]);
                        }
                        else
                        {
                            move.ValueOfMove = parent.move.ValueOfMove - GetPoints(parent.board[move.To.X, move.To.Y]);
                        }
                    }
                    else
                    {
                        move.ValueOfMove = GetPoints(parent.board[move.To.X, move.To.Y]);
                    }
                    Node newChild = new Node(OppColor(parent.color), move, tempBoard, parent);
                    if(bestMoveSoFar == null)
                    {
                        bestMoveSoFar = newChild;
                    }
                    if (IsValidMove(parent.board, move, OppColor(parent.color)))
                    {
                        parent.children.Add(newChild);
                        if (newChild.move.ValueOfMove > bestMoveSoFar.move.ValueOfMove)
                        {
                            bestMoveSoFar = newChild;
                        }
                    }
                    else
                    {
                        move.Flag = ChessFlag.Check;
                        if (!(IsValidMove(parent.board, move, OppColor(parent.color))))
                        {
                            move.Flag = ChessFlag.Checkmate;
                            if (IsValidMove(parent.board, move, OppColor(parent.color)))
                            {
                                if (OppColor(parent.color) != myColor)
                                {
                                    parent.children.Add(newChild);
                                    if (newChild.move.ValueOfMove > bestMoveSoFar.move.ValueOfMove)
                                    {
                                        bestMoveSoFar = newChild;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (newChild.move.ValueOfMove > bestMoveSoFar.move.ValueOfMove)
                            {
                                bestMoveSoFar = newChild;
                            }
                            parent.children.Add(newChild);
                        }
                    }
                }
            }
            else
            {
                foreach (Node child in parent.children)
                {
                    MakeChildren(child);
                }
            }
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
                    ChessMove move = new ChessMove(new ChessLocation(X, Y), new ChessLocation(X, Y - i));
                    move.ValueOfMove = GetPoints(board[X, Y - i]);
                    allMoves.Add(move);
                }
                else
                    up = false;
                //If we should check right and the next spot isn't off the board and the next spot is empty || opposite color, then add move
                if (right && IsNotOffBoard(X + i, Y) && !IsColor(myColor, board[X + i, Y]))
                {
                    int points = GetPoints(board[X + i, Y]);
                    right = (points == 0) && IsNotOffBoard(X + i, Y);
                    ChessMove move = new ChessMove(new ChessLocation(X, Y), new ChessLocation(X + i, Y));
                    move.ValueOfMove = GetPoints(board[X + i, Y]);
                    allMoves.Add(move);
                }
                right = false;
                //If we should check down and the next spot isn't off the board and the next spot is empty, then add move
                if (down && IsNotOffBoard(X, Y + i) && !IsColor(myColor, board[X, Y + i]))
                {
                    int points = GetPoints(board[X, Y + i]);
                    down = (points == 0) && IsNotOffBoard(X, Y + i);
                    ChessMove move = new ChessMove(new ChessLocation(X, Y), new ChessLocation(X, Y + i));
                    move.ValueOfMove = GetPoints(board[X, Y + i]);
                    allMoves.Add(move);
                }
                else
                    down = false;
                //If we should check left and the next spot isn't off the board and the next spot is empty, then add move
                if (left && IsNotOffBoard(X - i, Y) && !IsColor(myColor, board[X - i, Y]))
                {
                    int points = GetPoints(board[X - i, Y]);
                    left = (points == 0) && IsNotOffBoard(X - i, Y);
                    ChessMove move = new ChessMove(new ChessLocation(X, Y), new ChessLocation(X - i, Y));
                    move.ValueOfMove = GetPoints(board[X - i, Y]);
                    allMoves.Add(move);
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
