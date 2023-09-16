using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public static CameraScript Instance;
    private void Awake()
    {
        Instance = this;
    }
    public IEnumerator Shake(float _duration, float _magnitude)
    {
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            float x = Random.Range(-1f, 1f) * _magnitude;
            float y = Random.Range(-1f, 1f) * _magnitude;

            Camera.main.transform.position += new Vector3(x, y);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        //Camera.main.transform.position = new Vector3(0, 0, -10);
    }
    public void CameraShake(CinemachineImpulseSource _source, float _force)
    {
        _source.GenerateImpulseWithForce(_force);
    }
}
