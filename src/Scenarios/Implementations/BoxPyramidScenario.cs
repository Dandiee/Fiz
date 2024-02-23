using Common;
using Physics;
using Physics.Bodies;
using Physics.Helpers;

namespace Scenarios.Implementations
{
    public sealed class BoxPyramidScenario : BaseScenario
    {
        private int _baseRowBoxCount = 30;
        private const float Distance = 0.2f;
        private static readonly Vector2 BoxSize = Vector2.One;

        public BoxPyramidScenario(World world)
            : base(world, false)
        {
            InitializeObjects();
        }

        public void AddBoxToBaseRow()
        {
            Clear();
            _baseRowBoxCount++;
            InitializeObjects();
        }
        public void RemoveBoxToBaseRow()
        {
            if (_baseRowBoxCount > 1)
            {
                Clear();
                _baseRowBoxCount--;
                InitializeObjects();
            }
        }

        private void InitializeObjects()
        {
            Add(new Segment(new Vector2(-100, 0), new Vector2(100, 0)));

            for (var i = 0; i < _baseRowBoxCount; i++)
            {
                var y = Distance + BoxSize.Y / 2 + i*(Distance + BoxSize.Y);
                
                var halfBoxesCount = (_baseRowBoxCount - i) / 2;
                var startX = (_baseRowBoxCount - i)%2 == 0
                    ? -(Distance + BoxSize.X)*(.5f+(halfBoxesCount - 1))
                    : -halfBoxesCount * (BoxSize.X + Distance);

                for (var j = 0; j < _baseRowBoxCount-i; j++)
                {
                    var currentX = startX + j*(BoxSize.X + Distance);
                    var box = BodyBuilder.BuildBox(BoxSize, new Vector2(currentX, y));

                    Add(box);
                }
            }
        }
    }
}
