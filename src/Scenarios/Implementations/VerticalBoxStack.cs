using System.Collections.Generic;
using Common;
using Physics;
using Physics.Bodies;
using Physics.Helpers;
using Physics.Joints;

namespace Scenarios.Implementations
{
    public sealed class VerticalBoxStack : BaseScenario
    {
        public VerticalBoxStack(World world)
            : base(world)
        {
            InitializeObjects();
        }

        private Body cart;
        private void InitializeObjects()
        {
            

            //cart = new Polygon(new Vector2(10, 2), new Vector2(0, 4), BodyDefinition.Default.WithDensity(100));
            //var fWheel = new Circle(new Vector2(-4, 1.2f), 0.5f);
            //var bWheel = new Circle(new Vector2(+4, 1.2f), 0.5f);
            //var cWheel = new Circle(new Vector2(0, 1.2f), 0.5f)
            //{
            //    Name = "back"
            //};
            //
            //Add(new LineJoint(cart, fWheel, new Vector2(-4, -1), Vector2.Zero, Vector2.UnitY));
            //Add(new DistanceJoint(cart, fWheel, new Vector2(-4, -1), Vector2.Zero, 10, 10));
            //
            //Add(new LineJoint(cart, bWheel, new Vector2(+4, -1), Vector2.Zero, Vector2.UnitY));
            //Add(new DistanceJoint(cart, bWheel, new Vector2(+4, -1), Vector2.Zero, 10, 10));
            //
            //Add(new LineJoint(cart, cWheel, new Vector2(0, -1), Vector2.Zero, Vector2.UnitY));
            //Add(new DistanceJoint(cart, cWheel, new Vector2(0, -1), Vector2.Zero, 10, 10));
            //Add(cWheel);
            //Add(cart); Add(fWheel); Add(bWheel); 

            Add(BodyBuilder.BuildCircle(new Vector2(0, 3), 1));
            //Add(new Polygon(new Vector2(5), new Vector2(0, 10)));
            Add(BodyBuilder.BuildIrregularPolygon(new List<Vector2>() { new Vector2(1, 0), new Vector2(0, 0.4f), new Vector2(-1, 0) }, BodyDefinition.DefaultLocked));

            return;
            var rami1 = BodyBuilder.BuildBox(new Vector2(1), new Vector2(-2, 2));
            var rami2 = BodyBuilder.BuildBox(new Vector2(1), new Vector2(+2, 2));
            Add(rami1); Add(rami2);

            Add(new DistanceJoint(rami1, rami2, new Vector2(0.5f, 0), new Vector2(-0.5f, 0), 1));
            return;

            var box = BodyBuilder.BuildBox(new Vector2(1), Vector2.UnitY*10);
            Add(box);
            box.Force += Vector2.UnitY*-500;

            Add(BodyBuilder.BuildBox(new Vector2(20, 0.2f), new Vector2(0, 5), BodyDefinition.DefaultLocked));


            return;

            var dis1 = BodyBuilder.BuildBox(new Vector2(1), new Vector2(-10, 2));
            var dis2 = BodyBuilder.BuildBox(new Vector2(1), new Vector2(-8, 2));

            Add(dis1); Add(dis2);
            Add(new DistanceJoint(dis1, dis2, new Vector2(0.3f, 0.3f), new Vector2(-0.3f, -0.3f), 1));

            return;
            var linearMotor1 = BodyBuilder.BuildBox(new Vector2(5), new Vector2(0, 6));
            var linearMotor2 = BodyBuilder.BuildBox(new Vector2(2), new Vector2(0, 10));

            Add(linearMotor1);
            Add(linearMotor2);
            Add(new LinearMotorJoint(linearMotor1, linearMotor2, new Vector2(0), new Vector2(0, 0), Vector2.UnitY)
            {
                MaximumMotorForce = 700,
                MotorSpeed = 1
            });

            return;

            var angleRev = BodyBuilder.BuildBox(new Vector2(10, 0.2f), new Vector2(-10, 2));

            Add(angleRev);
            Add(new AngleJoint(Ground, angleRev, 10, 0));
            Add(new RevoluteJoint(Ground, angleRev, angleRev.ToGlobal(new Vector2(-4, 0))));
            return;
            var c = 4;
            var step = MathUtil.TwoPi / c;
            var angleOffset = 0.01f;
            var positionOffset = Vector2.UnitY * 20;
            var outerRadius = 20f;
            var innerRadius = outerRadius - 0.5f;

            var ps = new List<Polygon>();
            for (var i = 0; i < c; i++)
            {
                var v1 = Vector2.Rotate(step * i - (step / 2) + angleOffset, Vector2.UnitY) * innerRadius + positionOffset;
                var v2 = Vector2.Rotate(step * i + (step / 2) - angleOffset, Vector2.UnitY) * innerRadius + positionOffset;
                var v3 = Vector2.Rotate(step * i - (step / 2) + angleOffset, Vector2.UnitY) * outerRadius + positionOffset;
                var v4 = Vector2.Rotate(step * i + (step / 2) - angleOffset, Vector2.UnitY) * outerRadius + positionOffset;

                var p = BodyBuilder.BuildIrregularPolygon(new List<Vector2> { v1, v2, v4, v3 });
                ps.Add(p);
                Add(new RevoluteJoint(Ground, ps[i], positionOffset));
                Add(new AngularMotorJoint(Ground, ps[i])
                {
                    MaximumMotorTorque = 100000,
                    MotorSpeed = 0.5f
                });
                Add(p);
            }

            for (var i = 0; i < ps.Count; i++)
            {
                var j = i == ps.Count - 1 ? 0 : i + 1;
                var prev = ps[i];
                var next = ps[j];

                Add(new WeldJoint(prev, next, (prev.Position + next.Position) / 2f));
            }

            return;


            return;

            var autoBody = BodyBuilder.BuildBox(new Vector2(5, 1), new Vector2(0, 3));
            //var fronWheel = new Polygon(16, 0.3f, new Vector2(-2, 1));
            //var backWheel = new Polygon(16, 0.3f, new Vector2(+2, 1));
            var fronWheel = BodyBuilder.BuildCircle(new Vector2(-1.8f, 1), 0.5f);
            var backWheel = BodyBuilder.BuildCircle(new Vector2(+1.8f, 1), 0.5f);

            Add(autoBody); Add(fronWheel); Add(backWheel);
            var freq = 10f;
            var damp = .4f;

            //Add(new DistanceJoint(autoBody, fronWheel, new Vector2(-2.5f, -0.5f), Vector2.Zero, freq, damp));
            //Add(new DistanceJoint(autoBody, fronWheel, new Vector2(0, -0.5f), Vector2.Zero, freq, damp));
            //Add(new DistanceJoint(autoBody, backWheel, new Vector2(+2.5f, -0.5f), Vector2.Zero, freq, damp));
            //Add(new DistanceJoint(autoBody, backWheel, new Vector2(0, -0.5f), Vector2.Zero, freq, damp));
            //Add(new DaniMotorJoint(autoBody, backWheel) { MaximumMotorTorque = 10, MotorSpeed = 10f });

            Add(new LineJoint(autoBody, fronWheel, new Vector2(-1.8f, -0.5f), Vector2.Zero, Vector2.UnitY));
            Add(new LineJoint(autoBody, backWheel, new Vector2(+1.8f, -0.5f), Vector2.Zero, Vector2.UnitY));

            Add(new AngularMotorJoint(autoBody, backWheel) { MaximumMotorTorque = 100, MotorSpeed = 5f });
            Add(new DistanceJoint(autoBody, fronWheel, new Vector2(-1.8f, -0.5f), Vector2.Zero, freq, damp));
            Add(new DistanceJoint(autoBody, backWheel, new Vector2(+1.8f, -0.5f), Vector2.Zero, freq, damp));

            return;
          

           


            

            var motorAngl1 = BodyBuilder.BuildRegularPolygon(7, 1, new Vector2(0, 5));
            var motorAngl2 = BodyBuilder.BuildBox(new Vector2(3, 1), new Vector2(0, 10));
            Add(new RevoluteJoint(motorAngl1, motorAngl2, motorAngl1.Position + Vector2.UnitY));
            Add(new AngularMotorJoint(motorAngl1, motorAngl2)
            {
                MotorSpeed = 5,
                MaximumMotorTorque = 100f
            });

            Add(motorAngl1);
            Add(motorAngl2);
            

           

            var angle1 = BodyBuilder.BuildBox(new Vector2(1), new Vector2(-10, 2));
            var angle2 = BodyBuilder.BuildBox(new Vector2(1), new Vector2(-8, 2));

            Add(angle1); Add(angle2);
            Add(new AngleJoint(angle1, angle2));
            return;

            var line1 = BodyBuilder.BuildBox(new Vector2(5), new Vector2(0, 6));
            var line2 = BodyBuilder.BuildBox(new Vector2(2), new Vector2(0, 10));

            Add(line1);
            Add(line2);
            Add(new LineJoint(line1, line2, new Vector2(0), new Vector2(3, 0), Vector2.UnitY));

            return;

            var pri1 = BodyBuilder.BuildBox(new Vector2(1), new Vector2(-2, 5));
            var pri2 = BodyBuilder.BuildBox(new Vector2(10, 3), new Vector2(+2, 1));

            Add(pri1); Add(pri2);
            Add(new PrismaticJoint(pri1, pri2, new Vector2(1), Vector2.Zero, Vector2.UnitY));

            return;

            var pulley1 = BodyBuilder.BuildBox(new Vector2(1), new Vector2(-4, 3.6f));
            var pulley2 = BodyBuilder.BuildBox(new Vector2(1), new Vector2(+4, 3.6f));

            Add(pulley1); Add(pulley2);
            Add(new PulleyJoint(pulley1, pulley2,
                new Vector2(0.3f), new Vector2(-0.3f),
                pulley1.Position + Vector2.UnitY * 6, pulley2.Position + Vector2.UnitY * 6, 1));

            var weld1 = BodyBuilder.BuildBox(new Vector2(1), new Vector2(-2, 2));
            var weld2 = BodyBuilder.BuildBox(new Vector2(1), new Vector2(+2, 2));

            Add(weld1); Add(weld2);
            Add(new WeldJoint(weld1, weld2, (weld1.Position + weld2.Position) / 2f + Vector2.UnitY * 3));
            
            var rev1 = BodyBuilder.BuildBox(new Vector2(1), new Vector2(10, 2));
            var rev2 = BodyBuilder.BuildBox(new Vector2(1), new Vector2(8, 2));

            Add(rev1); Add(rev2);
            Add(new RevoluteJoint(rev1, rev2, (rev1.Position + rev2.Position) / 2));

          

           

          

            return;


           
            return;


        

            
          

            var b1 = BodyBuilder.BuildBox(new Vector2(1), new Vector2(-2, 5));
            var b2 = BodyBuilder.BuildBox(new Vector2(10, 3), new Vector2(+2, 1));

            Add(b1); Add(b2);
            Add(new LineJoint(b1, b2, new Vector2(1), Vector2.Zero, Vector2.UnitY));

            //for (var i = 0; i < 20; i++)
            //{
            //    var box = new Polygon(new Vector2(1), new Vector2(0, 0.2f + i*1.1f));
            //    Add(box);
            //}
        }

        //public override void Update(GameTime time)
        //{
        //    if (Game.MouseState.MiddleButton.Pressed)
        //    {
        //        Add(new Polygon(new Vector2(1), Game.Camera.CursorPosition.AsPhysicsVector2()));
        //    }
        //
        //    if (Game.KeyboardState.IsKeyPressed(Keys.LeftControl))
        //    {
        //        cart.Force += new Vector2(1000000, 0);
        //    }
        //}
    }
}
