using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/OSkill")]
public class Skill : ScriptableObject
{
    [Header("Stats")]
    public int Power;
    public float Speed;

    [Tooltip("Ignore this value if the skill does not shoot things.")]
    public float ProjectileSpeed;
}