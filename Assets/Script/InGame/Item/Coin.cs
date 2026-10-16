using InGame.Item;
using Unity.VisualScripting;
using UnityEngine;

public class Coin : MonoBehaviour, ICollectible
{
    [SerializeField] private string _name = "defaultItemName";
    [SerializeField] private float move = 0.5f;
    [SerializeField] private float movespeed = 1f;

    private Vector3 firstPosition;

    public string Name => _name;

    public void PullItem()
    {
            Destroy(this.gameObject);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PullItem();
        }
    }
    void Start()
    {
        firstPosition = transform.position;//基準の座標
    }
    void Update()
    {
        float y = Mathf.Sin(Time.time * movespeed) * move;//オブジェクトのY座標の高さ計算
        transform.position = firstPosition + new Vector3(0,y,0);//計算した物を使ってオブジェクトを動かす
    }
}
