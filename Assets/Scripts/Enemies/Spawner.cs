using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private List<Transform> spawnPoints;
    [SerializeField]
    private GameObject enemyPrefab;

    [Tooltip("Assign lock if spawns should count to enemies that must be destroyed before the lock opens.")]
    [SerializeField]
    private KillLock killLock;

    public void Spawn()
    {
        foreach (Transform spawn in spawnPoints)
        {
            SpawnEnemy(spawn);
        }
    }

    private void SpawnEnemy(Transform spawn)
    {
        var enemy = Instantiate(enemyPrefab, spawn.position, spawn.rotation);

        if (killLock != null)
            killLock.ReportSpawn(enemy.GetComponent<EnemyController>());
    }
}
