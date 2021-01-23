using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Base")]
public class Skill_Base : ScriptableObject
{
    [Header("Stats")]
    public int Power;
    public float Speed;
}