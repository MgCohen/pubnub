using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputService : MonoBehaviour
{
    private List<Func<RaycastResult, bool>> filters = new List<Func<RaycastResult, bool>>();
    public void FilterRaycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
    {
        resultAppendList.RemoveAll(hit => FilterRaycastHit(hit));
    }

    private bool FilterRaycastHit(RaycastResult hit)
    {
        foreach(var filter in filters)
        {
            if (filter.Invoke(hit))
            {
                return true;
            }
        }
        return false;
    }

    public void SetInputFilter(Func<RaycastResult, bool> predicate, bool removeOtherFilters = false)
    {
        if (removeOtherFilters)
        {
            filters.Clear();
        }
        filters.Add(predicate);
    }
}
