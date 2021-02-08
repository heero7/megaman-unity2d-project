using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AttackData/AttackData")]
public class AttackData : ScriptableObject
{
    public float attackSpeed;
    public float chargeBeginSpeed;
    public float fireRate;
}
