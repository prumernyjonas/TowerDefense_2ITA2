using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{

    [SerializeField]
    public int Hp;
    [SerializeField]
    public int MaxHp;
    [SerializeField]
    public int TakeDmg;
    [SerializeField]
    public int Dmg;
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
