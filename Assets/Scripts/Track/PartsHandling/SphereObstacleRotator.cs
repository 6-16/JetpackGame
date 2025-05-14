using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class SphereObstacleRotator : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 90f;




    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    private void FixedUpdate()
    {
        transform.Rotate(0f, 0f, -_rotationSpeed * Time.deltaTime);
    }

    // while (_isActive)
    // {
    //     transform.Rotate(0f, 0f, -_rotationSpeed * Time.deltaTime);
    //     await Task.Yield();
    // }
}

