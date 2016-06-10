﻿using UnityEngine;
using System.Collections;

public class blockController : MonoBehaviour
{
    public enum rotation { DOWN, RIGHT, UP, LEFT };
    public rotation actitveRotation = rotation.DOWN;
    public bool canFall = false;
    public bool speedUp = false;

    protected blockTileController[] tile = new blockTileController[4];
    protected arenaManager managerArena;
    protected blocksManager managerBlocks;

    protected float fallTimer = 0.6f;
    protected float fallTimerFast = 0.02f;
    protected float timer = 0;

    virtual protected void Start()
    {
        for (int i = 0; i < 4; ++i) tile[i] = transform.GetChild(i).GetComponent<blockTileController>();
        managerArena = FindObjectOfType<arenaManager>();
        managerBlocks = FindObjectOfType<blocksManager>();
    }

    virtual protected void Update()
    {
        if (canFall)
        {
            if (timer < 0)
            {
                fallDown();
                if (!speedUp) timer = fallTimer;
                else timer = fallTimerFast;
            }
            else timer -= Time.deltaTime;
        }
    }

    virtual public void rotate() { }
    virtual public void turnLeft() { }
    virtual public void turnRight() { }
    virtual protected void fallDown() { }

    virtual public void randColor()
    {
        blockTileController.blockColor col = (blockTileController.blockColor)Random.Range(0, 7);
        foreach (Transform tl in transform) tl.GetComponent<blockTileController>().setColor(col);
    }

    protected void moveTilesHorizontal(int direction)
    {
        transform.position = managerArena.tile[tile[0].arenaTile.posX + direction, tile[0].arenaTile.posY].transform.position;
    }

    protected void moveTilesVertical(int direction = 1)
    {
        transform.position = managerArena.tile[tile[0].arenaTile.posX, tile[0].arenaTile.posY + direction].transform.position;
    }

    protected bool canTurn(int[] tileIndex, int direction)
    {
        foreach(int i in tileIndex)
        {
            if (tile[i].arenaTile.posX + direction > 9 || tile[i].arenaTile.posX + direction < 0 || !managerArena.tile[tile[i].arenaTile.posX + direction, tile[i].arenaTile.posY].isEmpty) return false;
        }
        return true;
    }

    protected bool canFallDown(int[] tileIndex, int tileDown)
    {
        if (tile[tileDown].arenaTile.posY < 19)
        {
            foreach(int i in tileIndex)
            {
                if (!managerArena.tile[tile[i].arenaTile.posX, tile[i].arenaTile.posY + 1].isEmpty) return false;
            }
        }
        else return false;
        return true;
    }
}
