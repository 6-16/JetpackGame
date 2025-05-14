using System;
using UnityEngine;

public class PauseButtonPresenter : IDisposable
{
    private PauseButtonView _pauseButtonView;
    private GameStateService _gameStateService;


    public event Action OnPause;


    public PauseButtonPresenter(PauseButtonView view)
    {
        _pauseButtonView = view;
    }

    public void Init()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        _pauseButtonView.OnPause += Pause;
    }

    private void UnSubscribe()
    {
        _pauseButtonView.OnPause -= Pause;
    }

    private void Pause()
    {
        OnPause?.Invoke();
    }

    public void Disable()
    {

    }

    public void Dispose()
    {
        UnSubscribe();
    }
}
