using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubesSc : MonoBehaviour {

    public int id;
    public Vector3 target;
    public PlayerSc player;
    public bool move;

    bool follow = true;
    Rigidbody2D rb;
    BoxCollider2D col;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        target = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Block")
            BlockStop();

        if (collision.gameObject.tag == "Coin")
        {
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Grav")
        {
            player.GravityChanger(collision.gameObject);
            Destroy(collision.gameObject);
        }
    }

    public void BlockStop()
    {
        col.isTrigger = false;
        follow = false;
        rb.constraints = RigidbodyConstraints2D.None;
        transform.parent = null;
        player.sil(this);
        Destroy(this.gameObject, 5f);
    }

    private void Update()
    {
        if(follow)
            transform.position = new Vector2(player.transform.position.x, transform.position.y);

        if (transform.position != target && move)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, target.y, transform.position.z), 5f * Time.deltaTime);
        else if (move)
            move = false;
    }
}
