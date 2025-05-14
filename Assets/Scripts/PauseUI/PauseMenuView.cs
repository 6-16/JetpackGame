using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuView : MonoBehaviour
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private GameObject _windowHandler;


    public event Action OnContinue;
    public event Action OnRestart;


    private void OnEnable()
    {
        _continueButton.onClick.AddListener(ContinuePressed);
        _restartButton.onClick.AddListener(RestartPressed);
    }

    private void OnDisable()
    {
        _continueButton.onClick.RemoveAllListeners();
        _restartButton.onClick.RemoveAllListeners();
    }

    public void SetWindowActive(bool isActive)
    {
        _windowHandler.SetActive(isActive);
    } 

    private void ContinuePressed()
    {
        OnContinue?.Invoke();
    }

    private void RestartPressed()
    {
        OnRestart?.Invoke();
    }
}
