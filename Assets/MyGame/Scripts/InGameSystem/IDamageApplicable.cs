/// <summary>
/// ダメージ計算を行うインターフェース
/// </summary>
public interface IDamageApplicable
{
    /// <summary>
    /// ダメージを与える
    /// </summary>
    /// <param name="damage"></param>
    void AddlyDamage(int damage);

    void AddlyDamage(int damage, DamageType damageType);
}

public enum DamageType
{
    None,
    Shot,
    Melee,
    Explosion,
    Beam,
}
[System.Serializable]
public struct DamageRate
{
    private const float DEFAULT_RATE = 1;
    public DamageType Type;
    public float Rate;
    public float ChangeRate => DEFAULT_RATE - Rate;
}