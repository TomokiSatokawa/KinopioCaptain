using UnityEngine;

namespace InGame.Stage
{
    public class BrickBlock : MonoBehaviour, IDamageble
    {
        public int HP => 0;

        public void Damage(int damageAmount = 1)
        {
            Destroy(this.gameObject);
        }
    }
}
