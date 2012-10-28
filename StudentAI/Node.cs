using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UvsChess;

namespace StudentAI
{
    public class Node
    {
        public ChessMove move = null;
        public Node parent = null;
        public List<Node> children = null;
        public ChessColor color;
        public int depth;

        public Node(ChessColor c, ChessMove m, Node p)
        {
            color = c;
            move = m;
            parent = p;
            if (parent == null)
            {
                depth = 0;
            }
            else
            {
                depth = parent.depth + 1;
            }
        }

        // THESE ARE SIMPLY THE PROPERTIES FOR THE DIFFERENT MEMBER VALUES
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
        /// <summary>
        /// This function will climb the tree from this node all the way to the root and return the node
        /// </summary>
        /// <returns></returns>
        public Node GetActualNode()
        {
            Node temp = this;
            if (temp.Parent == null)
            {
                return temp;
            }
            else
            {
                while (temp.Parent.Parent != null)
                {
                    temp = temp.Parent;
                }
                return temp;
            }
        }

    }
}