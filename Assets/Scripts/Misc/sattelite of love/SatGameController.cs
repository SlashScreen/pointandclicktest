using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatGameController : MonoBehaviour
{
    public SatController Sat;
    public int maxscore = 20;
    public int score = 0;
    public GameObject goodObstacle;
    public GameObject badObstacle;
    float timer;
    List<GameObject> obstacles = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        List<Collider2D> colliders = new List<Collider2D>();

        if (Sat.gameObject.GetComponent<Collider2D>().OverlapCollider(new ContactFilter2D(),colliders)>0){
            if ( colliders.Find(x => x.gameObject.tag == "satGood")){
                score += 1;
            }else if ( colliders.Find(x => x.gameObject.tag == "satBad")){
                score -= 1;
            }

            Object.Destroy(colliders.Find(x => x.gameObject.tag == "satGood" || x.gameObject.tag == "satBad").gameObject);

            Mathf.Clamp(score, 0, maxscore);

            if (score == maxscore){
                print("win");
            }
        }

        //add gameObjects

        timer += Time.deltaTime;
        Debug.Log(Mathf.Lerp(0, maxscore,score)/maxscore);
        if (timer >= 2f - (Mathf.Lerp(0, maxscore,score)/maxscore ) ){ //timer interval
            timer = 0;
            if( Mathf.RoundToInt(Random.Range(0f,1f)) == 1 ){
                GameObject ob = Instantiate(goodObstacle);
                obstacles.Add(ob);
                ob.GetComponent<SatObstacle>().SetSpeed(2.5f + (Mathf.Lerp(0, maxscore,score)/maxscore));
            }else{
                GameObject ob = Instantiate(badObstacle);
                obstacles.Add(ob);
                ob.GetComponent<SatObstacle>().SetSpeed(2.5f + (Mathf.Lerp(0, maxscore,score)/maxscore));
            }
        }
    }
}
