using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Transform playerSpawn;

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

    public static GameManager GetInstance()
    {
        return instance;
    }

    public static Transform GetPlayerTransform()
    {
        var GM = GetInstance();
        return GM.player.transform;
    }

    public static Vector3 PredictPlayerMovement()
    {
        var GM = GetInstance();
        return GM.player.transform.position;
    }

    public static Vector3 GetPlayerSpawn()
    {
        var GM = GetInstance();
        return GM.playerSpawn.position;
    }
}