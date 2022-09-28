public interface IWeapon
{
    void Fire();
}

public interface IReloadableWeapon : IWeapon
{
    void Reload();
}
