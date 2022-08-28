using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.gameObject.GetComponent<EnemyController>();
        EnemyPullManager.instance.DestroyCube(enemy);
        Debug.Log("풀로 돌아갔소.");
    }
}
