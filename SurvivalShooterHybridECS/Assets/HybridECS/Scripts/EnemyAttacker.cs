using UnityEngine;

public class EnemyAttacker : MonoBehaviour
{
    public float Timer;
    public bool PlayerInRange { get; private set; }

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            PlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            PlayerInRange = false;
        }
    }
}
