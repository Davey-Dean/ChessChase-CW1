using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnMovement : EnemyMovement {
    public Direction direction;

    public enum Direction {
        North,
        East,
        South,
        West
    }

    private static Vector2Int DirectionToVector(Direction direction) {
        return direction switch
        {
            Direction.North => Vector2Int.up,
            Direction.East => Vector2Int.right,
            Direction.South => Vector2Int.down,
            Direction.West => Vector2Int.left,
            _ => Vector2Int.zero,
        };
    }

    public static Direction InverseDirection(Direction direction) {
        return direction switch
        {
            Direction.North => Direction.South,
            Direction.East => Direction.West,
            Direction.South => Direction.North,
            Direction.West => Direction.East,
        };
    }

    private List<Vector2Int> GetTraversableCells() {
        List<Vector2Int> cells = new List<Vector2Int>();

        if ((Position + DirectionToVector(direction)).x >= 0
            && (Position + DirectionToVector(direction)).x < grid.XSize
            && (direction == Direction.East || direction == Direction.West)) {

            for (int y = Mathf.Max(Position.y - 1, 0); y <= Mathf.Min(grid.YSize - 1, Position.y + 1); y++) {
                if (grid.GetGridValue((Position + DirectionToVector(direction)).x, y).IsEmpty()) {
                    cells.Add(new Vector2Int((Position + DirectionToVector(direction)).x, y));
                }
            }

        } else if ((Position + DirectionToVector(direction)).y >= 0
            && (Position + DirectionToVector(direction)).y < grid.YSize
            && (direction == Direction.North || direction == Direction.South)) {

            for (int x = Mathf.Max(Position.x - 1, 0); x <= Mathf.Min(grid.XSize - 1, Position.x + 1); x++) {
                if (grid.GetGridValue(x, (Position + DirectionToVector(direction)).y).IsEmpty()) {
                    cells.Add(new Vector2Int(x, (Position + DirectionToVector(direction)).y));
                }
            }
        }

        return cells;
    }

    public override Queue<Vector2Int> GetPathToCell(Vector2Int cell) {
        var path = new Queue<Vector2Int>();
        path.Enqueue(cell);
        return path;
    }

    public override bool HasAttackOppurtunity(Vector3 playerPosition)
    {
        return GetTraversableCells().Contains(grid.WorldToCell(playerPosition));
    }

    public override Vector2Int? GetNextMovementCell(Vector3 playerPosition) {
        List<Vector2Int> traversableCells = GetTraversableCells();

        if (traversableCells.Contains(Position + DirectionToVector(direction))) {
            return Position + DirectionToVector(direction);
        }

        direction = InverseDirection(direction);

        traversableCells = GetTraversableCells();

        if (traversableCells.Contains(Position + DirectionToVector(direction))) {
            return Position + DirectionToVector(direction);
        }

        return null;
    }

    public override Vector2Int? GetNextAttackCell(Vector3 playerPosition) {
        Vector2Int playerCell = grid.WorldToCell(playerPosition);

        List<Vector2Int> traversableCells = GetTraversableCells();

        if (traversableCells.Count <= 0) {
            direction = InverseDirection(direction);
            return null;
        }

        if (traversableCells.Contains(playerCell)) {
            return playerCell;
        } else {
            return null;
        }
    }
    
}
