using System.Threading.Tasks;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _playerRigidbody;
    [SerializeField] private BoxCollider2D _playerCollider;
    [SerializeField] private GameObject _armorMarker;

    public Rigidbody2D PlayerRigidbody => _playerRigidbody;
    public BoxCollider2D PlayerCollider => _playerCollider;
    public GameObject ArmorMarker => _armorMarker;

    public async void DimArmorMarker()
    {
        SpriteRenderer armorMarkerColor = _armorMarker.GetComponent<SpriteRenderer>();
        Color defaultColor = armorMarkerColor.color;
        armorMarkerColor.color = Color.red;

        await Task.Delay(200);

        armorMarkerColor.color = defaultColor;
    }
}
