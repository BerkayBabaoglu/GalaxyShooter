using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    [System.NonSerialized] public bool _isEnemyLaser = false;


    void Update()
    {

        
        _isEnemyLaser ? MoveUpDown(-1.0f) : MoveUp(1.0f);   // does the same thing as the below line but in a simplier way

        // if (_isEnemyLaser == false)
        // {
        //     MoveUp(1.0f);
        // }
        // else
        // {
        //     MoveUpDown(-1.0f);
        // }

    }

    void MoveUpDown(float direction)
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime * direction);

        if (transform.position.y > 8f * direction)
        {
            // check if this object has a parent
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            // destroy the parent too!
            Destroy(this.gameObject);
        }
    }

    // Instead of changing variable via method, you can simply change variable directly
    // for example laser._isEnemyLaser = true;
    // public void AssignEnemyLaser()
    // {
    //     _isEnemyLaser = true;
    // }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //You dont have to check if bool is true, you can simply type it without checking
        /*
        Those are the same:
        _isEnemyLaser
        _isEnemyLaser == true
        */
        if (other.CompareTag("Player") && _isEnemyLaser)
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
        }
    }

}
