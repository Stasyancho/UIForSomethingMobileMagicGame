using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Spell")]
public class Spell : ScriptableObject
{
    [SerializeField] int _key = -1;
    [SerializeField] int _manaCost;
    [SerializeField] SpellType _spellType;
    [Header("Damage")]
    [SerializeField] int _fireDamage;
    [SerializeField] int _earthDamage;
    [SerializeField] int _waterDamage;
    [Header("EffectTime")]
    [SerializeField] int _burningTime;
    [SerializeField] int _stunTime;
    [SerializeField] int _slowDownTime;

    public int Key => _key;
    public int ManaCost => _manaCost;
    public SpellType SpellType => _spellType;
    public int FireDamage => _fireDamage;
    public int EarthDamage => _earthDamage;
    public int WaterDamage => _waterDamage;
    public int BurningTime => _burningTime;
    public int StunTime => _stunTime;
    public int SlowDownTime => _slowDownTime;
}

public enum SpellType
{
    Mass,
    Solo,
    Buff
}
