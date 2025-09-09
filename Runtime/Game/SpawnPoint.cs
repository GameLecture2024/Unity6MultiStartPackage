using System.Collections.Generic;
using UnityEngine;
using NetworkB;

public class SpawnPoint : MonoBehaviour
{
    private static List<SpawnPoint> spawnPoints = new();

    private static Vector3 defaultPos = new Vector3(-7,0,0);

    private void OnEnable()
    {
        spawnPoints.Add(this);
    }

    private void OnDisable()
    {
        spawnPoints.Remove(this);
    }

    public static Vector3 GetSpawonPos()
    {
        if (spawnPoints.Count == 0) { return defaultPos; }

        while(true)
        {
            Vector3 SpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position;

            Collider2D[] collide = Physics2D.OverlapCircleAll(SpawnPoint, 1);

            if (collide.Length == 0)
            {
                return SpawnPoint;
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}
