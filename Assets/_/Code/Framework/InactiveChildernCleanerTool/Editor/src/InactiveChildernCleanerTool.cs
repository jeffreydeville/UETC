using UnityEditor;
using UnityEngine;

public static class InactiveChildernCleanerTool 
{

    [MenuItem("GameObject/Tools/InactiveChildernCleanerTool", false, 1000)]
    static void CleanUnusedChildren()
    {
        GameObject selected = Selection.activeGameObject;
        if (selected == null) return;

        CleanRecursive(selected.transform);
    }

    static void CleanRecursive(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Transform child = parent.GetChild(i);
            CleanRecursive(child);

            if (!child.gameObject.activeInHierarchy)
                Undo.DestroyObjectImmediate(child.gameObject);
        }
    }

    [MenuItem("GameObject/Tools/InactiveChildernCleanerTool", false, 1000)]
    static bool CleanUnusedChildrenValidate()
    {
        return Selection.activeGameObject;
    }
}