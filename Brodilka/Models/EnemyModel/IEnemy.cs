using HodimBrodim;

namespace Brodilka.Models
{
    public interface IEnemy
    {
        public Point Position { get; }
        public Point PreviousPosition { get; }

        public void Move(Point playerPosition);

        public bool CollisionWithPlayer(Point playerPosition);
    }
}
