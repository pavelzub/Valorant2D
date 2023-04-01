using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Xml;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public int frameHeight = 10;
    public int frameWidth = 10;
    public float cellSize = 1;
    public Size frameSize = new Size(10, 10);
    public GameObject horizontalWallPrefab;
    public GameObject verticalWallPrefab;
    public GameObject holder;

    private static int groupIndex = 0;

    class Cell
    {
        public bool needBottomWall = false;
        public bool needRightWall = false;
        public GameObject bottomWall = null;
        public GameObject rightWall = null;
        public int group = 0;


        public Cell() { }
        public Cell(Cell other) {

            needBottomWall = other.needBottomWall;
            needRightWall = other.needRightWall;
            group = other.group;
        }

        public void InitWalls(Vector2 pos, GameObject horizontalWallPrefab, GameObject verticalWallPrefab, GameObject warldMover, float cellSize) {
            if (needBottomWall) {
                if (bottomWall == null) {
                    bottomWall = Instantiate(horizontalWallPrefab, warldMover.transform);
                    bottomWall.transform.Translate(new Vector3(pos.x, pos.y - cellSize / 2f, 0f));
                }
            }
            if (needRightWall) {
                if (rightWall == null) {
                    rightWall = Instantiate(verticalWallPrefab, warldMover.transform);
                    rightWall.transform.Translate(new Vector3(pos.x + cellSize / 2f, pos.y, 0));
                }
            }
        }
    }

    private List<Cell[]> arr = new List<Cell[]>();

    void Start() {
        frameSize = new Size(frameWidth, frameHeight);

        ResetMaze();
        GenerateMaze();
        InitMaze();
        CreateFrame();
    }

    void ResetMaze() {
        arr.ForEach(item =>
        {
            for (int i = 0; i < item.Length; i++) {
                if (item[i].bottomWall != null) {
                    Destroy(item[i].bottomWall);
                }
                if (item[i].rightWall != null) {
                    Destroy(item[i].rightWall);
                }
            }
        });
    }

    bool RandomBool() {
        return UnityEngine.Random.Range(0, 2) == 0;
    }

    void GenerateMaze() {
        int index = 0;
        arr.Add(new Cell[frameSize.Width]);

        for (int i = 0; i < frameSize.Width; i++) {
            arr[0][i] = new Cell();
            arr[0][i].group = index++;
        }

        for (int row = 0; row < frameSize.Height; row++) {
            GeneraneNewRow(false);
        }
    }

    void GeneraneNewRow(bool pizdato) {
        //новая строка
        arr.Add(Array.ConvertAll(arr.Last(), originalItem => new Cell(originalItem)));

        var newRow = arr.Last();

        for (int i = 0; i < frameSize.Width; i++) {
            if (newRow[i].needBottomWall) {
                newRow[i].group = groupIndex++;
            }
            newRow[i].needBottomWall = false;
            newRow[i].needRightWall = false;
        }

        //рандомим правые стены
        for (int i = 0; i < frameSize.Width - 1; i++) {
            if (RandomBool() || newRow[i].group == newRow[i + 1].group) {
                newRow[i].needRightWall = true;
            }
            else {
                if (newRow[i + 1].group > newRow[i].group) {
                    newRow[i + 1].group = newRow[i].group;
                }
                newRow[i + 1].group = newRow[i].group;
            }
        }

        Dictionary<int, int> dict = new Dictionary<int, int>();
        for (int i = 0; i < frameSize.Width; i++) {
            if (dict.ContainsKey(newRow[i].group)) {
                dict[newRow[i].group]++;
            }
            else {
                dict.Add(newRow[i].group, 1);
            }
        }

        //рандомим нижние стены
        for (int i = 0; i < frameSize.Width; i++) {
            if (dict[newRow[i].group] > 1 && RandomBool()) {
                newRow[i].needBottomWall = true;
                dict[newRow[i].group]--;
            }
        }
    }



    void InitMaze() {
        Vector2 pos = new Vector2(0, 0);
        arr.ForEach(row =>
        {
            for (int i = 0; i < row.Length; i++) {
                row[i].InitWalls(pos, horizontalWallPrefab, verticalWallPrefab, holder, cellSize);
                pos.x += cellSize;
            }
            pos.y -= cellSize;
            pos.x = 0;
        });
    }

    void CreateFrame() {
        Point pos = new Point(0, 0);
        for (int i = 0; i < frameSize.Width; i++) {
            Instantiate(verticalWallPrefab, new Vector3(-cellSize / 4f, (-i + 1) * cellSize, 0f), Quaternion.identity);
            Instantiate(verticalWallPrefab, new Vector3((frameSize.Width - 0.25f) * cellSize, (-i + 1) * cellSize, 0f), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
