using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject cargoContainer;
    private CargoSlot[] slots;
//    private Vector3 originalPosition;
    public void OnBeginDrag(PointerEventData eventData)
    {
//        throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
 //       throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        RectTransform button = transform.GetComponent<RectTransform>();
        for(int i = 0; i < slots.Length; i++)
        {

            RectTransform slot = slots[i].transform.GetComponent<RectTransform>();
            if(is_rectTransformsOverlap(Camera.main, slot, button)) { 
                Debug.Log("BUTTON CONTAINS " + slots[i].gameObject.name);
                slots[i].Fill();
                break;
            }
        }
        transform.localPosition = Vector2.zero;
 //       throw new System.NotImplementedException();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
 //       throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        slots = cargoContainer.GetComponentsInChildren<CargoSlot>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    public static bool is_rectTransformsOverlap(Camera cam,
                                                     RectTransform elem,
                                                     RectTransform viewport = null)
    {
        Vector2 viewportMinCorner;
        Vector2 viewportMaxCorner;

        if (viewport != null)
        {
            //so that we don't have to traverse the entire parent hierarchy (just to get screen coords relative to screen),
            //ask the camera to do it for us.
            //first get world corners of our rect:
            Vector3[] v_wcorners = new Vector3[4];
            viewport.GetWorldCorners(v_wcorners); //bot left, top left, top right, bot right

            //+ow shove it back into screen space. Now the rect is relative to the bottom left corner of screen:
            viewportMinCorner = cam.WorldToScreenPoint(v_wcorners[0]);
            viewportMaxCorner = cam.WorldToScreenPoint(v_wcorners[2]);
        }
        else
        {
            //just use the scren as the viewport
            viewportMinCorner = new Vector2(0, 0);
            viewportMaxCorner = new Vector2(Screen.width, Screen.height);
        }

        //give 1 pixel border to avoid numeric issues:
        viewportMinCorner += Vector2.one;
        viewportMaxCorner -= Vector2.one;

        //do a similar procedure, to get the "element's" corners relative to screen:
        Vector3[] e_wcorners = new Vector3[4];
        elem.GetWorldCorners(e_wcorners);

        Vector2 elem_minCorner = cam.WorldToScreenPoint(e_wcorners[0]);
        Vector2 elem_maxCorner = cam.WorldToScreenPoint(e_wcorners[2]);

        //perform comparison:
        if (elem_minCorner.x > viewportMaxCorner.x) { return false; }//completelly outside (to the right)
        if (elem_minCorner.y > viewportMaxCorner.y) { return false; }//completelly outside (is above)

        if (elem_maxCorner.x < viewportMinCorner.x) { return false; }//completelly outside (to the left)
        if (elem_maxCorner.y < viewportMinCorner.y) { return false; }//completelly outside (is below)

        /*
             commented out, but use it if need to check if element is completely inside:
            Vector2 minDif = viewportMinCorner - elem_minCorner;
            Vector2 maxDif = viewportMaxCorner - elem_maxCorner;
            if(minDif.x < 0  &&  minDif.y < 0  &&  maxDif.x > 0  &&maxDif.y > 0) { //return "is completely inside" }
        */

        return true;//passed all checks, is inside (at least partially)
    }
}
