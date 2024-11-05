using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Spell")]
public class Spell : ScriptableObject
{
    [SerializeField] private int _key = -1;
    [SerializeField] private int _manaCost;
    [SerializeField] private SpellType _spellType;
    [Header("Damage")]
    [SerializeField] private int _fireDamage;
    [SerializeField] private int _earthDamage;
    [SerializeField] private int _waterDamage;
    [Header("EffectTime")]
    [SerializeField] private int _burningTime;
    [SerializeField] private int _stunTime;
    [SerializeField] private int _slowDownTime;

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
