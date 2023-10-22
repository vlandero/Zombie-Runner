using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    private int amount;

    public void SetScore(int score)
    {
        amount = score;
    }

    public void InitializeStart()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("PlayerCapsule"))
        {
            GameManager.instance.SetScore(GameManager.instance.GetScore() + amount);
            GameManager.instance.SetIsScoreOnMap(false);
            ObjectPooler.instance.DestroyObject(gameObject);
        }
        if (collision.gameObject.layer == 10)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
        }
    }
}
