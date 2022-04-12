using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    [SerializeField] float speed = 15; //Asi es privada y se puede modificar
    [SerializeField] float dir = -1;
    private Animator myAnim;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dir * speed, rb.velocity.y);
    }
}