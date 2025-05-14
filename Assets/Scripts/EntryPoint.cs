using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    // Services / Handlers
    private ServiceManager _serviceManager;
    private GameStateService _gameStateService;
    private InputHandler _inputHandler;
    private PoolHandler _poolHandler;
    private CoroutineHandler _coroutineHandler;
    

    // Views
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private TrackView _trackView;
    [SerializeField] private PauseMenuView _pauseMenuView;
    [SerializeField] private PauseButtonView _pauseButtonView;
    [SerializeField] private CrashedMenuView _crashedMenuView;
    [SerializeField] private CountdownView _countdownView;


    // Presenters
    private PlayerPresenter _playerPresenter;
    private TrackPresenter _trackPresenter;
    private PauseMenuPresenter _pauseMenuPresenter;
    private PauseButtonPresenter _pauseButtonPresenter;
    private CrashedMenuPresenter _crashedMenuPresenter;
    private CountdownPresenter _countdownPresenter;





    private void Start()
    {
        TrackModel trackModel = new TrackModel();

        _coroutineHandler = GetComponent<CoroutineHandler>();

        _poolHandler = new PoolHandler();
        _inputHandler = new InputHandler();
        _playerPresenter = new PlayerPresenter(_playerView);
        _trackPresenter = new TrackPresenter(trackModel, _trackView, _poolHandler);
        _pauseMenuPresenter = new PauseMenuPresenter();
        _pauseButtonPresenter = new PauseButtonPresenter(_pauseButtonView);
        _crashedMenuPresenter = new CrashedMenuPresenter(_crashedMenuView);
        _countdownPresenter = new CountdownPresenter(_countdownView);

        _serviceManager = new ServiceManager(_playerPresenter, _trackPresenter, _coroutineHandler, _poolHandler);
        _gameStateService = new GameStateService(_playerPresenter, _pauseButtonPresenter, _pauseMenuPresenter, _crashedMenuPresenter, _coroutineHandler, _inputHandler, _poolHandler);

        _playerPresenter.SetServices(_serviceManager, _gameStateService, _inputHandler);
        _trackPresenter.SetServices(_serviceManager, _gameStateService);
        _crashedMenuPresenter.SetServices(_serviceManager, _gameStateService);
        _countdownPresenter.SetServices(_gameStateService);
    

        _inputHandler.Init();
        _playerPresenter.Init();
        _trackPresenter.Init();
        _pauseMenuPresenter.Init(_pauseMenuView);
        _pauseButtonPresenter.Init();
        _crashedMenuPresenter.Init();
        _countdownPresenter.Init();
        _serviceManager.Init();
        _gameStateService.Init();
        _poolHandler.Init(_trackView.AllParts, _serviceManager);

        _coroutineHandler.StartTrackCoroutine();
    }


    private void OnDisable()
    {
        _playerPresenter.Disable();
        _trackPresenter.Disable();
        _pauseMenuPresenter.Disable();
        _pauseButtonPresenter.Disable();
        _crashedMenuPresenter.Disable();
        _countdownPresenter.Disable();
        _serviceManager.Disable();
        _inputHandler.Disable();
    }

    private void OnDestroy()
    {
        _playerPresenter.Dispose();
        _trackPresenter.Dispose();
        _pauseMenuPresenter.Dispose();
        _pauseButtonPresenter.Dispose();
        _crashedMenuPresenter.Dispose();
        _countdownPresenter.Dispose();
        _serviceManager.Dispose();
        _inputHandler.Dispose();
    }

}
