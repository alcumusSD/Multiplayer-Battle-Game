using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    //GameObject enemy;
    Animator ani;
    Animator aniHero;
    public Transform target;
    public float speed;
    Rigidbody2D rb;
    Vector2 move;
    public Image border;
    public Image red;
    public Image healthBar;
    public float healthAmount = 100f;
    public Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        aniHero = GameObject.FindGameObjectWithTag("Hero").GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Hero").GetComponent<Transform>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == ("Sword") && aniHero.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            Debug.Log("hello");
            //Destroy(gameObject);
            //Crumble(transform);
            ani.SetTrigger("hit");
            TakeDamage(3);
            
        }

       
    }

    void Crumble(Transform t)
    {
        Transform parent = transform.parent;

        foreach (Transform child in t.GetComponentInChildren<Transform>())
        {
            Crumble(child);

        }

        //rb.AddTorque(Random.Range(0, 30), Random.Range(0, 30), Random.Range(0, 30));
        Debug.Log("Print Crumbling");
       
        Rigidbody2D body = t.AddComponent<Rigidbody2D>();
        body.gravityScale = 9;
        float directionX = Random.Range(-5, 50);
        float directionY = Random.Range(5, 50);
        float torque = Random.Range(5, 8);
        body.AddForce(new Vector2(directionX,directionY), ForceMode2D.Impulse);
        Debug.Log("Bro");
        Destroy(t.gameObject, 1);
        ani.enabled = false;

        // body.AddTorque(torque, ForceMode2D.Force);

    }

    // Update is called once per frame
    void Update()
    {
       
        if(healthAmount <= 0)
        {
            Destroy(healthBar);
            Destroy(border);
            Destroy(red);
            //Destroy(canvas)
            Crumble(transform);
        }
        // transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        move = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y);
        if (move.x != 0 || move.y != 0)
        {
            ani.SetBool("walk", true);
            if (target.transform.position.x - transform.position.x <= 3 && target.transform.position.y - transform.position.y <= 3)
            {
                ani.SetBool("attack", true);
            }

            else
            {
                ani.SetBool("attack", false);
            }
        }
        else
        {
            ani.SetBool("walk", false);
        }
       
        
        if (move.x > 0)
        {
            transform.localScale = new Vector3(-3, 3, 3);
            healthBar.transform.localScale = new Vector3(1,1,1);

        }


        else if (move.x < 0)
        {
            transform.localScale = new Vector3(3, 3, 3);
            healthBar.transform.localScale = new Vector3(-1,1,1);

        }
    }
	private void FixedUpdate()
	{
        rb.velocity = new Vector2(move.x, move.y);
    }

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / 100f;

    }

}
