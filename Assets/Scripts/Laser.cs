using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Laser : MonoBehaviour
{
    [SerializeField] 
    private float lazer_speed = 8.0f;

    private bool _isParentNotNull;


    // Start is called before the first frame update
    private void Start()
    {
        _isParentNotNull = null != this.gameObject.transform.parent;
    }

    // Update is called once per frame
    private void Update()
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
}
