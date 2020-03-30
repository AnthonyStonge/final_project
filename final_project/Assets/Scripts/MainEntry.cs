using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainEntry : MonoSingleton<MainEntry>
{
    private Flow flow;
    
    protected void Awake()
    {
        base.Awake(); //Singleton Pattern
        if (toDestroy) { return; }
        
        flow = Flow.Instance;
        
        flow.PreInitialize();
    }
    
    void Start()
    {
        flow.Initialize();
    }
    
    void Update()
    {
        flow.Refresh();
    }

    private void FixedUpdate()
    {
        flow.PhysicRefresh();
    }

    private void LateUpdate()
    {
        flow.LateRefresh();
    }
}
