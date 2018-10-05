using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragScript : MonoBehaviour
{

    bool bDragging = false;

        void OnMouseDrag()
        {
        
        }

    void OnMouseExit()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("coucou");
    }


    public void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 toMouse = (mousePos - new Vector2(transform.position.x, transform.position.y));

        if (Input.GetButtonDown("Fire1"))
            if (toMouse.sqrMagnitude < 3)
                bDragging = true;

            if (Input.GetButton("Fire1") && bDragging)
        {


            if (toMouse.sqrMagnitude > 1)
                toMouse = toMouse.normalized;

            //transform.position = mousePos;
            GetComponent<Rigidbody2D>().velocity = toMouse * 20;
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            bDragging = false;
        }
           



        Debug.DrawLine(new Vector3(mousePos.x, mousePos.y, 0), transform.position);
    }

}
