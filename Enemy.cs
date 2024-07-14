using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private Player _player;

    private Animator _anim;

    [SerializeField]
    private int _pointsForEnemy = 10;
    [SerializeField]
    private GameObject _laserPrefab;

    private AudioSource _audioSource;
    private float _fireRate = 3.0f;
    private float _canFire = -1;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();


        // FindWithTag is more optimizatied way to find the object in the scene
        //_player = GameObject.Find("Player").GetComponent<Player>();
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();


        if (_player == null)
        {
            Debug.LogError("The Player is NULL.");
        }

        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("The animator is NULL.");
        }
    }


    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3.9f, 7.0f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5.4f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7.0f, 0);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_player != null)
            {
                _player.Damage();
            }
            
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 2.8f);
        }
        else if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(_pointsForEnemy);
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();

            // You also can disable the collider instead of destroying it for better optimization
            // Destroy(this.gameObject.GetComponent<Collider2D>());
            GetComponent<Collider2D>().enabled = false;
            Destroy(this.gameObject, 2.8f);
        }

    }
}
