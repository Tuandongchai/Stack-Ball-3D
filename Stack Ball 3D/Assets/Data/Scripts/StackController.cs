using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackController : MonoBehaviour
{
    [SerializeField] private StackPartController[] stackPartControlls = null;

    public virtual void ShatterAllParts()
    {
        if (transform.parent != null)
        {
            transform.parent = null;
            FindObjectOfType<Ball>().IncreaseBrokenStacks();
        }
        foreach (StackPartController o in stackPartControlls)
        {
            o.Shatter();
        }
        StartCoroutine(RemoveParts());
    }
    IEnumerator RemoveParts()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
