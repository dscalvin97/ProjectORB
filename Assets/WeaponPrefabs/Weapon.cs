using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum WeaponType { Projectile, HitScan }

public class Weapon : MonoBehaviour, IWeapon
{
    
    public WeaponType _WeaponType = WeaponType.Projectile;
    public GameObject _Projectile;
    public float _BaseDamage = 10;
    public float _FireRate = .5f;
    public float _AmmoSpeed = 10;

    private GameObject _bulletSpawn;
    private GameManager _gameManager;
    private bool _canFire = true;
    private float _fireTimer = 0;
    private GameControls _gameControls;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _gameControls = _gameManager._GameControls;
    }

    private void OnEnable()
    {
        _bulletSpawn = gameObject.transform.Find("BulletSpawn").gameObject;
        
    }

    void FixedUpdate()
    {
        if (_fireTimer > 1 / _FireRate)
            _canFire = true;

        _fireTimer += Time.deltaTime;
    }

    public void Fire()
    {
        if (_canFire)
        {
            IWeaponAmmo ammo = _gameManager.pools[0].GetObjectFromPool(_bulletSpawn.transform) as IWeaponAmmo;
            ammo.SetAmmoDamage(_BaseDamage);
            ammo.SetAmmoSpeed(_AmmoSpeed);
            _canFire = false;
            _fireTimer = 0;
        }
    }
}
