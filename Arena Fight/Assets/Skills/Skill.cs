using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Skill")]
public class Skill : ScriptableObject
{
    [Header("Stats")]
    public int Power;
    public float Speed;
}