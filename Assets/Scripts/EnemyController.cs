using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public RawImage _img;
    [HideInInspector] public Vector2 _dir, _tarPos;
    [HideInInspector] public bool isDead = false;
    [HideInInspector] public bool isCount;
    [HideInInspector] public float _speed = 10f;
    [HideInInspector] public float _minValue, _maxValue;


    public void StartToTarget(float min, float max)
    {
        StartCoroutine(CoGotoTarget(min, max));
    }
    public void StopToTarget()
    {
        StopAllCoroutines();
    }

    private IEnumerator CoGotoTarget(float min, float max)
    {
        _dir = _tarPos - (Vector2)transform.position;
        while (!isDead)
        {
            transform.localPosition += (Vector3)_dir * Random.Range(min, max) * Time.deltaTime;
            if (_dir.x > 0)
            {
                transform.Rotate(0f, 0f, -400f * Time.deltaTime);
            }
            else
            {
                transform.Rotate(0f, 0f, 400f * Time.deltaTime);
            }
            yield return null;
        }
    }
}
