using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CustomRigidbody : MonoBehaviour
{
    public static Vector3 GRAVITY { get { return new Vector3(0f, -9.8f, 0f); } }

    public Vector3 GravityActive;
    public Vector3 Velocity;

    public bool IsKinematic
    {
        get { return isKinematic; }
        set
        {
            if (value) forces.Clear();
            isKinematic = value;
        }
    }

    [SerializeField] bool isKinematic;
    [SerializeField] List<Vector3> forces = new List<Vector3>();

    public bool HasGravity
    {
        get
        {
            return GravityActive == GRAVITY;
        }
        set
        {
            if (value == true)
                GravityActive = GRAVITY;
            else
            {
                GravityActive = Vector3.zero;
            }
        }
    }

    private Vector3 lastPosition;
    private SpriteRenderer sprite;

    private void OnEnable()
    {
        GravityActive = GRAVITY;
        lastPosition = transform.position;
        sprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (IsKinematic)
        {
            Velocity = (transform.position - lastPosition) / Time.fixedDeltaTime;
            CheckBounds();
            lastPosition = transform.position;
            return;
        }

        Velocity = Velocity + GravityActive * Time.fixedDeltaTime;

        for (int i = 0; i < forces.Count; i++)
        {
            if(forces[i].sqrMagnitude <1f)
            {
                forces.RemoveAt(i);
                continue;
            }

            Vector3 appliedForce = forces[i] * Time.fixedDeltaTime;
            Velocity += appliedForce;
            forces[i] -= appliedForce;
        }

        Vector3 v = Velocity;
        v.x = Mathf.MoveTowards(v.x, 0, Time.fixedDeltaTime);
        Velocity = v;

        Vector3 gravityForce = GravityActive * Time.fixedDeltaTime * Time.fixedDeltaTime / 2f;
        transform.position = lastPosition + Velocity * Time.fixedDeltaTime + gravityForce;

        CheckBounds();

        lastPosition = transform.position;
    }

    void CheckBounds()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        Vector3 screenMax = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        Vector3 screenMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0,0));

        Vector3 viewMin = Camera.main.WorldToViewportPoint(new Vector3(screenMin.x + sprite.bounds.size.x /2f, screenMin.y + sprite.bounds.size.y / 2f, 0));
        Vector3 viewMax = Camera.main.WorldToViewportPoint(new Vector3(screenMax.x - sprite.bounds.size.x /2f, screenMax.y - sprite.bounds.size.y / 2f, 0));

        if (viewPos.x > viewMin.x && viewPos.y > viewMin.y && viewPos.x < viewMax.x && viewPos.y < viewMax.y) return;

        viewPos.y = Mathf.Clamp(viewPos.y, viewMin.y, viewMax.y);
        viewPos.x = Mathf.Clamp(viewPos.x, viewMin.x, viewMax.x);
        transform.position = Camera.main.ViewportToWorldPoint(viewPos);
    }

    public void AddForce(Vector2 force)
    {
        if (force.sqrMagnitude <= 0) return;
        forces.Add(new Vector3(force.x, force.y, 0));
    }

    public void AddImpact(Vector3 force, Vector3 impactPosition)
    {
        Velocity = Vector3.zero;
        forces.Clear();
        forces.Add(force);
    }
}
