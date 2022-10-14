using UnityEngine;

public enum WeaponType { Projectile, HitScan }

public class Weapon : MonoBehaviour, IWeapon
{
    
    public WeaponType _WeaponType = WeaponType.Projectile;
    public GameObject _Projectile;
    public float _FireRate = .5f;

    private GameObject _bulletSpawn;
    private GameManager _gameManager;
    private bool _canFire = true;
    private float _fireTimer = 0;
    private AudioSource _firingSound;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _firingSound = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _bulletSpawn = gameObject.transform.Find("BulletSpawn").gameObject;
    }

    void Update()
    {
        if (_fireTimer > 1 / _FireRate)
            _canFire = true;

        _fireTimer += Time.deltaTime;
    }

    public void Fire(float baseDamage, float fireRate, float ammoSpeed)
    {
        _FireRate = fireRate;
        if (_canFire)
        {
            IWeaponAmmo ammo = _gameManager.pools[0].GetObjectFromPool(_bulletSpawn.transform) as IWeaponAmmo;
            ammo.SetAmmoDamage(baseDamage);
            ammo.SetAmmoSpeed(ammoSpeed);

            if (_firingSound.isPlaying)
                _firingSound.Stop();
            _firingSound.pitch = Random.Range(.9f, 1.05f);
            _firingSound.Play();

            _canFire = false;
            _fireTimer = 0;
        }
    }
}
