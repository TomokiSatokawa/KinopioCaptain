using UnityEngine;
using InGame.Item;
using Unity.VisualScripting;
using DG.Tweening;

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
            // 指定した範囲内のすべてのBlockコライダーを検出
            Collider[] hitBlocks = Physics.OverlapSphere(detectionCenter, detectionRadius, hitBlocksLayer);
            if (hitBlocks.Length > 1)
            {
                foreach (Collider Block in hitBlocks)
                {
                    Debug.Log("Block");
                    Destroy(Block.gameObject);
                }
            }
            this.transform.DOMoveY(1.5f, 0.1f).OnComplete(() => { this.transform.DOMoveY(1, 0.2f).OnComplete(() => { Destroy(this.gameObject); }); });
            //this.transform.DOMoveY(1, 0.5f).OnComplete(() => { Destroy(this.gameObject); }); 
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
