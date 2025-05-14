using System;
using System.Threading.Tasks;
using UnityEngine;

public class InputHandler
{
    private bool _handleInput = false;
    private bool _inputHold = false;

    public event Action OnPressEvent;
    public event Action OnReleaseEvent;
    public event Action OnJetpackPressedEvent;

    public void Init()
    {
        StartInputHandle();
    }

    public async void StartInputHandle()
    {
        _handleInput = true;
        await InputHandle();
    }

    public void StopInputHandle()
    {
        _handleInput = false;
        OnRelease();
    }

    private async Task InputHandle()
    {
        while (_handleInput)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnPress();
            }

            if (Input.GetMouseButtonUp(0))
            {
                OnRelease();
            }

            await Task.Yield();
        }
    }

    private async void OnPress()
    {
        if (_inputHold)
        {
            return;
        }

        _inputHold = true;

        OnPressEvent?.Invoke();
        await OnHoldPress();
    }

    private void OnRelease()
    {
        _inputHold = false;
        OnReleaseEvent?.Invoke();
    }

    private async Task OnHoldPress()
    {
        while (_inputHold)
        {
            OnJetpackPressedEvent?.Invoke();
            await Task.Yield();
        }
    }

    public void Disable()
    {
        StopInputHandle();
        OnRelease();
    }

    public void Dispose()
    {
        StopInputHandle();
        OnRelease();
        OnPressEvent = null;
        OnReleaseEvent = null;
        OnJetpackPressedEvent = null;
    }
}

