using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PickupableObject : MonoBehaviour
{
    [SerializeField] GameObject pickupPartical;

    private void Start()
    {
        transform.DOMoveY(0.75f, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        transform.DORotate(new Vector3(0,360,0), 2f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDestroy()
    {
        Destroy(Instantiate(pickupPartical, transform.position, transform.rotation), 1f);
    }
}
