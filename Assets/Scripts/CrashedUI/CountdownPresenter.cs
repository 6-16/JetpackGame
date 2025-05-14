using System;
using System.Threading.Tasks;

public class CountdownPresenter : IDisposable
{
    private CountdownView _countdownView;
    private GameStateService _gameStateService;




    public CountdownPresenter(CountdownView view)
    {
        _countdownView = view;
    }

    public void Init() => Subscribe();


    private void Subscribe()
    {
        _gameStateService.OnRestartGame += StartCountdown;
    }

    private void UnSubscribe()
    {
        _gameStateService.OnRestartGame -= StartCountdown;
    }

    private async void StartCountdown()
    {
        _countdownView.ToggleCountdownText();

        _countdownView.SetCountdownText("3");
        await Task.Delay(1000);

        _countdownView.SetCountdownText("2");
        await Task.Delay(1000);

        _countdownView.SetCountdownText("1");
        await Task.Delay(1000);

        _countdownView.SetCountdownText("0");

        _countdownView.ToggleCountdownText();
    }

    public void SetServices(GameStateService gameStateService)
    {
        SetGameStateService(gameStateService);
    }
    private void SetGameStateService(GameStateService gameStateService)
    {
        _gameStateService = gameStateService;
    }

    public void Disable()
    {

    }

    public void Dispose()
    {
        UnSubscribe();
    }

}
