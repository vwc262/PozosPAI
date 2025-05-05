using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raskulls.ScriptableSystem
{
    [CreateAssetMenu(fileName = "PlayerDamage_Name", menuName = "Raskulls/Scriptable System/Variables/PlayerDamage")]
    public class SV_PlayerDamage : ScriptableVariableBase
    {
        public float DamageAmount;
        public DamageType DamageType;
    }
}
