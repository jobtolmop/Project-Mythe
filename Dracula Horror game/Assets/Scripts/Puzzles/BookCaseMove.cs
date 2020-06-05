using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCaseMove : MonoBehaviour
{
    private Vector3 originalPos;
    [SerializeField] private WeightedButton button;
    [SerializeField] private Transform movePos;
    [SerializeField] private Transform enemy;
    private Vector3 posToMove;

    [SerializeField] private float speed = 1;

    private void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
        originalPos = transform.position;
        posToMove = movePos.position;
    }
    
    void FixedUpdate()
    {
        if (button.ButtonPressed || (enemy.position - transform.position).sqrMagnitude < 50)
        {
            transform.position = Vector3.MoveTowards(transform.position, posToMove, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPos, speed * Time.deltaTime);
        }
    }
}
