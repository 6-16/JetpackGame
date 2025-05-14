using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseButtonView : MonoBehaviour
{
    [SerializeField] private Button _pauseButton;


    public event Action OnPause;



    private void OnEnable()
    {
        _pauseButton.onClick.AddListener(PausePressed);
    }

    private void OnDisable()
    {
        _pauseButton.onClick.RemoveAllListeners();
    }

    private void PausePressed()
    {
        OnPause?.Invoke();
    }
}
