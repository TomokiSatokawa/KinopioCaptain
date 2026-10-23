using InGame.Item;
using UnityEngine;

public class Coin : MonoBehaviour, ICollectible
{
    [SerializeField] private string _name = "defaultItemName";
    [SerializeField] private float _move = 0.5f;
    [SerializeField] private float _moveSpeed = 1f;

    private Vector3 _firstPosition;

    public string Name => _name;

    public void OnPlayerTouch()
    {
        Destroy(this.gameObject);
    }

    private void Start()
    {
        _firstPosition = transform.position;//基準の座標
    }

    private void Update()
    {
        float y = Mathf.Sin(Time.time * _moveSpeed) * _move;//オブジェクトのY座標の高さ計算
        transform.position = _firstPosition + new Vector3(0, y, 0);//計算した物を使ってオブジェクトを動かす
    }
}
