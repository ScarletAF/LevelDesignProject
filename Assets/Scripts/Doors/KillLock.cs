using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillLock : MonoBehaviour
{
    [SerializeField] private Door door;
    [SerializeField] private List<EnemyController> enemies;

    public void ReportSpawn(EnemyController enemy)
    {
        enemies.Add(enemy);
        if (enemies.Count == 1)
        {
            door.Close();
        }
    }

    public void ReportDeath(EnemyController enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count == 0)
        {
            door.Open();
        }
    }
}
