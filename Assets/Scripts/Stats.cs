using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{

    [SerializeField]
    int Hp;
    [SerializeField]
    int MaxHp;
    [SerializeField]
    int TakeDmg;
    [SerializeField]
    int Dmg;
    // Start is called before the first frame update
    void Start()
    {
        Hp=MaxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
