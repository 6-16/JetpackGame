using UnityEngine;
using System;

public class CollectibleCollisionHandler : MonoBehaviour
{
    public event Action OnCollisionEvent;







    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnCollisionEvent?.Invoke();
    }
}
