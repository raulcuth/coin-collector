using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolEnemyController : MonoBehaviour, IComparable<EvolEnemyController>
{
    public static int counter = 0;
    [HideInInspector] public EvolEnemy template;
    public float time;
    protected Vector2 bounds;
    protected SpriteRenderer _renderer;
    protected BoxCollider2D _collider;

    public void Init(EvolEnemy template, Vector2 bounds)
    {
        this.template = template;
        this.bounds = bounds;
        Revive();
    }

    private void Update()
    {
        if (template == null)
        {
            return;
        }

        time += Time.deltaTime;
    }

    /// <summary>
    /// Reactivate the object
    /// </summary>
    public void Revive()
    {
        GameObject obj;
        (obj = gameObject).SetActive(true);
        counter++;
        obj.name = "EvolEnemy" + counter;

        Vector3 newPosition = UnityEngine.Random.insideUnitCircle;
        newPosition *= bounds;
        _renderer.sprite = template.sprite;
        _collider = obj.AddComponent<BoxCollider2D>();
    }

    /// <summary>
    /// Function for defeating the enemy with a click
    /// </summary>
    private void OnMouseDown()
    {
        Destroy(_collider);
        gameObject.SendMessageUpwards("KillEnemy", this);
    }

    public int CompareTo(EvolEnemyController other)
    {
        return other.time > time ? 0 : 1;
    }
}