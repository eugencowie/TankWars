using System;
using Microsoft.Xna.Framework;

namespace TankWars
{
    /// <summary>
    /// Interface for a collider which can collide with rectangles and circles.
    /// </summary>
    interface ICollider
    {
        bool Intersects(RectangleCollider rectangle);
        bool Intersects(CircleCollider circle);

        bool Contains(Point point);
        bool Contains(Vector2 point);
    }

    /// <summary>
    /// A collider which uses a rectangle for collision.
    /// </summary>
    sealed class RectangleCollider : ICollider
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }

        public RectangleCollider(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }

        public float Left { get { return Position.X; } }
        public float Top { get { return Position.Y; } }
        public float Right { get { return Position.X + Size.X; } }
        public float Bottom { get { return Position.Y + Size.Y; } }

        public bool Intersects(RectangleCollider rectangle)
        {
            Rectangle r1 = new Rectangle(Position.ToPoint(), Size.ToPoint());
            Rectangle r2 = new Rectangle(rectangle.Position.ToPoint(), rectangle.Size.ToPoint());

            return r1.Intersects(r2);
        }

        public bool Intersects(CircleCollider circle)
        {
            return circle.Intersects(this);
        }

        public Rectangle GetRekt()
        {
            return new Rectangle(Position.ToPoint(), Size.ToPoint());
        }

        public bool Contains(Point point)
        {
            Rectangle r1 = new Rectangle(Position.ToPoint(), Size.ToPoint());
            return r1.Contains(point);
        }

        public bool Contains(Vector2 point)
        {
            Rectangle r1 = new Rectangle(Position.ToPoint(), Size.ToPoint());
            return r1.Contains(point);
        }
    }

    /// <summary>
    /// A collider which uses a circle for collision.
    /// </summary>
    sealed class CircleCollider : ICollider
    {
        public Vector2 Position { get; set; }
        public float Radius { get; set; }

        public CircleCollider(Vector2 position, float radius)
        {
            Position = position;
            Radius = radius;
        }

        public bool Intersects(CircleCollider other)
        {
            Vector2 difference = other.Position - Position;
            float intersectDistance = other.Radius + Radius;

            return (difference.Length() < intersectDistance);
        }

        public bool Intersects(RectangleCollider rectangle)
        {
            Vector2 v = new Vector2 {
                X = MathHelper.Clamp(Position.X, rectangle.Left, rectangle.Right),
                Y = MathHelper.Clamp(Position.Y, rectangle.Top, rectangle.Bottom)
            };

            Vector2 direction = Position - v;
            float distanceSquared = direction.LengthSquared();

            return ((distanceSquared > 0) && (distanceSquared < Radius * Radius));
        }

        public Rectangle GetRekt()
        {
            return new Rectangle((Position - new Vector2(Radius)).ToPoint(), new Point((int)(Radius * 2)));
        }

        public bool Contains(Point point)
        {
            return Contains(point.ToVector2());
        }

        public bool Contains(Vector2 point)
        {
            float distance = (Position - point).Length();
            return distance <= Radius;
        }
    }
}
