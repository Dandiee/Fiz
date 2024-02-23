using System;
using System.Collections.Generic;
using Common;
using Physics.Bodies;

namespace Physics
{
    /*
    public class Tree
    {
        public string Name { get; set; } 
        public delegate void ChangedEventHandler(object sender, EventArgs e);
        public event EventHandler<ChangedEventHandler> Change;
        protected virtual void OnChange(ChangedEventHandler e)
        {
            var handler = Change;
            if (handler != null) handler(this, e);
        }

    

        public Node Root { get; set; }

        public Tree()
        {
            Root = new Node(null);

            Name = System.Guid.NewGuid().ToString();
        }

        public void Add(Body body)
        {
            Root.Add(body);
            OnChange(null);
        }

        public void UpdateBody(Body body)
        {
            //var node = body.Node;

            //if (node.GlobalMin.X > body.GlobalMin.X || node.GlobalMin.Y > body.GlobalMin.Y || node.GlobalMax.X < body.GlobalMax.X || node.GlobalMax.Y < body.GlobalMax.Y)
            //{
            //    RemoveNode(body.Node);
                
            //    if(node.LeftBody != null)
            //    {
            //        Add(node.LeftBody);
            //    }

            //    if (node.RightBody != null)
            //    {
            //        Add(node.RightBody);
            //    }

            //    OnChange(null);

            //}

            
        }

        public void RemoveNode(Node node)
        {
            if (node.Parent != null)
            {
                var parent = node.Parent;
                if (parent.LeftNode == node)
                {
                    parent.LeftNode = null;
                }
                else
                {
                    parent.RightNode = null;
                }

                parent.Update();
            }
        }
    }

    public class Node
    {
        public int Level = 0;
        private static readonly System.Random _random = new System.Random();
        public Vector3 Color { get; set; }

        public Node LeftNode { get; set; }
        public Node RightNode { get; set; }

        public Body LeftBody { get; set; }
        public Body RightBody { get; set; }

        public Node Parent { get; set; }

        public Vector2 GlobalMin { get; protected set; }
        public Vector2 GlobalMax { get; protected set; }
        public Vector2 GlobalSize { get; protected set; }
        public float Area { get; set; }

        

        public Node(Node parent)
        {
            Level = parent == null ? 1 : parent.Level + 1;
            Parent = parent;
            Color = _random.NextVector3(Vector3.Zero, Vector3.One);
        }

        public Node(Node parent, Body leftBody, Body rightBody)
            : this(parent)
        {
            LeftBody = leftBody;
            RightBody = rightBody;
            if (leftBody != null)
            {
                leftBody.Node = this;
            }

            if (rightBody != null)
            {
                rightBody.Node = this;
            }

            GlobalMin = Vector2.Minimum(LeftBody.GlobalMin, RightBody.GlobalMin);
            GlobalMax = Vector2.Maximum(LeftBody.GlobalMax, RightBody.GlobalMax);
            GlobalSize = GlobalMax - GlobalMin;
            Area = GlobalSize.X * GlobalSize.Y;

            Tightening();
        }

        private void Tightening()
        {
            var increase = GlobalSize * 0.05f;
            GlobalMin -= increase;
            GlobalMax += increase;
        }

        

        public void Add(Body body)
        {
            //var leftArea = AreaWithThis(LeftNode, body, LeftBody);
            //var rightArea = AreaWithThis(RightNode, body, RightBody);
            //
            //if (LeftNode == null && LeftBody == null)
            //{
            //    LeftBody = body;
            //    body.Node = this;
            //
            //    Update();
            //}
            //else if(RightNode == null && RightBody == null)
            //{
            //    RightBody = body;
            //    body.Node = this;
            //
            //    Update();
            //}
            //else if (leftArea <= rightArea)
            //{
            //    if (LeftNode == null)
            //    {
            //        
            //        LeftNode = new Node(this, LeftBody, body);
            //        LeftBody = null;
            //        Update();
            //    }
            //    else
            //    {
            //        LeftNode.Add(body);
            //    }
            //}
            //else
            //{
            //    if(RightNode == null)
            //    {
            //        
            //        RightNode = new Node(this, RightBody, body);
            //        RightBody = null;
            //        Update();
            //    }
            //    else
            //    {
            //        RightNode.Add(body);
            //    }
            //}
            
        }

        private float AreaWithThis(Node node, Body body, Body childBody)
        {
            if(node == null && childBody == null)
            {
                return 0;
            }

            var hasNode = node != null;

            var minimum = hasNode ? Vector2.Minimum(node.GlobalMin, body.GlobalMin) : Vector2.Minimum(body.GlobalMin, childBody.GlobalMin);
            var maximum = hasNode ? Vector2.Maximum(node.GlobalMax, body.GlobalMax) : Vector2.Maximum(body.GlobalMax, childBody.GlobalMax);

            var size = maximum - minimum;
            var area = size.X * size.Y;

            return area;
        }

        public void Update()
        {
            if (LeftBody == null && LeftNode == null)
            {
                var rightMin = RightBody != null ? RightBody.GlobalMin : RightNode.GlobalMin;
                var rightMax = RightBody != null ? RightBody.GlobalMax : RightNode.GlobalMax;

                GlobalMin = rightMin;
                GlobalMax = rightMax;
            }
            else if (RightBody == null && RightNode == null)
            {
                var leftMin = LeftBody != null ? LeftBody.GlobalMin : LeftNode.GlobalMin;
                var leftMax = LeftBody != null ? LeftBody.GlobalMax : LeftNode.GlobalMax;

                GlobalMin = leftMin;
                GlobalMax = leftMax;
            }
            else
            {
                var leftMin = LeftBody != null ? LeftBody.GlobalMin : LeftNode.GlobalMin;
                var leftMax = LeftBody != null ? LeftBody.GlobalMax : LeftNode.GlobalMax;
                var rightMin = RightBody != null ? RightBody.GlobalMin : RightNode.GlobalMin;
                var rightMax = RightBody != null ? RightBody.GlobalMax : RightNode.GlobalMax;

                GlobalMin = Vector2.Minimum(rightMin, leftMin);
                GlobalMax = Vector2.Maximum(rightMax, leftMax);

                
            }
            GlobalSize = GlobalMax - GlobalMin;
            Area = GlobalSize.X * GlobalSize.Y;

            Tightening();

            if (Parent != null)
            {
                Parent.UpdateParent();
            }
        }

        public void UpdateParent()
        {
            var leftMin = LeftBody != null ? LeftBody.GlobalMin : LeftNode.GlobalMin;
            var leftMax = LeftBody != null ? LeftBody.GlobalMax : LeftNode.GlobalMax;

            var rightMin = RightBody != null ? RightBody.GlobalMin : RightNode.GlobalMin;
            var rightMax = RightBody != null ? RightBody.GlobalMax : RightNode.GlobalMax;

            GlobalMin = Vector2.Minimum(leftMin, rightMin);
            GlobalMax = Vector2.Maximum(leftMax, rightMax);

            GlobalSize = GlobalMax - GlobalMin;
            Area = GlobalSize.X * GlobalSize.Y;
            Tightening();
            if (Parent != null)
            {
                Parent.UpdateParent();
            }
        }
    }*/
}
