using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InvisibleDoor : MonoBehaviour
{
    [SerializeField]
    private float revealRate;

    private new Collider collider;
    private new Renderer renderer;

    private void Start()
    {
        collider = GetComponent<Collider>();
        renderer = GetComponent<Renderer>();
    }

    public  void Appear()
    {
        collider.enabled = false;
        StartCoroutine(RevealDoor());
    }

    private IEnumerator RevealDoor()
    {
        while (renderer.material.color.a > 0)
        {
            Color color = renderer.material.color;
            color.a -= revealRate * Time.deltaTime;
            renderer.material.color = color;
            yield return null;
        }
    }
}
