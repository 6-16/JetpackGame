using TMPro;
using UnityEngine;

public class CountdownView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countdownText;


    






    public void ToggleCountdownText()
    {
        _countdownText.gameObject.SetActive(!_countdownText.gameObject.activeSelf);
    }

    public void SetCountdownText(string inputText)
    {
        _countdownText.text = inputText;
    }
}
