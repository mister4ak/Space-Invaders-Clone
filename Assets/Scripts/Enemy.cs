using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public event Action<Enemy> Died; 
    public void TakeDamage()
    {
        Destroy(gameObject);
        Died?.Invoke(this);
    }

    public void Move(float deltaX, float deltaY)
    {
        transform.position += new Vector3(deltaX, deltaY);
    }
}