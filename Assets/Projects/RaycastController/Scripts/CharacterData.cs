using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Character/CharacterData", order = 0)]
public class CharacterData : ScriptableObject
{
    public FloatReference currentHealth;
    public FloatReference maxHealth;
    public FloatReference moveSpeed;
    public FloatReference jumpHeight;
}