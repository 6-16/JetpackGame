using System;
using UnityEngine;

public class PauseMenuPresenter : IDisposable
{
    private PauseMenuView _pauseMenuView;
    private ServiceManager _serviceManager;


    public event Action OnContinue;
    public event Action OnRestart;


    public PauseMenuPresenter()
    {

    }



    public void Init(PauseMenuView view)
    {
        _pauseMenuView = view;
        Subscribe();
    }

    private void Subscribe()
    {
        _pauseMenuView.OnContinue += Continue;
        _pauseMenuView.OnRestart += Restart;
    }

    private void UnSubscribe()
    {
        _pauseMenuView.OnContinue -= Continue;
        _pauseMenuView.OnRestart -= Restart;
    }

    public void OnPause()
    {
        _pauseMenuView.SetWindowActive(true);
    }

    private void Continue()
    {
        OnContinue?.Invoke();
        _pauseMenuView.SetWindowActive(false);
    }

    private void Restart()
    {
        OnRestart?.Invoke();
        _pauseMenuView.SetWindowActive(false);
    }

    public void Disable()
    {

    }

    public void Dispose()
    {
        UnSubscribe();

    }

}
