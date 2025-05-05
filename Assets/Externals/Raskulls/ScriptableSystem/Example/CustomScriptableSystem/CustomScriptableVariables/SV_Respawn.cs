using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raskulls.ScriptableSystem
{
    [CreateAssetMenu(fileName = "Respawn_Name", menuName = "Raskulls/Scriptable System/Variables/Respawn")]
    public class SV_Respawn : ScriptableVariableBase
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }
}
