using UnityEngine;

public interface IDamageable
{
    int HP { get; set; }

    void TakeDamage(int damageAmount);
}

