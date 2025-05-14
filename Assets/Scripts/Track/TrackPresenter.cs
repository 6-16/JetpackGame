using System;
using UnityEngine;

public class TrackPresenter : IDisposable
{
    private ServiceManager _serviceManager;
    private GameStateService _gameStateService;
    private TrackModel _trackModel;
    private TrackView _trackView;
    private PoolHandler _poolHandler;



    public TrackPresenter(TrackModel model, TrackView view, PoolHandler poolHandler)
    {
        _trackModel = model;
        _trackView = view;
        _poolHandler = poolHandler;
    }

    public void Init()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        _serviceManager.OnTrackCoroutine += MoveTrack;
        _gameStateService.OnRestartGame += ResetTrack;
    }

    private void UnSubscribe()
    {
        _serviceManager.OnTrackCoroutine -= MoveTrack;
        _gameStateService.OnRestartGame -= ResetTrack;
    }

    public void SetServices(ServiceManager serviceManager, GameStateService gameStateService)
    {
        SetServiceManager(serviceManager);
        SetGameStateService(gameStateService);
    }

    private void SetServiceManager(ServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    private void SetGameStateService(GameStateService gameStateService)
    {
        _gameStateService = gameStateService;
    }

    private void MoveTrack()
    {
        float moveAmount = _trackModel.TrackSpeed * Time.fixedDeltaTime;
        _poolHandler.UpdateTrack(moveAmount);
    }

    private void ResetTrack()
    {
        _poolHandler.Reset();
    }

    public void Disable() => UnSubscribe();

    public void Dispose()
    {
        UnSubscribe();
        _trackModel = null;
        _trackView = null;
    }
}