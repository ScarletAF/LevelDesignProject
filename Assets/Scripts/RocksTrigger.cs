using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RocksTrigger : MonoBehaviour
{
    [SerializeField] private RocksFalling rocks;

    private bool readied = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && readied)
        {
            rocks.Fall();
            readied = false;
        }
        else if (other.CompareTag("Rocks"))
        {
            Destroy(other.gameObject, .3f);
        }
    }
}
