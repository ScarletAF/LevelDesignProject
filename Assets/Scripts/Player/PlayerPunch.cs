using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPunch : MonoBehaviour
{
    [SerializeField]
    private Fist fist;

    private void OnFire(InputValue value)
    {
        fist.Punch();
    }
}
