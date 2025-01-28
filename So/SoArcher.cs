using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Entities/Archer", fileName = "Archer")]
public class SoArcher : SoEntity
{
    public int arrowCount;
    private int LittlePrivateIntValue;

    [GridAttribute.Grid(5)]
    [SerializeField]
    public GridAttribute.GridList<AttackStats> m_attackPatern;
}

[Serializable]
public struct AttackStats
{
    public int m_dammage;
    public AttackEffect m_effect;
}

public enum AttackEffect
{
    Fire,
    Ice,
    Thunder,
}