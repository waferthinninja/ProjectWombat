using UnityEngine;

namespace Helper
{
    public static class GameObjectExtension
    {
        public static void SetLayer(this GameObject parent, int layer, bool includeChildren = true)
        {
            parent.layer = layer;
            if (includeChildren)
                foreach (var trans in parent.transform.GetComponentsInChildren<Transform>(true))
                    trans.gameObject.layer = layer;
        }
    }
}