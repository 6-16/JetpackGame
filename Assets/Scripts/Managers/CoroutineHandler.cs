using System;
using System.Collections;
using UnityEngine;

public class CoroutineHandler : MonoBehaviour
{
    public event Action OnTrackMoveCoroutine;

    private Coroutine _trackMoveCoroutine;


    public void StartTrackCoroutine()
    {
        _trackMoveCoroutine = StartCoroutine(TrackMoveLoop());
    }

    public void StopTrackCoroutine()
    {
        StopCoroutine(_trackMoveCoroutine);
    }

    private IEnumerator TrackMoveLoop()
    {
        while (true)
        {
            OnTrackMoveCoroutine?.Invoke();
            yield return new WaitForFixedUpdate();
        }
    }
}
