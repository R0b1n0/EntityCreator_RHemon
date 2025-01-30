using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Entities/Archer", fileName = "Archer")]
public class SoArcher : SoEntity
{
    public int arrowCount;
    private int LittlePrivateIntValue;

    [Robino.GridAttribute.Grid(7)]
    [SerializeField]
    public Robino.GridAttribute.GridList<AttackStats> m_attackPatern;
}

[Serializable]
public struct AttackStats
{
    public int m_dammage;
    public AttackEffect m_effect;
    private int testValue;
}

public enum AttackEffect
{
    Fire,
    Ice,
    Thunder,
}