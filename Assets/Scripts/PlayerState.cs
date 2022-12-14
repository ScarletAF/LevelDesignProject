using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private List<int> keyList;

    private void Start()
    {
        keyList = new List<int> { 1 };
    }


    public bool HasKey(int keyID)
    {
        return keyList.Contains(keyID);
    }
}
