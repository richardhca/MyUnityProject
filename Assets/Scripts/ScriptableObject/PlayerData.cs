using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "CharacterData/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public string Name;
    public CombatType ATKType;
    public int HP;
    public int STR;
    public float AGI;
    
    [Space(10)]
    public GameObject playerModel;
}

public enum CombatType { Melee, Range };