using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Door : MonoBehaviour
{
    [SerializeField] BetArea betArea;
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] float scaleDuration = 0.25f;
    [SerializeField] int stakePercentage;
    [SerializeField] Image image;
    [SerializeField] Color color;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManagement.Instance.StartBet(stakePercentage);

            image.DOColor(color, 0.25f).OnComplete(() => betArea.OneDoorChoseen());
        }
    }

    public void OneDoorTriggered()
    {
        boxCollider.enabled = false;
        transform.DOScale(Vector3.zero, scaleDuration).OnComplete(() => Destroy(gameObject));
    }

}
