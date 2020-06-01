using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCaseMove : MonoBehaviour
{
    private Vector3 originalPos;
    [SerializeField] private WeightedButton button;
    [SerializeField] private Transform movePos;
    private Vector3 posToMove;

    [SerializeField] private float speed = 1;

    private void Start()
    {
        originalPos = transform.position;
        posToMove = movePos.position;
    }
    
    void FixedUpdate()
    {
        if (button.ButtonPressed)
        {
            transform.position = Vector3.MoveTowards(transform.position, posToMove, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPos, speed * Time.deltaTime);
        }
    }
}
