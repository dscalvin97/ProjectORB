using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyController : GameCharacterController, IPoolableObject
{
    #region Variables
    protected PlayerController _player;
    private ObjectPool originalPool;

    protected GameObject _mesh;
    #endregion

    #region Unity Methods
    protected override void Start()
    {
        base.Start();
        _player = FindObjectOfType<PlayerController>();
        _mesh = transform.Find("Mesh").gameObject;
    }

    protected virtual void FixedUpdate()
    {
        Movement(transform.forward);
        Rotation((_player.transform.position - transform.position).normalized);
    }
    #endregion

    #region Methods Inherited from Parent Class
    public static void CreateEnemy(GameManager gm, Vector3 position, Vector3 direction, float maxHealth, float characterSpeed, float baseDamage)
    {
        EnemyController newEnemy = (EnemyController)gm.pools[1].GetObjectFromPool(position, Quaternion.LookRotation(direction, Vector3.up));

        newEnemy._MaxHealth = maxHealth;
        newEnemy._CharacterSpeed = characterSpeed;
        newEnemy._BaseDamage = baseDamage;
    }

    protected override void Die()
    {
        base.Die();
        SendBackToPool();
    }
    #endregion

    #region Interface Implementations
    public GameObject GetGameObject()
    {
        if (gameObject)
            return gameObject;
        else
            return null;
    }

    public void SendBackToPool()
    {
        if (originalPool != null)
            originalPool.ReturnObjectToPool(this);
        else
            Destroy(gameObject);
    }

    public void SetOriginatingPool(ObjectPool originalPool)
    {
        this.originalPool = originalPool;
    }
    #endregion
}
