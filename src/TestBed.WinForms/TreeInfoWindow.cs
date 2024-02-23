using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Physics;

namespace TestBed.WinForms
{
    /*
    public partial class TreeInfoWindow : Form
    {
        public Tree Tree { get; set; }

        public TreeInfoWindow()
        {
            InitializeComponent();
        }

        public void Init()
        {
            Tree.Change += (sender, args) =>
            {

                Draw();

            };
        }
        private System.Drawing.Graphics g;
        private System.Drawing.Pen pen1 = new System.Drawing.Pen(Color.Blue, 2F);

        private Point rootPosition = new Point();
        private Size nodeSize = new Size(20,20);
        private int levelHeight = 50;
        private int levelWidth= 50;
        private int height = 0;
        private int width = 0;

        private void Travel(Node node)
        {
            if (node.Level > height)
            {
                height = node.Level;
            }

            if (node.LeftNode != null)
            {
                Travel(node.LeftNode);
            }


            if (node.RightNode != null)
            {
                Travel(node.RightNode);
            }
        }

        private void Draw()
        {
            width = panel1.Width;
            height = -1;
            Travel(Tree.Root);

            g = panel1.CreateGraphics();
            g.Clear(Color.Black);

            rootPosition = new Point(panel1.Width/2, nodeSize.Height / 2 + 10);
            g.DrawRectangle(pen1, new Rectangle(rootPosition, nodeSize));

            if (Tree.Root.LeftNode != null)
            {
                DrawNode(Tree.Root.LeftNode, true, rootPosition);
            }


            if (Tree.Root.RightNode != null)
            {
                DrawNode(Tree.Root.RightNode, false, rootPosition);
            }

        }

        private void DrawNode(Node node, bool isLeft, Point parentPosition)
        {

            var parts = width / Math.Pow(2, node.Level - 1);
            
            var positionX =(int)(parentPosition.X + (parts / 2 * (isLeft ? -1 : 1)));
            var positionY = (node.Level + 1) * (nodeSize.Height + levelHeight) ;

            

            var position = new Point(positionX, positionY);
            g.DrawRectangle(pen1, new Rectangle(position, nodeSize));

            g.DrawLine(pen1, parentPosition, position);
            g.DrawString(((int)node.Area).ToString(), new Font(new FontFamily(GenericFontFamilies.Monospace), 10), new SolidBrush(Color.White), positionX, positionY);
            if (node.LeftNode != null)
            {
                DrawNode(node.LeftNode, true, position);
            }


            if (node.RightNode != null)
            {
                DrawNode(node.RightNode, false, position);
            }


            if (node.LeftBody != null)
            {
                DrawLeaf(true, position, node);
            }


            if (node.RightBody != null)
            {
                DrawLeaf(false, position, node);
            }

        }

        private void DrawLeaf(bool isLeft, Point parentPosition, Node parentNode)
        {

            var parts = width / Math.Pow(2, parentNode.Level);

            var positionX = (int)(parentPosition.X + (parts / 2 * (isLeft ? -1 : 1)));
            var positionY = (parentNode.Level + 2) * (nodeSize.Height + levelHeight);

            var position = new Point(positionX, positionY);
            g.DrawLine(pen1, parentPosition, position);
            g.DrawEllipse(pen1, new Rectangle(position, nodeSize));

         

        }


    }
    */
}
