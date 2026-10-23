using UnityEngine;
using InGame.Item;
using Unity.VisualScripting;

public class Powblock : MonoBehaviour,IDamageble
{

    public int HP = 1;
    public Vector3 detectionCenter;  // 検出の中心点（プレイヤーの位置など）
    public float detectionRadius;    // 検出範囲の半径
    public LayerMask hitBlocksLayer;     // 周りのブロックを示すレイヤーマスク

    public void Damage(int damageAmount)
    {
        HP -= 1;
        if (HP < 1)
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 指定した範囲内のすべての敵コライダーを検出
        Collider[] hitBlocks = Physics.OverlapSphere(detectionCenter, detectionRadius, hitBlocksLayer);
        if(hitBlocks.Length > 1)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Damage(1);
        }
    }
}
