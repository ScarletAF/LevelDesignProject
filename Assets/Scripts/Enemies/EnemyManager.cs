using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager instance;

    [SerializeField]
    private List<Spawner> spawns;
    [SerializeField]
    private List<KillLock> killLocks;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static EnemyManager GetInstance()
    {
        return instance;
    }

    public static void ReportDeath(EnemyController enemy)
    {
        EnemyManager EM = GetInstance();
        foreach (KillLock door in EM.killLocks)
        {
            door.ReportDeath(enemy);
        }

        Destroy(enemy.gameObject);
    }

}
