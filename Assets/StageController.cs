using UnityEngine;

public class StageController : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(collision);
        if (collision.transform.position.x > transform.position.x)
        {
            transform.Translate(39.4f, 0, 0);
            Stage.Change(true);
        }
        else
        {
            transform.Translate(-39.4f,0,0);
            Stage.Change(false);
        }
    }
}