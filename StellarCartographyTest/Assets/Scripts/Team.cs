using UnityEngine;

[CreateAssetMenu(fileName = "Team", menuName = "ScriptableObjects/Team", order = 1)]
public class Team :  ScriptableObject
{
    public Color color;
    public bool canProduceUnits = false;
}