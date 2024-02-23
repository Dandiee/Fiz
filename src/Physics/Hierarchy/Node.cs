using Common;
using Physics.Bodies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.Hierarchy
{
    public class Node
    {
        public Vector2 GlobalMin { get; protected set; }
        public Vector2 GlobalMax { get; protected set; }
        public Vector2 GlobalSize { get; protected set; }
        public float Area { get; set; }

        public Node LeftNode { get; set; }
        public Node RightNode { get; set; }

        public Body LeftBody { get; set; }
        public Body RightBody { get; set; }

        public Node Parent { get; set; }
    }
}
