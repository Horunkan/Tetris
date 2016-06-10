﻿using UnityEngine;
using System.Collections;

public class blocksManager : MonoBehaviour
{
    public GameObject startTile;
    private nextBlockController nextBlock;

	void Awake()
    {
        nextBlock = FindObjectOfType<nextBlockController>();
    }
	
	void Update ()
    {
        if (transform.childCount == 0) pushBlock();
	}

    public void pushBlock()
    {
        GameObject buffer = Instantiate(nextBlock.getBlock()) as GameObject;
        buffer.transform.SetParent(transform);
        buffer.transform.SetAsLastSibling();
        buffer.transform.localPosition = startTile.transform.localPosition;

        buffer.GetComponent<blockController>().canFall = true;
        nextBlock.randNew();
    }

    public void removeAllBlocks()
    {
        foreach(Transform obj in transform)
        {
            Destroy(obj.gameObject);
        }
    }

    public GameObject getBlock()
    {
        return transform.GetChild(transform.childCount - 1).gameObject;
    }
}
