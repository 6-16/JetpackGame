using System;
using UnityEngine;

public class PlayerPresenter : IDisposable
{
    private ServiceManager _serviceManager;
    private GameStateService _gameStateService;
    private PlayerModel _playerModel;
    private PlayerView _playerView;
    private InputHandler _inputHandler;

    private Rigidbody2D _playerRigidbody;
    // private BoxCollider2D _playerCollider;
    private Vector3 _startingPosition;



    public event Action OnShieldCollected;
    public event Action OnLacksShield;
    public event Action <ObstacleCollisionHandler> OnHasShield;



    public PlayerPresenter(PlayerView view)
    {
        _playerView = view;
    }

    public void Init()
    {
        _playerModel = new PlayerModel();
        LocatePlayerComponents();
        Subscribe();

        GetStartingPosition();
    }

    private void Subscribe()
    {
        _inputHandler.OnJetpackPressedEvent += OnJetpackPressed;
        _gameStateService.OnRestartGame += ResetPlayer;
        _gameStateService.OnPauseGame += FreezePlayer;
        _gameStateService.OnContinueGame += UnFreezePlayer;
        _gameStateService.OnCrashedEvent += FreezePlayer;
        _serviceManager.OnObstacle += CheckForShield;
        _serviceManager.OnCollectible += TryCollectShield;

        // _playerView.OnCollision += FreezePlayer;

    }

    private void UnSubscribe()
    {
        _inputHandler.OnJetpackPressedEvent -= OnJetpackPressed;
        _gameStateService.OnRestartGame -= ResetPlayer;
        _gameStateService.OnPauseGame -= FreezePlayer;
        _gameStateService.OnContinueGame -= UnFreezePlayer;
        _gameStateService.OnCrashedEvent -= FreezePlayer;
        _serviceManager.OnObstacle -= CheckForShield;
        _serviceManager.OnCollectible -= TryCollectShield;
        
        // _playerView.OnCollision -= FreezePlayer;
    }

    public void SetServices(ServiceManager serviceManager, 
    GameStateService gameStateService,
    InputHandler inputHandler)
    {
        SetServiceManager(serviceManager);
        SetGameStateService(gameStateService);
        SetInputHandler(inputHandler);
    }

    #region Set Services
    private void SetServiceManager(ServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    private void SetGameStateService(GameStateService gameStateService)
    {
        _gameStateService = gameStateService;
    }

    private void SetInputHandler(InputHandler inputHandler)
    {
        _inputHandler = inputHandler;
    }

    #endregion

    private void OnJetpackPressed()
    {
        _playerRigidbody.AddForce(Vector2.up * _playerModel.JetpackForce, ForceMode2D.Impulse);
    }

    private void LocatePlayerComponents()
    {
        _playerRigidbody = _playerView.PlayerRigidbody;
        // _playerCollider = _playerView.PlayerCollider;
    }

    private void ResetPlayer()
    {
        _playerRigidbody.transform.position = _startingPosition;

        _playerModel = new PlayerModel();
        _playerView.ArmorMarker.SetActive(false);

        UnFreezePlayer();
    }

    private void FreezePlayer()
    {
        _playerRigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
    }

    private void UnFreezePlayer()
    {
        _playerRigidbody.constraints = RigidbodyConstraints2D.None;
        _playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        _playerRigidbody.WakeUp();
    }

    private void CheckForShield(ObstacleCollisionHandler obstacle)
    {
        if (_playerModel.IsArmored == false)
        {
            LacksShield();
        }
        else
        {
            _playerModel.IsArmored = false;
            _playerView.ArmorMarker.SetActive(false);
            OnHasShield?.Invoke(obstacle);

            // _serviceManager.DeactivateObstacle(obstacle);
        }
    }

    private void LacksShield() => OnLacksShield?.Invoke();

    private void TryCollectShield(CollectibleCollisionHandler collectible)
    {
        Debug.Log(_playerModel.IsArmored);

        if (_playerModel.IsArmored == false)
        {
            _playerModel.IsArmored = true;
            _playerView.ArmorMarker.SetActive(true);
            OnShieldCollected?.Invoke();
            //_serviceManager.OnShield?.Invoke();
            _serviceManager.DeactivateShield(collectible);
        }
        else
        {
            _playerView.DimArmorMarker();
        }
    }

    private void GetStartingPosition()
    {
        _startingPosition = _playerRigidbody.transform.position;
    }

    public void Disable()
    {

    } 

    public void Dispose()
    {
        UnSubscribe();
        _playerModel = null;
        _playerView = null;
        _inputHandler = null;
    }
}
