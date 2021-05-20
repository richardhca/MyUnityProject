using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "NewMonsterData", menuName = "CharacterData/MonsterData", order = 2)]
public class MonsterData : ScriptableObject
{
    public string Name;
    public CombatType ATKType;
    public int HP;
    public int STR;
    public float AGI;
    public float ATKRANGE;

    [Space(10)]
    public GameObject monsterModel;
}