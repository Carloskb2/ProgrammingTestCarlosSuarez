using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CircleDragger : MonoBehaviour
{
    Vector3 circlePosition;
    Vector3 mousePosition;
    CustomRigidbody rb;
    SpriteRenderer sprite;
    bool isDragging;

    private void OnEnable()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<CustomRigidbody>();
    }

    private void FixedUpdate()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        circlePosition = transform.position;

        if (rb) rb.IsKinematic = isDragging;

        if (isDragging)
        {
            transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Mathf.Pow(mousePosition.x - circlePosition.x, 2) + Mathf.Pow(mousePosition.y - circlePosition.y, 2) < Mathf.Pow(sprite.bounds.size.y / 2, 2))
            {
                isDragging = true;
            }
        }
        else
            isDragging = false;
    }
}
