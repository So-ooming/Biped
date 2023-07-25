using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularArrangement : MonoBehaviour
{
    [SerializeField] GameObject cube;
    public void Spawn_Pyosik(Transform pivotLeg)
    {
        int num = 12;
        for (int i = 0; i < num; i++)
        {
            GameObject obj = Instantiate(cube, pivotLeg.position + new Vector3(0, 0.35f, 0),
                Quaternion.Euler(0, (360 * i / num) + 180, 0));
            Vector3 dir = new Vector3(Mathf.Cos((Mathf.PI) * 2 * i / num), 0, Mathf.Sin((Mathf.PI) * i * 2 / num));
            obj.transform.position += dir * 0.7f;
        }
    }
}
