using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookcaseCrush : MonoBehaviour
{
    [SerializeField] private PlayerCrush crush;

    private bool alreadyDead = false;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerCol") && crush.PlayerAgainstWall && !alreadyDead)
        {
            alreadyDead = true;
            collision.gameObject.transform.parent.GetComponentInChildren<PlayerHealth>().Die(null, true);
        }
    }
}
