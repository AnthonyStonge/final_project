using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Flow : IFlow
{
    
    #region Singleton
    private static Flow instance = null;

    private Flow()
    {
    }

    public static Flow Instance
    {
        get => instance ?? (Instance = new Flow());
        private set => instance = value; 
    }
    #endregion

    public bool IsActive { get; set; }

    public void PreInitialize()
    {
        
    }

    public void Initialize()
    {
        
    }

    public void Refresh()
    {
        
    }

    public void PhysicRefresh()
    {
        
    }

    public void LateRefresh()
    {
       
    }

    public void End()
    {
        
    }
}
