using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 15; //Asi es privada y se puede modificar
    [SerializeField] float dir = 1;
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
        if (collision.gameObject.layer == 3)
        {
            Debug.Log(collision.gameObject.layer + "me destruyo contra el ground");
            StartCoroutine("destroy");
        }
        if (collision.gameObject.layer == 9)
        {
            Debug.Log(collision.gameObject.layer + "me destruyo contra el enemigo");
            StartCoroutine("destroy");
        }
        if (collision.gameObject.layer == 10)
        {
            Debug.Log(collision.gameObject.layer + "me destruyo contra el enemigo");
            StartCoroutine("destroy");
        }
        if (collision.gameObject.layer == 11)
        {
            Debug.Log(collision.gameObject.layer + "me destruyo contra el enemigo");
            StartCoroutine("destroy");
        }
    }
    IEnumerator destroy()
    {
        myAnim.SetBool("isDestroy", true);
        rb.constraints = (RigidbodyConstraints2D)RigidbodyConstraints.FreezePosition;
        yield return new WaitForSeconds(0.2f);
        Destroy(this.gameObject);
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dir * speed, rb.velocity.y);
    }
}
