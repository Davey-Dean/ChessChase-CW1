using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour
{
    public Vector2Int Position {get; protected set;}
    protected EnemyGrid grid;
    public virtual void Init(EnemyGrid grid) {
        this.grid = grid;
        Position = this.grid.WorldToCell(this.gameObject.transform.position);
    }

    public abstract Queue<Vector2Int> GetPathToCell(Vector2Int cellPosition);

    public abstract Vector2Int? GetNextAttackCell(Vector3 playerPosition);
    public abstract Vector2Int? GetNextMovementCell(Vector3 playerPosition);

    public Vector2Int GetPosition() {
        return Position;
    }

    public void SetPosition(Vector2Int newPosition) {
        Position = newPosition;
    }

    public abstract bool HasAttackOppurtunity(Vector3 playerPosition);
}
