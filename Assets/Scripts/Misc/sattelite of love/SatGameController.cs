using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SatGameController : MonoBehaviour
{
    public SatController Sat;
    public int maxscore = 20;
    public int score = 0;
    public float separation = 1f;
    public GameObject goodObstacle;
    public GameObject badObstacle;
    public UnityEvent<int> onScore;
    public UnityEvent<int> onHit;
    public UnityEvent onWin;
    public UnityEvent onPositive;
    public UnityEvent onNegative;
    float timer;
    bool going = false;
    List<int> history = new List<int>();
    List<GameObject> obstacles = new List<GameObject>();
    
    private float Average(int[] values)
    {
        int sum = 0;
        for (int i = 0; i < values.Length; i++)
        {
            sum += values[i];
            System.Console.Write(values[i] + ", ");
        }
        return((float)sum/values.Length);
    }

    public void CalcHistory(){
        float avg = Average(history.ToArray()); //calc average
        if (avg < 0){ //if it is negative, make her say something awkward. else positive
            onNegative.Invoke();
        }else{
            onPositive.Invoke();
        }
        history.Clear(); //clear the history
    }

    public void SetGoing(bool b){
        going = b;
    }

    // Update is called once per frame
    void Update()
    {
        if (!going){
            return;
        }

        List<Collider2D> colliders = new List<Collider2D>();

        if (Sat.gameObject.GetComponent<Collider2D>().OverlapCollider(new ContactFilter2D(),colliders)>0){
            if ( colliders.Find(x => x.gameObject.tag == "satGood")){
                score += 1;
                history.Add(1);
                onScore.Invoke(score);
            }else if ( colliders.Find(x => x.gameObject.tag == "satBad")){
                score -= 1;
                history.Add(-1);
                onHit.Invoke(score);
            }

            Object.Destroy(colliders.Find(x => x.gameObject.tag == "satGood" || x.gameObject.tag == "satBad").gameObject);

            Mathf.Clamp(score, 0, maxscore);

            if (score == maxscore){
                print("win");
                onWin.Invoke();
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
