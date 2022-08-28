using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Slider _slider;

    [SerializeField] private float _moveSpeed;

    private bool isValueUp;

    private void Start()
    {
        StartCoroutine(CoHandleRightMove());
    }

    private IEnumerator CoHandleRightMove()
    {
        isValueUp = true;
        while (isValueUp)
        {
            _slider.value += _moveSpeed * Time.deltaTime;
            yield return null;
            if (_slider.value >= 1)
            {
                StartCoroutine(CoHandleLeftMove());
            }
        }
    }
    private IEnumerator CoHandleLeftMove()
    {
        isValueUp = false;
        while (!isValueUp)
        {
            _slider.value -= _moveSpeed * Time.deltaTime;
            yield return null;
            if (_slider.value <= -1)
            {
                StartCoroutine(CoHandleRightMove());
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isValueUp)
        {
            StartCoroutine(CoHandleLeftMove());
        }
        else
        {
            StartCoroutine(CoHandleRightMove());
        }
    }
}
