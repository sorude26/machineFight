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
}
