using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FlyingEnemy : MonoBehaviour
{
    [SerializeField] GameObject player;
    private AIPath myPath;
    [SerializeField] float vida=5;
    private Animator myAnim;


    // Start is called before the first frame update
    void Start()
    {
        myPath = GetComponent<AIPath>();
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(myPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }else if (myPath.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        chasePlayer1();
    }

    void chasePlayer()
    {
        //alternativa 1 vector2.distance
        float d = Vector2.Distance(transform.position, player.transform.position);
        Debug.Log("Distancia con el jugador es" + d);
        if (d < 8)
        {

        }
        Debug.DrawLine(transform.position, player.transform.position, Color.red);
    }

    void chasePlayer1()
    {
        //alternativa 2 overlapCircle
        Collider2D colliding = Physics2D.OverlapCircle(transform.position, 8f, LayerMask.GetMask("Player"));

        if(colliding != null)
        {
            Debug.Log("Colisionando, jugador dentro del radio");
            myPath.isStopped = false;
        }
        else
        {
            myPath.isStopped = true;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 6f);
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
