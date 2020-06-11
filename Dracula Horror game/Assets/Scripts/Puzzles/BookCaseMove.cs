using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BookCaseMove : MonoBehaviour
{
    private Vector3 originalPos;
    [SerializeField] private WeightedButton button;
    [SerializeField] private Transform movePos;
    [SerializeField] private Transform enemy;
    private Vector3 posToMove;

    [SerializeField] private float speed = 1;
    private NavMeshObstacle obstacle;

    private void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
        originalPos = transform.position;
        posToMove = movePos.position;
        obstacle = GetComponent<NavMeshObstacle>();
    }
    
    void FixedUpdate()
    {
        bool enemyClose = false;

        if ((enemy.position - transform.position).sqrMagnitude < 100)
        {
            obstacle.enabled = true;
            enemyClose = true;
        }
        else
        {
            obstacle.enabled = false;
        }

        if (button.ButtonPressed || enemyClose)
        {
            transform.position = Vector3.MoveTowards(transform.position, posToMove, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPos, speed * Time.deltaTime);
        }
    }
}
