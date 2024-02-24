using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSystem : MonoBehaviour
{
    [SerializeField] private int _seed = 0;

    [SerializeField]
    GameObject plane;

    private void Start()
    {
        // 指定されたシードで内部状態初期

        int i = UnityEngine.Random.Range(3, 8);

        plane.GetComponent<Renderer>().material.SetFloat("_Dim", i);
    }
}
