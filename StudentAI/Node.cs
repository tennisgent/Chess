using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UvsChess;

namespace StudentAI
{
    public class Node
    {
        public ChessBoard board = null;
        public ChessMove move = null;
        public Node parent = null;
        public List<Node> children = null;
        public ChessColor color;

        public Node(ChessColor c, ChessMove m, ChessBoard b, Node p)
        {
            color = c;
            move = m;
            board = b;
            parent = p;
        }

        // THESE ARE SIMPLY THE PROPERTIES FOR THE DIFFERENT MEMBER VALUES
        public ChessBoard Board
        {
            get { return board; }
            set { board = value; }
        }
        public Node Parent
        {
            get { return parent; }
            set { parent = value; }
        }
        public ChessMove Move
        {
            get { return move; }
            set { move = value; }
        }
        public ChessColor Color
        {
            get { return color; }
            set { color = value; }
        }
        /// <summary>
        /// This function will climb the tree from this node all the way to the root and return the proper move to take from the actual possible moves
        /// </summary>
        /// <returns></returns>
        public ChessMove GetActualMove()
        {
            Node temp = this;
            if (temp.Parent == null)
            {
                return temp.Move;
            }
            else
            {
                while (temp.Parent.Parent != null)
                {
                    temp = temp.Parent;
                }
                return temp.Move;
            }
        }

    }
}
