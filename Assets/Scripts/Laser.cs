using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Laser : MonoBehaviour
{
    [SerializeField] 
    private float lazer_speed = 8.0f;
    private bool _isParentNotNull;
    private bool _isEnemyLaser = false;
    

    // Start is called before the first frame update
    private void Start()
    {
        _isParentNotNull = null != this.gameObject.transform.parent;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_isEnemyLaser == false)
        {
            MoveUp();
        }
        else if (_isEnemyLaser)
        {
            MoveDown();
        }
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * (Time.deltaTime * lazer_speed));
        if (transform.position.y < -8f)
        {
            if (_isParentNotNull)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    private void MoveUp()
    {
        transform.Translate(Vector3.up * (Time.deltaTime * lazer_speed));
        if (transform.position.y > 7f)
        {
            if (_isParentNotNull)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Player" && _isEnemyLaser == true)
        {
            var player = col.GetComponent<Player>();
            player.Damage();
        }
    }
}
