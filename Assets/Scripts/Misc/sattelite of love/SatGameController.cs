using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatGameController : MonoBehaviour
{
    public SatController Sat;
    public int maxscore = 20;
    public int score = 0;
    public float separation = 1f;
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
        //Debug.Log(Mathf.Lerp(0, maxscore,score)/maxscore);
        if (timer >= 4f - ((Mathf.Lerp(0, maxscore,score)/maxscore )*3f) ){ //timer interval

            timer = 0;

            GameObject[] column = new GameObject[4]; 
            bool didGood = false; //half assed way of making only 1 of each type
            bool didBad = false;
            for (int i = 0; i < 4; i++)
            {
                
                int decision = Random.Range(1,4);
                GameObject ob = null;
                switch (decision){
                    case 1:
                        if(!didGood){
                            ob = goodObstacle;
                            didGood = true;
                        }else{
                            ob = badObstacle;
                        }
                        break;
                    case 2:
                        if(!didBad){
                            ob = badObstacle;
                            didBad = true;
                        }else{
                            ob = null;
                        }
                        break;
                    case 3:
                        ob = null;
                        break;
                }
                column[i] = ob;
            }

            int index = 0;
            foreach (var item in column)
            {
                if (item != null){
                    GameObject add = Instantiate(item);
                    //add.GetComponent<SatObstacle>().SetSpeed(2.5f + (Mathf.Lerp(0, maxscore,score)/maxscore));
                    add.GetComponent<SatObstacle>().SetSpeed(2f);
                    add.GetComponent<SatObstacle>().SetY(index*separation); 
                    //Debug.Log(index + "," + index*.5f);
                }
                index += 1;
            }

        }
    }
}
