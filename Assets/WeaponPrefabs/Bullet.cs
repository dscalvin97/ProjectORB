using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour, IWeaponAmmo, IPoolableObject
{
    private float _speed;
    private float _damage;
    private Rigidbody _rig;
    private Camera _cam;
    private GameManager _gameManager;
    private ObjectPool originalPool;

    private void Start()
    {
        _rig = GetComponent<Rigidbody>();
        _cam = Camera.main;
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        Vector3 vpPos = _cam.WorldToViewportPoint(transform.position);
        float boundsMin = -.25f;
        float boundsMax = 1.25f;
        if (vpPos.x < boundsMin || vpPos.y < boundsMin || vpPos.x > boundsMax || vpPos.y > boundsMax)
            SendBackToPool();
    }

    private void FixedUpdate()
    {
        _rig.MovePosition(_rig.position + (transform.TransformDirection(Vector3.forward) * Time.deltaTime * _speed));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.gameObject)
        {
            try
            {
                other.GetComponentInParent<IDamageable>().DoDamage(_damage);
            }
            catch (System.Exception)
            {
                //Debug.Log("Not an IDamageableObject");
            }
            SendBackToPool();
        }
    }

    public void SetAmmoDamage(float damageValue)
    {
        _damage = damageValue;
    }

    public void SetAmmoSpeed(float speedValue)
    {
        _speed = speedValue;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void SendBackToPool()
    {
        originalPool.ReturnObjectToPool(this);
    }

    public void SetOriginatingPool(ObjectPool originalPool)
    {
        this.originalPool = originalPool;
    }
}
