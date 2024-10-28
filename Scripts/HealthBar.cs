using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Transform target;
    Vector2 move;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Hero").GetComponent<Transform>();
        //target2 = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
       move = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y);
    }
}
