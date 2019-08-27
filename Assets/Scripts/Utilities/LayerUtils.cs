using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class LayerUtils
{
    public static void SetChildLayerRecursivly(Transform transform, int layer)
    {
        foreach (Transform child in transform)
        {
            if (child.childCount > 0)
                SetChildLayerRecursivly(child, layer);

            child.gameObject.layer = layer;
        }
    }
}
