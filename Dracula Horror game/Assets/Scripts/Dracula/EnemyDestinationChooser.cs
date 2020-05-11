using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestinationChooser : MonoBehaviour
{
    public Vector3 TargetPos { get; set; }

    private EnemyPlayerSpotter spotter;
    //private EnemyPathFinding pathFinding;

    [SerializeField] private LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        spotter = GetComponent<EnemyPlayerSpotter>();
        //pathFinding = GetComponent<EnemyPathFinding>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spotter.PlayerSpotted)
        {
            TargetPos = spotter.Player.position;
        }   
        else
        {
            ChooseRandomLocation();
        }
    }

    private void ChooseRandomLocation()
    {
        TargetPos = Vector3.zero;
    }
}
