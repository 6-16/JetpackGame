using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolHandler
{
    // private const float SPAWN_X = 35.6f;
    private const float OFFSCREEN_X = 17.8f;

    private ServiceManager _serviceManager;

    private List<RootHandler> _allParts;
    private List<RootHandler> _availablePool = new();
    private Queue<RootHandler> _activeParts = new();

    private Transform _parent;

    public event Action <ObstacleCollisionHandler> OnCollisionEvent;
    public event Action <ObstacleCollisionHandler> OnObstacleCollision;
    public event Action <CollectibleCollisionHandler> OnCollectibleEvent;
    public event Action <CollectibleCollisionHandler> OnShieldEvent;



    public void Init(List<RootHandler> allParts, ServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
        Subscribe();


        _allParts = allParts;

        if (_allParts.Count < 3)
        {
            return;
        }

        _parent = _allParts[0].transform.parent;

        foreach (var part in _allParts)
        {
            part.gameObject.SetActive(false);
            part.Init(this);
            _availablePool.Add(part);
        }

        for (int i = 0; i < 3; i++)
        {
            PlaceNextPart(i * OFFSCREEN_X); 
        }
    }

    private void Subscribe()
    {
        _serviceManager.OnShield += OnShieldApplied;
        _serviceManager.DeactivateObstacleEvent += TriggerObstacleDeactivate;
    }

    private void UnSubscribe()
    {
        _serviceManager.OnShield -= OnShieldApplied;
    }

    private void OnShieldApplied()
    {

    }

    private void TriggerObstacleDeactivate(ObstacleCollisionHandler obstacle)
    {
        OnObstacleCollision?.Invoke(obstacle);
    }

    public void TriggerShieldCollected(CollectibleCollisionHandler collectible)
    {
        OnShieldEvent?.Invoke(collectible);
    }

    public void Reset()
    {
        while (_activeParts.Count > 0)
        {
            var part = _activeParts.Dequeue();
            part.gameObject.SetActive(false);
            _availablePool.Add(part);
        }

        for (int i = 0; i < 3; i++)
        {
            PlaceNextPart(i * OFFSCREEN_X);
        }
    }

    public void UpdateTrack(float moveAmount)
    {
        foreach (var part in _activeParts)
        {
            part.transform.position += Vector3.left * moveAmount;
        }

        var leftMost = _activeParts.Peek();

        if (leftMost.transform.position.x < -OFFSCREEN_X)
        {
            var old = _activeParts.Dequeue();
            old.OnObstacleCollision -= OnCollision;
            old.OnCollectibleCollision -= OnCollectible;
            old.gameObject.SetActive(false);
            _availablePool.Add(old);

            PlaceNextPart(OFFSCREEN_X * 2);
        }
    }

    private void PlaceNextPart(float spawnX)
    {
        if (_availablePool.Count == 0)
        {
            return;
        }


        int randomIndex = UnityEngine.Random.Range(0, _availablePool.Count);
        var next = _availablePool[randomIndex];
        _availablePool.RemoveAt(randomIndex);
        next.OnObstacleCollision += OnCollision;
        next.OnCollectibleCollision += OnCollectible;

        next.transform.position = new Vector3(spawnX, 0f, 0f);
        next.gameObject.SetActive(true);
        _activeParts.Enqueue(next);
    }

    private void OnCollision(ObstacleCollisionHandler obstacle)
    {
        OnCollisionEvent?.Invoke(obstacle);
    }

    private void OnCollectible(CollectibleCollisionHandler collectible)
    {
        OnCollectibleEvent?.Invoke(collectible);
    }
}

