using System;
using System.Threading.Tasks;

public class GameStateService : IDisposable
{
    private PlayerPresenter _playerPresenter;
    private PauseButtonPresenter _pauseButtonPresenter;
    private PauseMenuPresenter _pauseMenuPresenter;
    private CrashedMenuPresenter _crashedMenuPresenter;
    private CoroutineHandler _coroutineHandler;
    private InputHandler _inputHandler;
    private PoolHandler _poolHandler;

    public event Action OnRestartGame;
    public event Action OnPauseGame;
    public event Action OnContinueGame;

    public event Action OnCrashedEvent;
    




    public GameStateService(PlayerPresenter playerPresenter,
    PauseButtonPresenter pauseButtonPresenter, 
    PauseMenuPresenter pauseMenuPresenter,
    CrashedMenuPresenter crashedMenuPresenter, 
    CoroutineHandler coroutineHandler, 
    InputHandler inputHandler,
    PoolHandler poolHandler)
    {
        _playerPresenter = playerPresenter;
        _pauseButtonPresenter = pauseButtonPresenter;
        _pauseMenuPresenter = pauseMenuPresenter;
        _crashedMenuPresenter = crashedMenuPresenter;
        _coroutineHandler = coroutineHandler;
        _inputHandler = inputHandler;
        _poolHandler = poolHandler;
    }

    public void Init()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        _pauseButtonPresenter.OnPause += OnPause;
        _pauseMenuPresenter.OnContinue += OnContinue;
        _pauseMenuPresenter.OnRestart += OnRestart;
        _playerPresenter.OnLacksShield += OnEndGame;
        //_playerPresenter.OnLacksShield += TriggerCrashed;


        _crashedMenuPresenter.OnRestart += OnRestart;
    }

    private void UnSubscribe()
    {
        _pauseButtonPresenter.OnPause -= OnPause;
        _pauseMenuPresenter.OnContinue -= OnContinue;
        _pauseMenuPresenter.OnRestart -= OnRestart;
        _playerPresenter.OnLacksShield -= OnEndGame;
        //_playerPresenter.OnLacksShield -= TriggerCrashed;


        _crashedMenuPresenter.OnRestart -= OnRestart;
    }

    private void OnPause()
    {
        _pauseMenuPresenter.OnPause();
        _coroutineHandler.StopTrackCoroutine();
        _inputHandler.StopInputHandle();

        OnPauseGame?.Invoke();
    }

    private void OnContinue()
    {
        _coroutineHandler.StartTrackCoroutine();
        _inputHandler.StartInputHandle();

        OnContinueGame?.Invoke();
    }

    private async void OnRestart()
    {
        _coroutineHandler.StopTrackCoroutine();
        _inputHandler.StopInputHandle();

        OnRestartGame?.Invoke();
        
        await WaitForRestart();

        

        OnContinue();
    }

    private void OnEndGame()
    {
        _coroutineHandler.StopTrackCoroutine();
        _inputHandler.StopInputHandle();
        TriggerCrashed();
    }

    private void TriggerCrashed() => OnCrashedEvent?.Invoke();

    private async Task WaitForRestart() => await Task.Delay(3000);

    public void Dispose()
    {
        UnSubscribe();
    }
}
