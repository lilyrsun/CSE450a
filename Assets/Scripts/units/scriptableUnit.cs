using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Unit", menuName ="Scriptable Unit")]
public class scriptableUnit : ScriptableObject
{
    public Faction Faction;
    public baseUnit Unitprefab;
}

public enum Faction
{
    Player,
    NPC
}
