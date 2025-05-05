using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Raskulls.ScriptableSystem;

public enum DamageType
{
    Fire,
    Bleed,
    Ice,
    Poison
}

public class Jumper : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private SV_Respawn respawn = null;
    [SerializeField] private SV_PlayerDamage playerDamage = null;

    [Header("Events")]
    [SerializeField] private SE_SadEvent sadEvent = null;
    [SerializeField] private SE_PlayerHit playerHit = null;
    [SerializeField] private SE_Vector3 spawnEnemy = null;

    private void Start()
    {
        spawnEnemy.Raise(Vector3.zero, () => Debug.Log("Done!"));
    }
}
