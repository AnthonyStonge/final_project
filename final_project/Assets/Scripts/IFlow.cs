using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFlow
{
    bool IsActive { get; set; }
    void PreInitialize();
    void Initialize();
    void Refresh();
    void PhysicRefresh();
    void LateRefresh();
    void End();
}