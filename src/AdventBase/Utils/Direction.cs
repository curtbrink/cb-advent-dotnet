namespace AdventBase.Utils;

public enum Direction
{
    North,
    South,
    East,
    West,
}

public static class DirectionExtensions
{
    extension(Direction direction)
    {
        public Direction TurnRight() => direction switch
        {
            Direction.North => Direction.East,
            Direction.East => Direction.South,
            Direction.South => Direction.West,
            Direction.West => Direction.North,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), "Invalid direction"),
        };

        public Direction TurnLeft() => direction switch
        {
            Direction.North => Direction.West,
            Direction.East => Direction.North,
            Direction.South => Direction.East,
            Direction.West => Direction.South,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), "Invalid direction"),
        };

        public Direction TurnAround() => direction switch
        {
            Direction.North => Direction.South,
            Direction.East => Direction.West,
            Direction.South => Direction.North,
            Direction.West => Direction.East,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), "Invalid direction"),
        };

        public bool IsXAxis() => direction is Direction.East or Direction.West;

        public bool IsYAxis() => direction is Direction.North or Direction.South;

        public bool IsPositiveOnAxis() => direction is Direction.East or Direction.South;
        
        public bool IsNegativeOnAxis() => direction is Direction.North or Direction.West;
    }
}