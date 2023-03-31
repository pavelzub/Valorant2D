using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public Size frameSize = new Size(10, 10);
    public GameObject horizontalWallPrefab;
    public GameObject verticalWallPrefab;

    class Cell {
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

        public void InitWalls(Point pos, GameObject horizontalWallPrefab, GameObject verticalWallPrefab) {
            if (needBottomWall) {
                if (bottomWall == null) {
                    bottomWall = Instantiate(horizontalWallPrefab, new Vector3(pos.X, pos.Y - 0.5f, 0f), Quaternion.identity);
                }
            }
            if (needRightWall) {
                if (rightWall == null) {
                    rightWall = Instantiate(verticalWallPrefab, new Vector3(pos.X + 0.5f, pos.Y, 0), Quaternion.identity);
                }
            }
        }
    }

    private List<Cell[]> arr = new List<Cell[]>();

    void Start()
    {
        ResetMaze();
        GenerateMaze();
        InitMaze();
        CreateFrame();
    }

    void ResetMaze() {
        arr.ForEach(item =>
        {
            for (int i = 0; i < item.Length; i++)
            {
                if (item[i].bottomWall != null) {
                    Destroy(item[i].bottomWall);
                }
                if (item[i].rightWall != null)
                {
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

            //рандомим правые стены
            for (int i = 0; i < frameSize.Width - 1; i++) {
                if (RandomBool() || arr[row][i].group == arr[row][i + 1].group) {
                    arr[row][i].needRightWall = true;
                }
                else {
                    arr[row][i + 1].group = arr[row][i].group;
                }
            }


            Dictionary<int, int> dict = new Dictionary<int, int>();
            for (int i = 0; i < frameSize.Width; i++) {
                if (dict.ContainsKey(arr[row][i].group)) {
                    dict[arr[row][i].group]++;
                }
                else {
                    dict.Add(arr[row][i].group, 1);
                }
            }

            //рандомим нижние стены
            for (int i = 0; i < frameSize.Width; i++) {
                if (dict[arr[row][i].group] > 1 && RandomBool()) {
                    arr[row][i].needBottomWall= true;
                    dict[arr[row][i].group]--;
                }
            }

            //новая строка
            arr.Add(Array.ConvertAll(arr[row], originalItem => new Cell(originalItem)));

            for (int i = 0; i < frameSize.Width; i++) {
                if (arr[row + 1][i].needBottomWall) {
                    arr[row + 1][i].group = index++;
                }
                arr[row + 1][i].needBottomWall = false;
                arr[row + 1][i].needRightWall = false;
            }
        }
    }

    void InitMaze() {
        Point pos = new Point(0, 0);
        arr.ForEach(row => {
            for (int i = 0; i < row.Length; i++) {
                row[i].InitWalls(pos, horizontalWallPrefab, verticalWallPrefab);
                pos.X++;
            }
            pos.Y--;
            pos.X = 0;
        });
    }

    void CreateFrame() {
        Point pos = new Point(0, 0);
        for (int i = 0; i < frameSize.Width; i++) {
            Instantiate(verticalWallPrefab, new Vector3(-0.5f, -i, 0f), Quaternion.identity);
            Instantiate(verticalWallPrefab, new Vector3(9.5f, -i, 0f), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
