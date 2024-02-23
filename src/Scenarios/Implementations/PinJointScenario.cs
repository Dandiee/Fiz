using System.Collections.Generic;
using System.Linq;
using Common;
using Physics;
using Physics.Bodies;
using Physics.Helpers;
using Physics.Joints;

namespace Scenarios.Implementations
{
    public sealed class PinJointScenario : BaseScenario
    {

        public PinJointScenario(World world)
            : base(world)
        {
            InitializeObjects();
        }

        private void Teeter(float positionX)
        {
            

            var position = new Vector2(positionX, 1f);

            var size = new Vector2(10, 0.2f);
            var baseBox = BodyBuilder.BuildBox(new Vector2(0.3f, position.Y - size.Y),
                new Vector2(position.X, (position.Y - size.Y) / 2), BodyDefinition.DefaultLocked);

            var teeter = BodyBuilder.BuildBox(size, position);

            Add(BodyBuilder.BuildBox(new Vector2(1), new Vector2(position.X + size.X / 2 - 0.6f, position.Y + 1)));
            Add(BodyBuilder.BuildRegularPolygon(5, 0.2f, new Vector2(position.X - size.X / 2 + 0.3f, position.Y + 1)));
            Add(BodyBuilder.BuildRegularPolygon(5, 0.2f, new Vector2(position.X - size.X / 2 + 0.7f, position.Y + 3)));

            Add(baseBox);
            Add(teeter);
            Add(new RevoluteJoint(teeter, baseBox, position));
        }



        float PendantPlankWidth = 0.2f;
        float PendantPlankHeight = 0.5f;

        private void PendantRecursive(Polygon parent, int level)
        {
            var fullWidth = 7f;
            var offset = 0.3f;
            var holderHeight = 0.1f;
            

            var currentWidth = (fullWidth / level) - 2 * offset;
            var holder = BodyBuilder.BuildBox(new Vector2(currentWidth, holderHeight),
                new Vector2(parent.Position.X, parent.Position.Y - offset - PendantPlankHeight));

            var left = BodyBuilder.BuildBox(new Vector2(PendantPlankWidth, PendantPlankHeight),
                new Vector2(holder.Position.X - currentWidth / 2f, holder.Position.Y - offset - PendantPlankHeight / 2f));

            var right = BodyBuilder.BuildBox(new Vector2(PendantPlankWidth, PendantPlankHeight),
                new Vector2(holder.Position.X + currentWidth / 2f, holder.Position.Y - offset - PendantPlankHeight / 2f));

            Add(holder);
            Add(left);
            Add(right);

            Add(new RevoluteJoint(holder, parent, new Vector2(holder.Position)));

            Add(new RevoluteJoint(left, holder, new Vector2(holder.Position.X - currentWidth / 2f, left.Position.Y + PendantPlankHeight / 2)));
            Add(new RevoluteJoint(right, holder, new Vector2(holder.Position.X + currentWidth / 2f, right.Position.Y + PendantPlankHeight / 2)));

            if (level < 3)
            {
                PendantRecursive(left, level + 1);
                PendantRecursive(right, level + 1);
            }
        }

        private void Pendant()
        {
            var basePlank = BodyBuilder.BuildBox(new Vector2(PendantPlankWidth, PendantPlankHeight), 
                new Vector2(-20, 20), BodyDefinition.DefaultLocked);
            Add(basePlank);
            PendantRecursive(basePlank, 1);
        }

        private void HanginBalls()
        {
            var positionX = 10;
            var ballsCount = 5;
            var topHeight = 8;
            var ballHeight = 3;
            var ballRadius = 0.5f;
            var offset = 0.00f;

            var baseBoxWidth = (ballsCount - 1) * (ballRadius * 2 + offset);
            var baseBox = BodyBuilder.BuildBox(new Vector2(baseBoxWidth, 0.2f),
                new Vector2(positionX, topHeight), BodyDefinition.DefaultLocked);

            var x = positionX - baseBoxWidth / 2f;
            for (var i = 0; i < ballsCount; i++)
            {
                var ball = BodyBuilder.BuildCircle(new Vector2(x, ballHeight), ballRadius,
                    BodyDefinition.Default.WithRestitution(1).WithFriction(0));
                Add(ball);
                Add(new RevoluteJoint(ball, baseBox, new Vector2(x, topHeight)));
                x += ballRadius * 2 + offset;
            }

            Add(baseBox);
        }


        public void SuspensionBridge()
        {
            const int HalfPlanksCount = 10;
            const float PlankWidth = 1f;
            const float PlankDistance = 0.3f;
            const float BridgeHeight = 10f;

            var planks = new List<Polygon>();
            Polygon prevPlank = null;
            for (var i = -HalfPlanksCount; i <= HalfPlanksCount; i++)
            {
                var plank = BodyBuilder.BuildBox(new Vector2(PlankWidth, 0.2f), new Vector2(i * (PlankWidth + PlankDistance), BridgeHeight));
                if (prevPlank != null)
                {
                    var plankJoint = new RevoluteJoint(prevPlank, plank, (prevPlank.Position + plank.Position) / 2);
                    Add(plankJoint);
                }
                Add(plank);
                planks.Add(plank);
                prevPlank = plank;
            }


            var lBaseStone = BodyBuilder.BuildBox(new Vector2(1, 1), planks.First().Position - Vector2.UnitX * PlankWidth, BodyDefinition.DefaultLocked);
            var rBaseStone = BodyBuilder.BuildBox(new Vector2(1, 1), planks.Last().Position + Vector2.UnitX * PlankWidth, BodyDefinition.DefaultLocked);

            Add(new RevoluteJoint(lBaseStone, planks[0], planks[0].Position + new Vector2(-1, 0)));
            Add(new RevoluteJoint(rBaseStone, planks.Last(), planks.Last().Position + new Vector2(1, 0)));
            Add(lBaseStone);
            Add(rBaseStone);
        }

        private void MultiPendulum()
        {
            var posX = 20f;
            var planksCount = 15;
            var plankSize = new Vector2(0.1f, 0.5f);
            var offset = 0.1f;

            var totalHeight = plankSize.Y*(planksCount + offset) + 1 + 5;

            var baseBox = BodyBuilder.BuildBox(new Vector2(0.5f), new Vector2(posX, totalHeight + 1), BodyDefinition.DefaultLocked);
            Add(baseBox);

            var lastBody = BodyBuilder.BuildBox(plankSize, new Vector2(posX, totalHeight - offset));
            Add(new RevoluteJoint(lastBody, baseBox, (baseBox.Position + lastBody.Position) / 2));
            Add(lastBody);
            for (var i = 1; i < planksCount; ++i)
            {
                var pos = new Vector2(posX, lastBody.Position.Y - plankSize.Y - offset);
                var plank = BodyBuilder.BuildBox(plankSize, pos);
                Add(plank);
                Add(new RevoluteJoint(lastBody, plank, (pos + lastBody.Position) / 2));
                lastBody = plank;
            }
        }

        private void SimpleCar()
        {
            var carSize = new Vector2(5, 0.2f);
            var carPosition = new Vector2(0, 15);
            var carBody = BodyBuilder.BuildBox(carSize, carPosition);
            var lWheel = BodyBuilder.BuildCircle(new Vector2(carPosition.X - carSize.X/2 + 0.5f, carPosition.Y - 1), 0.6f);
            var rWheel = BodyBuilder.BuildCircle(new Vector2(carPosition.X + carSize.X/2 - 0.5f, carPosition.Y - 1), 0.6f);

            Add(carBody);
            Add(lWheel);
            Add(rWheel);

            Add(new RevoluteJoint(lWheel, carBody, lWheel.Position));
            Add(new RevoluteJoint(rWheel, carBody, rWheel.Position));
        }

        private void InitializeObjects()
        {
            SuspensionBridge();
            Pendant();
            Teeter(-5);
            HanginBalls();
            
            MultiPendulum();
            SimpleCar();
        }
    }
}
