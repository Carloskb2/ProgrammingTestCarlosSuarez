using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CustomRigidbody))]
[RequireComponent(typeof(SpriteRenderer))]
public class CircleCollider : MonoBehaviour
{
    CustomRigidbody cr;
    SpriteRenderer sprite;

    bool collisionFlagDown;
    bool collisionFlagTop;
    bool collisionFlagRight;
    bool collisionFlagLeft;

    private void OnEnable()
    {
        cr = GetComponent<CustomRigidbody>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        Vector3 screenMax = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector3 screenMin = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));

        if ((transform.position.y + sprite.bounds.size.y / 2f + 0.1f) >= screenMax.y)
        {
            if (!collisionFlagTop)
            {
                collisionFlagTop = true;
                Vector3 impact = new Vector3(transform.position.x, screenMax.y - sprite.bounds.size.y / 2f, transform.position.z);
                cr.AddImpact(Vector3.Reflect(cr.Velocity, Vector3.down), impact);
            }
        }
        else
            collisionFlagTop = false;

        if ((sprite.bounds.center.x + sprite.bounds.size.x / 2f + 0.1f) >= screenMax.x)
        {
            if (!collisionFlagRight)
            {
                collisionFlagRight = true;
                Vector3 impact = new Vector3(screenMax.x - sprite.bounds.size.x / 2f - 0.1f, transform.position.y, 0);
                cr.AddImpact(Vector3.Reflect(cr.Velocity, Vector3.left), impact);
            }
        }
        else
            collisionFlagRight = false;

        if ((sprite.bounds.center.x - sprite.bounds.size.x / 2f - 0.1f) <= screenMin.x)
        {
            if (!collisionFlagLeft)
            {
                collisionFlagLeft = true;
                Vector3 impact = new Vector3(screenMin.x + sprite.bounds.extents.x / 2f, transform.position.y, 0);
                cr.AddImpact(Vector3.Reflect(cr.Velocity, Vector3.right), impact);
            }
        }
        else
            collisionFlagLeft = false;

        if ((sprite.bounds.center.y - sprite.bounds.size.y / 2f - 0.1f ) <= screenMin.y)
        {
            if (!collisionFlagDown)
            {
                collisionFlagDown = true;
                Vector3 impact = new Vector3(transform.position.x, screenMin.y + sprite.bounds.size.y / 2f, 0);
                cr.AddImpact(Vector3.Reflect(new Vector3(cr.Velocity.x, Mathf.Max(cr.Velocity.y, CustomRigidbody.GRAVITY.y) * 2f, 0), Vector3.up), impact);
            }

            cr.HasGravity = false;
        }
        else
        {
            collisionFlagDown = false;
            cr.HasGravity = true;
        }

    }
}
