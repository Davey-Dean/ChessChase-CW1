using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookMovement : EnemyMovement
{
    private List<Vector2Int> GetTraversableCells() {
        List<Vector2Int> cells = new List<Vector2Int>();

        for (int x = Position.x - 1; x >= 0; x--) {
            if (grid.GetGridValue(x, Position.y).IsEmpty()) {
                cells.Add(new Vector2Int(x, Position.y));
            } else {
                break;
            }
        }

        for (int x = Position.x + 1; x < grid.XSize; x++) {
            if (grid.GetGridValue(x, Position.y).IsEmpty()) {
                cells.Add(new Vector2Int(x, Position.y));
            } else {
                break;
            }
        }

        for (int y = Position.y + 1; y < grid.YSize; y++) {
            if (grid.GetGridValue(Position.x, y).IsEmpty()) {
                cells.Add(new Vector2Int(Position.x, y));
            } else {
                break;
            }
        }

        for (int y = Position.y - 1; y >= 0; y--) {
            if (grid.GetGridValue(Position.x, y).IsEmpty()) {
                cells.Add(new Vector2Int(Position.x, y));
            } else {
                break;
            }
        }

        return cells;
    }

    public override Queue<Vector2Int> GetPathToCell(Vector2Int cellPosition) {
        Queue<Vector2Int> cells = new Queue<Vector2Int>();
        if ((cellPosition - Position).x > 0) {
            for (int x = Position.x + 1; x <= cellPosition.x; x++) {
                cells.Enqueue(new Vector2Int(x, Position.y));
            }
        } else if ((cellPosition - Position).x < 0) {
            for (int x = Position.x - 1; x >= cellPosition.x; x--) {
                cells.Enqueue(new Vector2Int(x, Position.y));
            }
        } else if ((cellPosition - Position).y > 0) {
            for (int y = Position.y + 1; y <= cellPosition.y; y++) {
                cells.Enqueue(new Vector2Int(Position.x, y));
            }
        } else if ((cellPosition - Position).y < 0) {
            for (int y = Position.y - 1; y >= cellPosition.y; y--) {
                cells.Enqueue(new Vector2Int(Position.x, y));
            }
        }

        return cells;
    }

    public override Vector2Int? GetNextAttackCell(Vector3 playerPosition) {
        Vector2Int playerCell = grid.WorldToCell(playerPosition);

        List<Vector2Int> traversableCells = GetTraversableCells();

        if (traversableCells.Count <= 0) {
            return null;
        }

        List<Vector2Int> validCells = new List<Vector2Int>();

        if (playerCell.y >= Position.y) {
            validCells.AddRange(traversableCells.FindAll(
                cell => cell.x == Position.x && playerCell.y <= cell.y && cell.y <= playerCell.y + 2
            ));
        } else if (playerCell.y < Position.y) {
            validCells.AddRange(traversableCells.FindAll(
                cell => cell.x == Position.x && playerCell.y >= cell.y && cell.y >= playerCell.y - 2
            ));
        }
        
        if (playerCell.x >= Position.x) {
            validCells.AddRange(traversableCells.FindAll(
                cell => cell.y == Position.y && playerCell.x <= cell.x && cell.x <= playerCell.x + 2
            ));
        } else if (playerCell.x < Position.x) {
            validCells.AddRange(traversableCells.FindAll(
                cell => cell.y == Position.y && playerCell.x >= cell.x && cell.x >= playerCell.x - 2
            ));
        }

        return validCells.Find(
                cell => validCells.TrueForAll(
                    innerCell => Vector2.Distance(cell, Position) >= Vector2.Distance(innerCell, Position)
                )
            );
    }

    public override Vector2Int? GetNextMovementCell(Vector3 playerPosition) {
        Vector2Int playerCell = grid.WorldToCell(playerPosition);

        List<Vector2Int> traversableCells = GetTraversableCells();

        if (traversableCells.Count <= 0) {
            return null;
        }

        List<Vector2Int> optimalCells = traversableCells.FindAll(
                cell => 
                    (playerCell.x - 1 <= cell.x && cell.x <= playerCell.x + 1)
                    ||
                    (playerCell.y - 1 <= cell.y && cell.y <= playerCell.y + 1)
            );
        if (optimalCells.Count > 0) {
            // Get cell with minimum travel distance 
            // 
            // i.e. 
            // x ∈ optimalCells : ∀ c ∈ optimalCells . |x - playerPosition| <= |c - playerPosition|
            // where optimalCells ⊆ ℕ²
            return optimalCells.Find(
                cell => optimalCells.TrueForAll(
                    innerCell => Vector2.Distance(cell, Position) <= Vector2.Distance(innerCell, Position)
                    )
                ); 
        } else {
            return traversableCells.Find(
                cell => traversableCells.TrueForAll(
                    innerCell => Vector2.Distance(cell, playerCell) <= Vector2.Distance(innerCell, playerCell)
                    )
                );
        }
    }

    public override bool HasAttackOppurtunity(Vector3 playerPosition) {
        Vector2Int playerCell = grid.WorldToCell(playerPosition);

        List<Vector2Int> traversableCells = GetTraversableCells();

        return traversableCells.Exists(cell => playerCell.x - 1 <= cell.x && cell.x <= playerCell.x + 1 && playerCell.y - 1 <= cell.y && cell.y <= playerCell.y + 1);
    }
}
