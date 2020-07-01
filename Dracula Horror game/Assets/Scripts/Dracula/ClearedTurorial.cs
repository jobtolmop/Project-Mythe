using UnityEngine;

public class ClearedTurorial : MonoBehaviour
{
    private EnemyDestinationChooser chooser;

    // Start is called before the first frame update
    void Start()
    {
        chooser = FindObjectOfType<EnemyDestinationChooser>(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            chooser.DontGoAfterPlayer = false;
        }
    }
}
