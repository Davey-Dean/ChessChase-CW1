using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
{
    public float attackInterval;

    public float moveInterval;

    public Transform playerTransform;

    private EnemyMovement mover;

    public EnemyGrid grid;

    private Transform tf;

    public float moveSpeed;
    public float attackSpeed;

    public float attackPauseTime;
    private bool moving = false;
    private Queue<Vector2Int> moveQueue = new Queue<Vector2Int>();
    private bool moveAvailable = true;
    private bool attackAvailable = true;

    private bool inAttackMode = false;

    private Vector2Int initialPosition;

    public bool startEnabled;

    void Start() {
        tf = this.gameObject.transform;

        mover = GetComponent<EnemyMovement>();
        mover.Init(grid);
        grid.ReserveGridCell(mover.GetPosition().x, mover.GetPosition().y, this);
        initialPosition = mover.GetPosition();
        this.enabled = false;

        if (startEnabled) {
            Enable();
        }
    }

    void Update() {
        if (!moving && moveQueue.Count != 0) {
            if (inAttackMode) {
                StartCoroutine(MoveToCell(moveQueue.Dequeue(), attackSpeed));
                if (moveQueue.Count == 0) {
                    StartCoroutine(AttackCooldown());
                    StartCoroutine(MoveCooldown());
                    inAttackMode = false;
                }
            } else {
                StartCoroutine(MoveToCell(moveQueue.Dequeue(), moveSpeed));
                if (moveQueue.Count == 0) {
                    StartCoroutine(MoveCooldown());
                }
            }
        }

        if (moveAvailable && !moving) {
            if (attackAvailable && mover.HasAttackOppurtunity(playerTransform.position)) {
                Vector2Int? nextCell = mover.GetNextAttackCell(playerTransform.position);
                if (nextCell.HasValue) {
                    Queue<Vector2Int> newQueue = mover.GetPathToCell(nextCell.Value);
                    bool free = true;
                    foreach (var cell in newQueue) {
                        if (!grid.GetGridValue(cell.x, cell.y).IsEmpty()) {
                            free = false;
                        } 
                    }
                    
                    if (free) {
                        moveQueue = newQueue;
                        foreach (var cell in moveQueue) {
                            grid.ReserveGridCell(cell.x, cell.y, this);
                        }

                        var audio = gameObject.GetComponent<EnemyAudio>();
                        if(audio) {
                            audio.PlayReservePathSound(tf.position);
                        }

                        moveAvailable = false;
                        attackAvailable = false;
                        inAttackMode = true;
                        StartCoroutine(AttackPause());
                    } 
                }
            } else {
                Vector2Int? nextCell = mover.GetNextMovementCell(playerTransform.position);
                if (nextCell.HasValue) {
                    Queue<Vector2Int> newQueue = mover.GetPathToCell(nextCell.Value);
                    bool free = true;
                    foreach (var cell in newQueue) {
                        if (!grid.GetGridValue(cell.x, cell.y).IsEmpty()) {
                            free = false;
                        } 
                    }
                    
                    if (free) {
                        moveQueue = newQueue;
                        foreach (var cell in moveQueue) {
                            grid.ReserveGridCell(cell.x, cell.y, this);
                        }
                        moveAvailable = false;
                    } 
                }
            }
        }
    }

    public void Reset() {
        this.StopAllCoroutines();
        moving = true;
        mover.SetPosition(initialPosition);
        grid.ReserveGridCell(mover.GetPosition().x, mover.GetPosition().y, this);
        moveQueue = new Queue<Vector2Int>();
        moveAvailable = true;
        attackAvailable = true;
        inAttackMode = false;
        StartCoroutine(MoveToCell(mover.GetPosition(), 10000));
    }

    public void Enable() {
        this.enabled = true;
        this.Reset();
    }

    public void Disable() {
        Reset();
        this.StopAllCoroutines();
        this.enabled = false;
    }

    private IEnumerator MoveCooldown() {
        yield return new WaitForSeconds(moveInterval);
        moveAvailable = true;
    }

    private IEnumerator AttackCooldown() {
        yield return new WaitForSeconds(attackInterval);
        attackAvailable = true;
    }

    private IEnumerator AttackPause() {
        moving = true;
        yield return new WaitForSeconds(attackPauseTime);
        moving = false;
        var audio = gameObject.GetComponent<EnemyAudio>();
        if(audio) {
            audio.PlayAttackSound(tf.position);
        }
    }

    private IEnumerator MoveToCell(Vector2Int cell, float speed) {
        moving = true;
        grid.ReleaseReservation(mover.Position.x, mover.Position.y, this);
        Vector3 targetPosition = grid.CellToWorld(cell);
        while ((targetPosition - tf.position).magnitude > speed/100f) {
            tf.position = Vector3.MoveTowards(tf.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
        tf.position = targetPosition;
        moving = false;
        mover.SetPosition(cell);
        grid.ClearCell(cell);
    }
}
