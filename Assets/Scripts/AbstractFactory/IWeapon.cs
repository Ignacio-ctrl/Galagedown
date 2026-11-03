public interface IWeapon
{
    string WeaponName { get; }
    void Attack();

    // para activar o desactivar el GameObject del arma
    void SetWeaponActive(bool isActive);
}