using Common;
using Physics;
using Physics.Bodies;
using Physics.Helpers;
using Physics.Joints;

namespace Scenarios.Implementations
{
    public class TankScenario: BaseScenario
    {
        public TankScenario(World world)
            : base(world)
        {
            InitializeObjects();
        }

        private Body lastLink;
        private Body firstLink;

        public void LancFeszites()
        {
            lastLink.Force += new Vector2(250, 0);
        }

        public void LancCsatolas()
        {
            Add(new RevoluteJoint(lastLink, firstLink, (lastLink.Position + firstLink.Position) / 2f));
        }

        private void InitializeObjects()
        {
            var bottomWheelRadius = .5f;
            var bottomWheelGap = 0.1f;
            var bottomWheelYOffset = 1f;
            var bottomWheelsCount = 7;
            var topWheelXOffset = 0.5f;
            var topWheelYOffset = 0.6f;
            var topWheelRadius = .5f;
            var bodyOversize = 0.2f;

            var bodyWidth = (topWheelRadius + bodyOversize + topWheelXOffset)*2
                            + bottomWheelGap * (bottomWheelsCount - 2)
                            + bottomWheelsCount * bottomWheelRadius * 2;

            var bodyPosY = 5;
            var bodyHeight = 1f;
            var body = BodyBuilder.BuildBox(new Vector2(bodyWidth, bodyHeight), new Vector2(0, bodyPosY));



            var topWheelHeihgt = bodyPosY - bodyHeight/2f - topWheelYOffset - topWheelRadius/2f;
            var rTopWheel = BodyBuilder.BuildCircle(new Vector2(bodyWidth/-2f + bodyOversize + topWheelRadius, topWheelHeihgt),topWheelRadius);
            var lTopWheel = BodyBuilder.BuildCircle(new Vector2(bodyWidth/2f - bodyOversize - topWheelRadius, topWheelHeihgt),topWheelRadius);

            //var rTopWheel = new Polygon(16, topWheelRadius, new Vector2(bodyWidth / -2f + bodyOversize + topWheelRadius, topWheelHeihgt));
            //var lTopWheel = new Polygon(16, topWheelRadius, new Vector2(bodyWidth / 2f - bodyOversize - topWheelRadius, topWheelHeihgt));

            Add(new RevoluteJoint(body, lTopWheel, lTopWheel.Position));
            Add(new RevoluteJoint(body, rTopWheel, rTopWheel.Position));

            Add(lTopWheel);
            Add(rTopWheel);
            Add(body);

            var bottomWheelPosY = bodyPosY - bodyHeight/2 -  bottomWheelRadius - bottomWheelYOffset;
            var bottomWheelPosX = -(bottomWheelsCount/2)*(bottomWheelRadius*2 + bottomWheelGap);
            var step = bottomWheelRadius*2 + bottomWheelGap;
            for (var i = 0; i < bottomWheelsCount; i++)
            {
                var pos = new Vector2(bottomWheelPosX + step*i, bottomWheelPosY);
                var bottomWheel = BodyBuilder.BuildCircle(pos, bottomWheelRadius);
                //var bottomWheel = new Polygon(16, bottomWheelRadius, pos);

                var globalAnchor1 = new Vector2(pos.X, bodyPosY - bodyHeight/2);
                var localAnchor1 = body.ToLocal(globalAnchor1);
                Add(new LineJoint(body, bottomWheel, localAnchor1, Vector2.Zero, Vector2.UnitY));
                Add(new DistanceJoint(body, bottomWheel, localAnchor1, Vector2.Zero, 3, .5f));

                Add(bottomWheel);
            }

            var topLinkHeight = lTopWheel.Position.Y + topWheelRadius + 0.4f;
            var linkLength = 0.3f;
            var linkHeight = 0.1f;
            var linkGap = 0.05f;

            Body prevLink = null;
            var topLinkStart = lTopWheel.Position.Y - topWheelRadius - 0.4f;
            var step1 = linkLength + linkGap;
            var linksCount = 58;
            var linkCollisionImmunityGroup = new CollisionImmunityGroup("TankLinks");
            for (var i = 0; i < linksCount; i++)
            {
                var pos = new Vector2(topLinkStart + step1 * i, topLinkHeight);
                var link = BodyBuilder.BuildBox(new Vector2(linkLength, linkHeight), pos);
                link.AddToCollisionImmunityGroup(linkCollisionImmunityGroup);
                Add(link);
                if (prevLink != null)
                {
                    Add(new RevoluteJoint(prevLink, link, (prevLink.Position + link.Position) /2f ));
                }

                if (i == 0) firstLink = link;
                if (i == linksCount-1) lastLink = link;

                prevLink = link;
            }

            var feszito = BodyBuilder.BuildCircle(new Vector2(0, 4), 0.2f);
            //var feszito = new Polygon(16, 0.2f, new Vector2(0, 4));
            Add(feszito);
            Add(new RevoluteJoint(body, feszito, feszito.Position));
        }
    }
}
