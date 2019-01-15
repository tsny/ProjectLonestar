using UnityEngine;
using System;
using UnityEngine.Events;

public class SelectiveTrigger : MonoBehaviour
{
    public bool destroyOnTrigger = true;
    public UnityEvent onTrigger;
    public string[] validTag = {"Player"};

    private Collider coll;

    private void Awake()
    {
        coll = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (var tag in validTag)
        {
            if (tag == other.tag)
            {
                onTrigger.Invoke();
                return;
            }
        }

        Destroy(gameObject, 2);
    }
}