using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField]
    Transform otherBackground;
    static int bgChangedNum = 0;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform.parent.TryGetComponent(out PlayerMovement player))
        {
            otherBackground.transform.position = new Vector3(0, transform.position.y + 27.26f, 0); //height of 2 backgrounds
            bgChangedNum++;
            if(bgChangedNum>=200)
            {
                EventManager.PositionResetEvent?.Invoke();
                bgChangedNum = 0;
                OnPositionReset();
            }
         
        }
    }

    void  OnPositionReset()
    {
            transform.position = Vector3.zero;
            otherBackground.transform.position = new Vector3(0, transform.position.y + 27.26f, 0);
    }
}


