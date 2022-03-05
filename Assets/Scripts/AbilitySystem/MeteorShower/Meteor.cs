using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Meteor : NetworkBehaviour
{
    private float damage;

    public void Init(float damage)
    {
        this.damage = damage;

    }

    private void Start()
    {
        this.transform.Rotate(new Vector3(0f,10f,-90f));
    
    }

    private void Update()
    {
        if (this.transform.position.y<=0f)
        {
            Destroy(gameObject);
            
        }
        else
        {
            // здесь настраиваем скорость падения
            transform.position -= new Vector3(0f, 6 * Time.deltaTime, 0.0f);
        }

    }

    

}
