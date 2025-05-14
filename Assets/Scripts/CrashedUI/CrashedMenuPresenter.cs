using System;

public class CrashedMenuPresenter : IDisposable
{
    private ServiceManager _serviceManager;
    private GameStateService _gameStateService;
    private CrashedMenuView _crashedMenuView;

    public event Action OnRestart;




    public CrashedMenuPresenter(CrashedMenuView view)
    {
        _crashedMenuView = view;
    }



    public void Init()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        _crashedMenuView.OnRestart += Restart;
        _gameStateService.OnCrashedEvent += TriggerCrashedPanel;
    }

    private void UnSubscribe()
    {
        _crashedMenuView.OnRestart -= Restart;
        _gameStateService.OnCrashedEvent -= TriggerCrashedPanel;
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

    private void Restart()
    {
        OnRestart?.Invoke();
        _crashedMenuView.SetWindowActive(false);
    }

    private void TriggerCrashedPanel()
    {
        _crashedMenuView.SetWindowActive(true);
    }

    public void Disable()
    {

    }

    public void Dispose()
    {
        UnSubscribe();
    }
}
