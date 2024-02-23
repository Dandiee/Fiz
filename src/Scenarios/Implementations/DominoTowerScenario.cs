using Common;
using Physics;
using Physics.Bodies;
using Physics.Helpers;

namespace Scenarios.Implementations
{
    public class DominoTowerScenario : BaseScenario
    {
        private int _baseCount = 10;
        private Vector2 _dominoSize = new Vector2(2, 0.5f);

        public DominoTowerScenario(World world) 
            : base(world, false)
        {
            CreateTower();
        }

        public void AddRow()
        {
            _baseCount++;
            CreateTower();
        }

        public void RemoveRow()
        {
            if (_baseCount > 0)
            {
                _baseCount--;
                CreateTower();
            }
        }

        private void CreateTower()
        {
            Clear();
            Add(new Segment(new Vector2(-100, 0), new Vector2(100, 0)));

            for (int i = 0; i < _baseCount; i++)
            {
                CreateRow(i);
            }

            for (var i = _baseCount; i > -1; i--)
            {
                var rowIndex = _baseCount - i;

                for (var j = i + 1; j < _baseCount; j++)
                {
                    
                }
            }
        }

        private void CreateRow(int rowIndex)
        {
            var count = _baseCount - rowIndex;
            var totalWidth = (count - 1)*(_dominoSize.X + _dominoSize.Y);
            var startX = totalWidth/-2f;
            var step = _dominoSize.X + _dominoSize.Y;

            var swappedSize = Vector2.Swap(_dominoSize);

            var offsetY = rowIndex*(_dominoSize.Y*2 + _dominoSize.X);

            var standingY = offsetY + _dominoSize.Y + _dominoSize.X/2f;
            var topLyingY = offsetY + _dominoSize.Y*1.5f + _dominoSize.X;
            var botLyingY = offsetY + _dominoSize.Y/2f;

            for (var i = 0; i < count; i++)
            {
                var currentX = startX + step*i;
                Add(BodyBuilder.BuildBox(_dominoSize, new Vector2(currentX, topLyingY)));
                Add(BodyBuilder.BuildBox(swappedSize, new Vector2(currentX, standingY)));
                Add(BodyBuilder.BuildBox(_dominoSize, new Vector2(currentX, botLyingY)));
            }
        }
    }
}
