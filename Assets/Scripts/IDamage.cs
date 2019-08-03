using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DOT
{
    public interface IDamage
    {
        void Damage(int amount, bool instantHit = false);
        void Die();
    }
}
