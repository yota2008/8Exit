using UnityEngine;

public class BG : MonoBehaviour
{
    public int MyNum;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void BGMove(int N, bool option)
    {
        GameObject[] D = GameObject.FindGameObjectsWithTag("BG");
        foreach (GameObject DBG in D)
        {
            int DN = DBG.GetComponent<BG>().MyNum;
            if (N - 2 >= DN && option)
            {
                DBG.transform.position = new Vector3(9.85f * (DN + 4), 0, 0);
                DBG.GetComponent<BG>().MyNum += 4;
                Debug.Log(DBG.GetComponent<BG>().MyNum);
            }
            if (N + 2 <= DN && !option)
            {
                DBG.transform.position = new Vector3(9.85f * (DN - 4), 0, 0);
                DBG.GetComponent<BG>().MyNum -= 4;
                Debug.Log(DBG.GetComponent<BG>().MyNum);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(collision);
        if (collision.transform.position.x > transform.position.x)
        {
            BGMove(this.MyNum, true);
        }
        else
        {
            BGMove(this.MyNum, false);
        }
    }
}