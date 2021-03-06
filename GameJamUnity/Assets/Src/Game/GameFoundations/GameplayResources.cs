﻿using UnityEngine;

[CreateAssetMenu(menuName ="IDGA/Gameplay Resources")]
public class GameplayResources : ScriptableObject
{
    public GameObject gameplayCamera;
    public PlayerCombatView playerCombatView;
    public EnemyView basicEnemyView;
    public ParentController parentController;
    
}
