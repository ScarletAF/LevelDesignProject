using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class KeyLock : MonoBehaviour
{
    [SerializeField] private Door door;
    [SerializeField] private int keyID;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerState>().HasKey(keyID))
                door.Open();
        }
    }
}
