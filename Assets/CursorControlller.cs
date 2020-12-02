using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorControlller : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject upLeft;
    public GameObject upRight;
    public GameObject bottomRight;
    public GameObject bottomLeft;
    public GameObject middlePoint;
    public float width = 1;
    public float height = 1;
    public bool inBounds = false;
    void Start()
    {
        SetSize(1, 1);
    }
    public void SetSize(float w, float h)
    {
        upLeft.transform.position = new Vector2(middlePoint.transform.position.x - (w / 2.0f), middlePoint.transform.position.y  + (h / 2.0f)) ;
        upRight.transform.position = new Vector2(middlePoint.transform.position.x + (w / 2.0f), middlePoint.transform.position.y + ( h / 2.0f));
        bottomRight.transform.position = new Vector2(middlePoint.transform.position.x + (w / 2.0f), middlePoint.transform.position.y - (h / 2.0f));
        bottomLeft.transform.position = new Vector2(middlePoint.transform.position.x  -  (w / 2.0f), middlePoint.transform.position.y - (h / 2.0f));
    }
    // Update is called once per frame
    void Update()
    {
        // SetSize(width, height);

        Vector3 bounds = new Vector3();
        Vector3 middlePosition = new Vector3();
        bool collided = IsColliding(out bounds, out middlePosition);
        if (!inBounds && collided)
        {
            middlePoint.transform.position = middlePosition;
            SetSize(bounds.x, bounds.y);
            inBounds = true;

        }
        if(!collided)
        {
            SetSize(width, height);
            inBounds = false;
        }

     }
    bool IsColliding(out Vector3 bounds, out Vector3 position)
    {
        Collider2D[] colliders = Physics2D.OverlapAreaAll(upLeft.transform.position, bottomRight.transform.position);
        foreach (Collider2D collider in colliders)
        {
            //Debug.Log(collider.tag);
            if (collider.gameObject.CompareTag("MovingPlatform"))
            {
                position = collider.bounds.center;
                bounds = collider.bounds.size;
                return true;
                
            }

        }
        position = new Vector3();
        bounds = new Vector3();
        return false;
    }
    private void OnDrawGizmos()
    {
        
    }
}
