using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public float speed = 5f;
    public SpriteRenderer sprite;


    private List<Vector3> path = new List<Vector3>();
    private State goalState;
    private Team myTeam;
    
    
    public void Setup(List<Vector3> path, State goalState, Team myTeam)
    {
        // Spawn animation
        Vector2 initialScale = transform.localScale;
        
        transform.localScale = Vector3.zero;
        transform.DOScale(initialScale, 0.2f);
        
        
        // Setup
        this.path = path;
        this.goalState = goalState;
        this.myTeam = myTeam;

        
        // Color
        sprite.color = myTeam.color;
        
        // Calculate Distance
        float distance = 0;
        Vector3 lastPosition = path[0];
        for (int i = 0; i < path.Count; i++)
        {
            Vector3 position = path[i];
            distance += (position - lastPosition).magnitude;
            Debug.DrawLine(position, lastPosition, Color.white, 3f);
            
            lastPosition = position;

        }
        
        // Set Unit on its way
        float duration = distance / speed;
        transform.DOPath(path.ToArray(), duration).SetEase(Ease.Linear).OnComplete(OnArrival);
    }

    public void OnArrival()
    {
        goalState.Conquer(myTeam);
        transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => Destroy(gameObject));
    }
}
