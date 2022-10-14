using UnityEngine;

public class OrbV01 : EnemyController
{
    #region Variables
    private bool isSelfDestructing = false;
    private Animator _animator;
    private AudioSource _audioSource;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Suicide bomb the player when close enough
        if ((_player.transform.position - transform.position).sqrMagnitude < 6.25f)
        {
            isSelfDestructing = true;
            StartDying();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.tag == "Player")
            _player.DoDamage(_BaseDamage);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        _mesh.transform.Rotate(Vector3.Cross(Vector3.up, _characterRigidbody.velocity), _characterRigidbody.velocity.magnitude/ .5f * Mathf.Rad2Deg * Time.deltaTime, Space.World);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        isSelfDestructing = false;
    }
    #endregion

    #region Methods Inherited from Parent Class
    public override void StartDying()
    {
        base.StartDying();
        if (isSelfDestructing)
        {
            _audioSource.pitch = Random.Range(.85f, 1.1f);
            _audioSource.Play();
            _animator.SetTrigger("QuickExplode");
        }
        else
            _animator.SetTrigger("Explode");
    }
    #endregion

    #region Internal Functions
    public void Explode()
    {
        _audioSource.pitch = Random.Range(.85f, 1.1f);
        _audioSource.Play();
        if (!isSelfDestructing)
            _player.GiveXP(Mathf.Pow(_player._Level/.8f, 1.5f));
    }
    #endregion
}
