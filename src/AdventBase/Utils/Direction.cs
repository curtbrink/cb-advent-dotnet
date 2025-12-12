namespace AdventBase.Utils;

public enum Direction
{
    North,
    South,
    East,
    West,
    Unknown
}

public static class DirectionExtensions
{
    extension(Direction direction)
    {
        public static Direction From(char s) => s switch
        {
            'U' or 'N' => Direction.North,
            'D' or 'S' => Direction.South,
            'L' or 'W' => Direction.West,
            'R' or 'E' => Direction.East,
            _ => throw new ArgumentOutOfRangeException(nameof(s), "Invalid direction"),
        };
        
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

        public (int x, int y) Vector() => direction switch
        {
            Direction.North => (0, -1),
            Direction.East => (1, 0),
            Direction.West => (-1, 0),
            Direction.South => (0, 1),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), "Invalid direction"),
        };
    }
}