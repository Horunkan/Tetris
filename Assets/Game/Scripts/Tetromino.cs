﻿using System.Collections;
using UnityEngine;

public class Tetromino : MonoBehaviour {
    public enum TurnDirection { LEFT, RIGHT }

    [SerializeField]
    private bool rotation = true;
    private const float tileSize = 0.4096f;
    private TetrominoTile[] rotationColliders = new TetrominoTile[4];
    private TetrominoTile[] tetrominoTiles = new TetrominoTile[4];
    private Game game;
    private float fallingTime;
    private TetrominoSpawner spawner;

    void Start() {
        rotationColliders = transform.GetChild(0).GetComponentsInChildren<TetrominoTile>();
        for (int i = 1; i < transform.childCount; ++i) tetrominoTiles[i - 1] = transform.GetChild(i).GetComponent<TetrominoTile>();
        game = Camera.main.GetComponent<Game>();
        spawner = GameObject.FindGameObjectWithTag("TetrominoSpawner").GetComponent<TetrominoSpawner>();
        fallingTime = game.tetrominoFallTime;
        StartCoroutine(fallingCoroutine());
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.W)) rotate();
        if (Input.GetKeyDown(KeyCode.S)) speedUpFalling();
        if (Input.GetKeyDown(KeyCode.A)) turn(TurnDirection.LEFT);
        if (Input.GetKeyDown(KeyCode.D)) turn(TurnDirection.RIGHT);
    }

    private void rotate() {
        if(rotation && canRotate()) transform.Rotate(0, 0, 90f);
    }

	private bool canRotate() {
        foreach(var obj in rotationColliders) {
            if (!obj.canRotate()) return false;
        }
        return true;
    }

    private IEnumerator fallingCoroutine() {
        bool falling = true;

        while(falling) {
            yield return new WaitForSeconds(fallingTime);
            
            try {
                foreach (var tile in tetrominoTiles) {
                    falling = tile.canFallDown();
                    if (!falling) break;
                }
            }
            catch (System.NullReferenceException ex) {
                Debug.LogWarning(ex, gameObject);
            }
            catch (System.Exception ex) {
                Debug.LogError(string.Format("Unhandled exception: {0}", ex), gameObject);
            }

            if(falling) transform.position = new Vector3(transform.position.x, transform.position.y - tileSize, transform.position.z);
        }

        if(!falling) endFalling();
    }

    private void turn(TurnDirection dir) {
        bool canTurn = true;

        try {
            foreach (var tile in tetrominoTiles) {
                canTurn = tile.canTurn(dir);
                if (!canTurn) break;
            }
        }
        catch (System.NullReferenceException ex) {
            Debug.LogWarning(ex, gameObject);
        }
        catch (System.Exception ex) {
            Debug.LogError(string.Format("Unhandled exception: {0}", ex), gameObject);
        }

        if(canTurn) {
            if (dir == TurnDirection.LEFT) transform.position = new Vector3(transform.position.x - tileSize, transform.position.y, transform.position.z);
            else transform.position = new Vector3(transform.position.x + tileSize, transform.position.y, transform.position.z);
        }
    }

    private void speedUpFalling() {
        fallingTime *= game.speedUpMultiplier;
    }

    private void endFalling() {
        foreach (var tile in tetrominoTiles) tile.endFalling();

        spawner.spawn();
        Destroy(transform.GetChild(0).gameObject); //Rotation colliders
        Destroy(this);
    }
}
