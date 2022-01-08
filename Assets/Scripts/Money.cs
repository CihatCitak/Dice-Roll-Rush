using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    void Start()
    {
        transform.DOMoveY(0.75f, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        transform.DORotate(new Vector3(0, 360, 0), 2f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            transform.DOMove(transform.position + Vector3.up * 6.5f, .3f).SetEase(Ease.Linear);
            transform.DOScale(Vector3.one * 5f, .3f).OnComplete(() => Destroy(gameObject));
        }
    }
}
