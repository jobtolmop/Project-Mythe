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
        originalPos = transform.position;
        posToMove = movePos.position;
        obstacle = GetComponent<NavMeshObstacle>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").transform;        
    }
    
    void Update()
    {
        if (enemy != null && (enemy.position - transform.position).sqrMagnitude < 25)
        {
            obstacle.enabled = true;
        }
        else
        {
            obstacle.enabled = false;
        }

        if (button.ButtonPressed || (enemy != null && (enemy.position - transform.position).sqrMagnitude < 100))
        {
            transform.position = Vector3.MoveTowards(transform.position, posToMove, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPos, speed * Time.deltaTime);
        }
    }
}
