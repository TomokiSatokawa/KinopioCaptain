using UnityEngine;

public interface IDamageble
{
    public int HP { get; }
    public void Damage(int damageAmount = 1);
}
