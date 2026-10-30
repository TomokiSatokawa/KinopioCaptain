using DG.Tweening;
using UnityEngine;

namespace InGame.Stage
{
    public class PowBlock : MonoBehaviour, IDamageble
    {
        [SerializeField] private float _detectionRadius;    // 検出範囲の半径
        [SerializeField] private float _damageAmount; //周囲に与えるダメージ量
        [SerializeField] private float _upAmount;
        [SerializeField] private float _animationDuration;
        [SerializeField] private LayerMask _hitBlocksLayer;     // 周りのブロックを示すレイヤーマスク
        private Sequence _upDownAnimation;

        private int _hp = 1;
        public int HP => _hp;

        private void Start()
        {
            _upDownAnimation = DOTween.Sequence().Pause();

            _upDownAnimation.Append(this.transform.DOMoveY(_upAmount, _animationDuration / 2).SetRelative());
            _upDownAnimation.Append(this.transform.DOMoveY(-_upAmount, _animationDuration / 2).SetRelative());
            _upDownAnimation.AppendCallback(() => Destroy(this.gameObject));
        }

        public void Damage(int damageAmount = 1)
        {
            if (HP <= 0) return;

            _hp -= damageAmount;

            if (HP < 1)
            {
                // 指定した範囲内のすべてのBlockコライダーを検出
                Collider[] hitBlocks = Physics.OverlapSphere(this.transform.position, _detectionRadius, _hitBlocksLayer);

                foreach (Collider block in hitBlocks)
                {
                    if (!block.TryGetComponent(out IDamageble damageble)) continue;

                    damageble.Damage(damageAmount);
                }

                _upDownAnimation.Restart();
            }
        }

        //public void OnCollisionEnter(Collision collision)
        //{
        //    if (collision.gameObject.CompareTag("Player"))
        //    {
        //        Damage(1);
        //    }
        //}
    }
}
