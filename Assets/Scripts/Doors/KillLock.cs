using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillLock : MonoBehaviour
{
    [SerializeField] private Door door;
    [SerializeField] private List<GameObject> enemies;

    public void ReportDeath(GameObject enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count == 0)
        {
            door.Open();
        }
    }
}
