using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget; //가장 가까운 타겟

    void FixedUpdate()
    {
        // 매개변수 -> 캐스팅 시작 위치, 원의 반지름, 캐스팅 방향, 캐스팅 길이, 대상 레이어
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();

    }

    public List<Transform> GetNearestTargets(int count)
    {
        List<Transform> nearestTargets = new List<Transform>();
        List<RaycastHit2D> sortedTargets = targets.OrderBy(t => Vector3.Distance(transform.position, t.transform.position)).ToList();

        for (int i = 0; i < Math.Min(count, sortedTargets.Count); i++)
        {
            nearestTargets.Add(sortedTargets[i].transform);
        }

        return nearestTargets;
    }


    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach(RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);

            if(curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }

        }

        return result;
    }


}
