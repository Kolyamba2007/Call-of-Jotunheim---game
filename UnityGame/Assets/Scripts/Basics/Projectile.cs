using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public IEnumerator Shoot(Vector3 _casterPosition, Vector3 _targetPosition)
    {
        Debug.Log(name);
        float startTime = Time.time;
        while (transform.position != _targetPosition)
        {
            float elapsedTime = Time.time - startTime;
            Vector3 endPosition = Vector3.Lerp(_casterPosition, _targetPosition, elapsedTime);
            transform.position = endPosition;
            yield return null;
        }
        Destroy(gameObject);
        StopCoroutine(Shoot(_casterPosition, _targetPosition));
    }

    public IEnumerator Shoot(Vector3 _casterPosition, Unit _target)
    {
        float startTime = Time.time;
        while (transform.position != _target.transform.position)
        {
            float elapsedTime = Time.time - startTime;
            Vector3 endPosition = Vector3.Lerp(_casterPosition, _target.transform.position, elapsedTime);
            transform.position = endPosition;
            yield return null;
        }
        Destroy(gameObject);
        StopCoroutine(Shoot(_casterPosition, _target));
    }
}
