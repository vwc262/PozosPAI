using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Raskulls.ScriptableSystem
{
    [CreateAssetMenu(fileName = "Enemies_Name", menuName = "Raskulls/Scriptable System/Variables/Enemies")]
    public class SV_Enemies : ScriptableVariableBase
    {
        public List<GameObject> Value;
    }
}
