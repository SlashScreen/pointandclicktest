using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatObstacle : MonoBehaviour
{
    Transform spawnPoint;
    float speed = .5f;
    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = GameObject.FindGameObjectWithTag("GameSpawnpoint").transform;
        transform.position = spawnPoint.transform.position;
        transform.position = new Vector3 (transform.position.x, transform.position.y + Random.Range(-3f,3f), 0f);
    }

    // Update is called once per frame
    void Update()
    {
        List<Collider2D> colliders = new List<Collider2D>();
         if (GetComponent<Collider2D>().OverlapCollider(new ContactFilter2D(),colliders)>0){
            if ( colliders.Find(x => x.gameObject.tag == "Wall")){
                Object.Destroy(gameObject);
            }
         }

        Vector2 newPos;
        newPos.y = transform.position.y;
        newPos.x = transform.position.x - (speed * Time.deltaTime);
        transform.position = newPos;
    }

    private void OnCollisionEnter2D(Collision2D other){
        Debug.Log("hit wall");
        if (other.gameObject.tag == "Wall"){
            Object.Destroy(gameObject);
        }
    }


    public void SetSpeed(float s){
        speed = s;
    }

}
