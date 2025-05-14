using System;

public class ServiceManager : IDisposable
{
    private PlayerPresenter _playerPresenter;
    private TrackPresenter _trackPresenter;

    private CoroutineHandler _coroutineHandler;
    private PoolHandler _poolHandler;

    public event Action OnTrackCoroutine;
    public event Action <ObstacleCollisionHandler> OnObstacle;
    public event Action <ObstacleCollisionHandler> DeactivateObstacleEvent;
    public event Action <CollectibleCollisionHandler> OnCollectible;
    public event Action OnShield;


    public ServiceManager(PlayerPresenter playerPresenter, TrackPresenter trackPresenter, CoroutineHandler coroutineHandler, PoolHandler poolHandler)
    {
        _playerPresenter = playerPresenter;
        _trackPresenter = trackPresenter;
        _coroutineHandler = coroutineHandler;
        _poolHandler = poolHandler;
    }

    public void Init()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        _coroutineHandler.OnTrackMoveCoroutine += OnTrackCoroutineReceived;
        _poolHandler.OnCollisionEvent += OnObstacleCollision;
        _poolHandler.OnCollectibleEvent += OnCollectibleCollision;
        _playerPresenter.OnShieldCollected += OnShieldApplied;
        _playerPresenter.OnHasShield += DeactivateObstacle;
    }

    private void UnSubscribe()
    {
        _coroutineHandler.OnTrackMoveCoroutine -= OnTrackCoroutineReceived;
        _poolHandler.OnCollisionEvent -= OnObstacleCollision;
        _poolHandler.OnCollectibleEvent -= OnCollectibleCollision;
        _playerPresenter.OnShieldCollected -= OnShieldApplied;
        _playerPresenter.OnHasShield -= DeactivateObstacle;
    }

    private void DeactivateObstacle(ObstacleCollisionHandler obstacle)
    {
        DeactivateObstacleEvent?.Invoke(obstacle);
    }

    internal void DeactivateShield(CollectibleCollisionHandler collectible)
    {
        _poolHandler.TriggerShieldCollected(collectible);
    }

    private void OnTrackCoroutineReceived() => OnTrackCoroutine?.Invoke();
    private void OnObstacleCollision(ObstacleCollisionHandler obstacle) => OnObstacle?.Invoke(obstacle);
    private void OnCollectibleCollision(CollectibleCollisionHandler collectible) => OnCollectible?.Invoke(collectible);
    private void OnShieldApplied() => OnShield?.Invoke();


    public void Disable() => UnSubscribe();

    public void Dispose()
    {
        UnSubscribe();
        _playerPresenter = null;
        _trackPresenter = null;
        _coroutineHandler = null;
        _poolHandler = null;
    }
}
