using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAnimControllers", menuName = "ScriptableObjects/PlayerAnimControllers", order = 1)]
public class PlayerAnimControllers_SO : ScriptableObject
{
    [SerializeField] RuntimeAnimatorController[] Normal;
    [SerializeField] RuntimeAnimatorController[] Weight;
    [SerializeField] RuntimeAnimatorController[] Strength;

    public RuntimeAnimatorController GetNormalControllerAt(int index) => Normal[index];
    public RuntimeAnimatorController GetWeightControllerAt(int index) => Weight[index];
    public RuntimeAnimatorController GetStrengthControllerAt(int index) => Strength[index];
}
