using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private bool isFiring = false;
    private Animator myAnim;
    private float proximoDisparo = 0f;
    [SerializeField] float cadenciaDisparo = 1;
    [SerializeField] GameObject balaTorreta;
    [SerializeField] float vida = 5;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D ray = Physics2D.Raycast( transform.position, Vector2.left, 15f, LayerMask.GetMask("Player"));
        Debug.DrawRay(transform.position, Vector2.left * 15f, Color.red);
        isFiring = (ray.collider != null);
        Fire();
    }
    void Fire()
    {
        if (isFiring)
        {
            myAnim.SetBool("fire", true);
            if (Time.time > proximoDisparo)
            {
                proximoDisparo = Time.time + cadenciaDisparo;
                Instantiate(balaTorreta, transform.position, transform.rotation);
            }
        }
        else
        {
            myAnim.SetBool("fire", false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("BulletD(Clone)") || collision.gameObject.name.Equals("BulletI(Clone)"))
        {
            vida = vida - 1;
            Debug.Log(collision.gameObject.name);
            Debug.Log("vida actual" + this.gameObject.name + "=" + vida);
        }
        if (vida == 0)
        {
            StartCoroutine("destroy");
        }
    }
    IEnumerator destroy()
    {
        gameObject.layer = 6;
        myAnim.SetBool("isDead", true);
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
