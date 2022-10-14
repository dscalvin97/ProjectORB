public interface IWeapon
{
    void Fire(float baseDamage, float fireRate, float ammoSpeed);
}

public interface IReloadableWeapon : IWeapon
{
    void Reload();
}
