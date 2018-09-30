using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFlowState  {

    void Enter();
    void OnUpdate();
    void Exit();
}
