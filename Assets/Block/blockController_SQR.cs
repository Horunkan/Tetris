﻿using UnityEngine;
using System.Collections;

public class blockController_SQR : blockController
{
    override protected void turnLeft()
    {
        if (canTurn(new int[2] { 0, 2 }, -1)) moveTilesHorizontal(-1);
    }

    override protected void turnRight()
    {
        if (canTurn(new int[2] { 1, 3 }, 1)) moveTilesHorizontal(1);
    }

    override protected void fallDown()
    {
        if (actitveRotation == rotation.DOWN && canFallDown(new int[2] { 2, 3 }, 2)) moveTilesVertical();
        else if (actitveRotation == rotation.RIGHT) { }
        else if (actitveRotation == rotation.UP) { }
        else if (actitveRotation == rotation.LEFT) { }
        else
        {
            canFall = false;
            foreach (blockTileController tl in tile) tl.blockControllerRemoved = true;
            managerBlocks.pushBlock();
            Destroy(GetComponent<blockController>());
        }
    }
}
