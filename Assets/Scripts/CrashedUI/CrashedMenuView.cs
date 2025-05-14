using System;
using UnityEngine;
using UnityEngine.UI;

public class CrashedMenuView : MonoBehaviour
{
    [SerializeField] private GameObject _windowHandler;
    [SerializeField] private Button _restartButton;


    public event Action OnRestart;


    private void OnEnable()
    {
        _restartButton.onClick.AddListener(RestartPressed);
    }

    private void OnDisable()
    {
        _restartButton.onClick.RemoveAllListeners();
    }
    public void SetWindowActive(bool isActive)
    {
        _windowHandler.SetActive(isActive);
    } 

    private void RestartPressed()
    {
        OnRestart?.Invoke();
    }
}
