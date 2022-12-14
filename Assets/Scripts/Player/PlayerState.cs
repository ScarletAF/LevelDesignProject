using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField]
    private List<int> keyList;

    private void Start()
    {
        keyList = new List<int>();
    }

    public bool HasKey(int keyID)
    {
        return keyList.Contains(keyID);
    }

    public void AddKey(int keyID)
    {
        keyList.Add(keyID);
    }

    public void Respawn()
    {
        transform.position = GameManager.GetPlayerSpawn();
    }
}
