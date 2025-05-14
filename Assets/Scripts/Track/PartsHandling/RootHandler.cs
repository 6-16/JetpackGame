using System;
using System.Collections.Generic;
using UnityEngine;

public class RootHandler : MonoBehaviour
{
    // [SerializeField] private ObstacleCollisionHandler _obstacleCollisionHandler;
    [SerializeField] private List <ObstacleCollisionHandler> _obstaclesList;
    [SerializeField] private List <CollectibleCollisionHandler> _collectiblesList;

    private PoolHandler _poolHandler;

    //private List <ObstacleCollisionHandler> _activeObstacles = new();
    //private List <CollectibleCollisionHandler> _activeCollectibles = new();

    private List <Action> _obstaclesDelegates = new();
    private List <Action> _collectiblesDelegates = new();


    public event Action <ObstacleCollisionHandler> OnObstacleCollision;
    public event Action <CollectibleCollisionHandler> OnCollectibleCollision;


    public void Init(PoolHandler poolHandler)
    {
        _poolHandler = poolHandler;
    }

    private void OnEnable()
    {
        if (_obstaclesList == null || _collectiblesList == null) return;

        foreach (var obstacle in _obstaclesList)
        {
            obstacle.gameObject.SetActive(true);
        }

        foreach (var collectible in _collectiblesList)
        {
            collectible.gameObject.SetActive(true);
        }

        foreach (var obstacle in _obstaclesList)
        {
            var current = obstacle;


            Action obstacleHandler = () => OnObstacle(current);
            _obstaclesDelegates.Add(obstacleHandler);
            current.OnCollisionEvent += obstacleHandler;

            //_activeObstacles.Add(current);
        }

        foreach (var collectible in _collectiblesList)
        {
            var current = collectible;
            

            Action collectibleHandler = () => OnCollectible(current);
            _collectiblesDelegates.Add(collectibleHandler);
            current.OnCollisionEvent += collectibleHandler;

            //_activeCollectibles.Add(current);
        }

        // _obstacleCollisionHandler.OnCollisionEvent += OnObstacle;
        _poolHandler.OnObstacleCollision += HandleObstacle;
        _poolHandler.OnShieldEvent += HandleShield;

    }

    private void OnDisable()
    {
        if (_obstaclesList == null || _collectiblesList == null) return;

        for (int i = 0; i < _obstaclesList.Count; i++)
        {
            _obstaclesList[i].OnCollisionEvent -= _obstaclesDelegates[i];
        }
        _obstaclesDelegates.Clear();
        //_activeObstacles.Clear();

        for (int i = 0; i < _collectiblesList.Count; i++)
        {
            _collectiblesList[i].OnCollisionEvent -= _collectiblesDelegates[i];
        }
        _collectiblesDelegates.Clear();
        //_activeCollectibles.Clear();

        // _obstaclesList.OnCollisionEvent -= OnObstacle;
        if (_poolHandler != null)
        {
            _poolHandler.OnObstacleCollision -= HandleObstacle;
            _poolHandler.OnShieldEvent -= HandleShield;
        }
    }

    private void HandleObstacle(ObstacleCollisionHandler obstacle)
    {
        //if (_activeObstacles.Contains(obstacle))
        //{
            obstacle.gameObject.SetActive(false);
        //}
    }

    private void HandleShield(CollectibleCollisionHandler collected)
    {
        //if (_activeCollectibles.Contains(collected))
        //{
            collected.gameObject.SetActive(false);
        //}
    }

    private void OnObstacle(ObstacleCollisionHandler obstacle)
    {
        OnObstacleCollision?.Invoke(obstacle);
    }

    private void OnCollectible(CollectibleCollisionHandler collectible)
    {
        OnCollectibleCollision?.Invoke(collectible);
    }
}
