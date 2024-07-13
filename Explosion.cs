using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    [SerializeField]
    private AudioClip _explosionClip;
    private AudioSource _audioSource;


    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on Explosion is NULL");
        }
        else
        {
            _audioSource.clip = _explosionClip;
        }

        _audioSource.Play();
        Destroy(this.gameObject, 3.0f);
    }


}
