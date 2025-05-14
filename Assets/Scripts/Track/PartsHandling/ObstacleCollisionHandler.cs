using UnityEngine;
using System;

public class ObstacleCollisionHandler : MonoBehaviour
{

    public event Action OnCollisionEvent;






    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnCollisionEvent?.Invoke();
    }
}
