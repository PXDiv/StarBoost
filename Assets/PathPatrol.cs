using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PathPatrol : MonoBehaviour
{
    [SerializeField] List<Transform> patrolTransforms;
    [SerializeField] bool toTeleport;
    [SerializeField] bool toLoop = true;
    [SerializeField] float delayTime = 1, reachTime = 2;
    [SerializeField] Ease easeMethod;
    [SerializeField] bool startPatrolOnAwake = true;

    void Awake()
    {
        if (startPatrolOnAwake)
        { StartPatrol(); }
    }

    public void StartPatrol()
    {
        StartCoroutine(PatrolCo());
    }

    IEnumerator PatrolCo()
    {
        patrolTransforms.ForEach(t => t.parent = null);

        while (toLoop)
        {
            foreach (Transform _transform in patrolTransforms)
            {
                yield return new WaitForSeconds(delayTime);
                transform.DOMove(_transform.position, reachTime).SetEase(easeMethod);
                yield return new WaitForSeconds(reachTime);
            }
        }
    }

}
