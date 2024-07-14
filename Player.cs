using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 6f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private float _speedThrustersAdd = 3f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private Vector3 _laserOffset = new Vector3(0, 1.05f, 0);
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    private float _shotsToFire = 15;
    private bool _outOfAmmo = false;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;

    [SerializeField]
    private GameObject shieldVisualizer;
    [SerializeField]
    private int _shieldStrength = 0;

    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip _outOfAmmoAudioClip;


    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        
        
        _spawnManager = GameObject.FindbjectOfType(SpawnManager);
        _uiManager = GameObject.FindbjectOfType(UIManager);
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("The SpawnManager is NULL");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on the Player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

    }


    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

        Thrusters();


    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // new Vector3(1, 0, 0) is the same as Vector3.right => One unit (meter) on the X and zero units on the Y and Z
        //transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);    // Time.deltaTime converts from one meter per frame (60 meter per second) to one meter per second => Time.deltaTime can be seen as one second
        //transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);


        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        
        checkAmmo();

        if (_outOfAmmo == false)
        {
            _canFire = Time.time + _fireRate;

            if ((_isTripleShotActive == true) && (_shotsToFire > 2))
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
                _shotsToFire -= 3;
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity);
                _shotsToFire -= 1;
            }

            
            if(_audioSource != null)
            {
                _audioSource.Play();
            }
            
            Debug.Log(_shotsToFire);
        }
        else
        {
            /*
            Debug.Log("Out of Ammo");
            AudioSource.PlayClipAtPoint(_outOfAmmoAudioClip, transform.position);
            */
        }


    }
    
    public void checkAmmo()
    {
        if (_shotsToFire < 1)
        {
            _outOfAmmo = true;
        }
    }

    
    public void Damage()
    {
        if (_isShieldActive)
        {
            if (_shieldStrength > 1)
            {
                _shieldStrength--;
                //_uiManager.UpdateShieldStrength(_shieldStrength);
                //return;
            }
            else
            {
                _shieldStrength--;
                _isShieldActive = false;
                shieldVisualizer.gameObject.SetActive(false);
                //_uiManager.UpdateShieldStrength(_shieldStrength);
                //return;
            }
            return;
        }


        _lives--;

        // Alternative usage
        // switch (_lives)
        // {
        //     case 2:
        //         _rightEngine.gameObject.SetActive(true);
        //         break;
        //     case 1:
        //         _leftEngine.gameObject.SetActive(true);
        //         break;
        // }

        if (_lives == 2)
        {
            _rightEngine.gameObject.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngine.gameObject.SetActive(true);
        }


        _uiManager.UpdateLives(_lives);


        if (_lives < 1)
        {
            //Communicate with the SpawnManager
            //Let them know to stop spawning
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }

    }


    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }


    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }


    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }


    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }


    public void ShieldActive()
    {
        _isShieldActive = true;
        shieldVisualizer.gameObject.SetActive(true);
        _shieldStrength = 3;
        //_uiManager.UpdateShieldStrength(_shieldStrength);
    }


    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void Thrusters()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _speed = _speed + _speedThrustersAdd;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed = _speed - _speedThrustersAdd;
        }
    }

    
    public void AmmoReload()
    {
        _shotsToFire = 15;
        _outOfAmmo = false;
        // Debug.Log(_outOfAmmo);
    }
    
    
    public void HealthReload()
    {
        if (_lives < 3)
        {
            _lives += 1;

            if (_lives == 3)
            {
                _rightEngine.gameObject.SetActive(false);
            }
            else if (_lives == 2)
            {
                _leftEngine.gameObject.SetActive(false);
            }

            _uiManager.UpdateLives(_lives);
        }
    }

}
