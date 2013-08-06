using UnityEngine;
using System.Collections;

public class EnemyAwareness : MonoBehaviour 
{
    public Enemy myEnemy;
    public Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
            Debug.LogError("ERROR: There must be an object tagged \"Player\" in the scene.");
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
            myEnemy.atRisk = true;
    }

    // Whenever the player has left...
    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
            myEnemy.atRisk = false;
    }
}