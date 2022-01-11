using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Obstacle : MonoBehaviour
{

    private enum ObstacleMoveDirection
    {
        LEFTTORIGHT,
        RIGHTTOLEFT
    }
    [SerializeField] ObstacleMoveDirection obstacleMoveDirection;
    [SerializeField] Transform modelTransform, leftLimitTransform, rightLimitTransform;
    [SerializeField] float moveDuration, rotateDuration;



    private void Start()
    {
        Move();
    }

    private void Move()
    {
        if (obstacleMoveDirection == ObstacleMoveDirection.LEFTTORIGHT)
        {
            modelTransform.DOLocalMoveX(rightLimitTransform.localPosition.x, moveDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
            modelTransform.DOLocalRotate(new Vector3(0, 0, -360f), rotateDuration, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        }
        else if (obstacleMoveDirection == ObstacleMoveDirection.RIGHTTOLEFT)
        {
            modelTransform.DOLocalMoveX(leftLimitTransform.localPosition.x, moveDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
            modelTransform.DOLocalRotate(new Vector3(0, 0, 360f), rotateDuration, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        }
    }
}
