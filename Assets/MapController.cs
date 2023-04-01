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
    public float coinChance = 10;
    public float boosterChance = 1;
    public Size frameSize = new Size(10, 10);
    public GameObject coinPrefab;
    public GameObject boosterPrefab;
    public GameObject horizontalWallPrefab;
    public GameObject verticalWallPrefab;
    public GameObject horizontalSpikyWallPrefab;
    public GameObject verticalSpikyWallPrefab;
    public GameObject holder;
    public GameObject camera;
    private float cameraBottom;
    private int currentRow = 0;
    private int crasivayaRow = -1;
    public int spikyWallChance = 10;

    private static int groupIndex = 0;

    class Cell
    {
        public bool needBottomWall = false;
        public bool needRightWall = false;
        public GameObject bottomWall = null;
        public GameObject rightWall = null;
        public GameObject coin = null;
        public GameObject booster = null;
        public int group = 0;
        public bool needCoin = false;
        public bool needBooster = false;


        public Cell() {
        }
        public Cell(Cell other) {

            needBottomWall = other.needBottomWall;
            needRightWall = other.needRightWall;
            group = other.group;
        }
        bool NeedSpikyWall(int spikyWallChance) {
            return UnityEngine.Random.Range(0, 100) <= spikyWallChance;
        }

        public void InitWalls(Vector2 pos, GameObject horizontalWallPrefab, GameObject verticalWallPrefab, GameObject horizontalSpikyWallPrefab, GameObject verticalSpikyWallPrefab, GameObject coinPrefab, GameObject boosterPrefab, GameObject warldMover, float cellSize, int spikyWallChance) {
            if (needBottomWall) {
                if (bottomWall == null) {
                    if (NeedSpikyWall(spikyWallChance)) {
                        bottomWall = Instantiate(horizontalSpikyWallPrefab, warldMover.transform);
                    }
                    else {
                        bottomWall = Instantiate(horizontalWallPrefab, warldMover.transform);
                    }

                    bottomWall.transform.Translate(new Vector3(pos.x, pos.y + cellSize / 2f, 0f));
                }
            }
            if (needRightWall) {
                if (rightWall == null) {
                    if (NeedSpikyWall(spikyWallChance)) {
                        rightWall = Instantiate(verticalSpikyWallPrefab, warldMover.transform);
                    }
                    else {
                        rightWall = Instantiate(verticalWallPrefab, warldMover.transform);
                    }
                    rightWall.transform.Translate(new Vector3(pos.x + cellSize / 2f, pos.y, 0));
                }
            }

            if (needCoin) {
                if (coin == null) {
                    coin = Instantiate(coinPrefab, warldMover.transform);
                    coin.transform.Translate(new Vector3(pos.x, pos.y, 0));
                }
            }
            else if (needBooster) {
                if (booster == null) {
                    booster = Instantiate(boosterPrefab, warldMover.transform);
                    booster.transform.Translate(new Vector3(pos.x, pos.y, 0));
                }
            }
        }
    }

    private List<Cell[]> arr = new List<Cell[]>();

    void Start() {
        frameSize = new Size(frameWidth, frameHeight);
        crasivayaRow = frameSize.Height * 2;

        InitCamera();
        ResetMaze();
        GenerateMaze();
        CreateFrame();
    }

    void InitCamera() {
        var cameraComp = camera.GetComponent<Camera>();
        var cameraHeight = 2f * cameraComp.orthographicSize;
        cameraBottom = cameraComp.transform.position.y - cameraHeight / 2f;
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

    bool NeedCoin() {
        return UnityEngine.Random.Range(0, 100) <= coinChance;
    }

    bool NeedBooster() {
        return UnityEngine.Random.Range(0, 100) <= boosterChance;
    }

    void GenerateMaze() {
        int index = 0;
        arr.Add(new Cell[frameSize.Width]);

        for (int i = 0; i < frameSize.Width; i++) {
            arr[0][i] = new Cell();
            arr[0][i].group = index++;
        }

        for (int row = 0; row < frameSize.Height * 1.5; row++) {
            GeneraneNewRow();
        }
    }

    void GeneraneNewRow() {
        //новая строка
        arr.Add(Array.ConvertAll(arr.Last(), originalItem => new Cell(originalItem)));

        var newRow = arr.Last();

        for (int i = 0; i < frameSize.Width; i++) {
            if (newRow[i].needBottomWall) {
                newRow[i].group = groupIndex++;
            }
            newRow[i].needBottomWall = false;
            newRow[i].needRightWall = false;
            newRow[i].needCoin = NeedCoin();
            newRow[i].needBooster = !newRow[i].needCoin && NeedBooster();
        }   

        //рандомим правые стены
        for (int i = 0; i < frameSize.Width - 1; i++) {
            if (RandomBool() || newRow[i].group == newRow[i + 1].group) {
                newRow[i].needRightWall = true;
            }
            else {
                //if (newRow[i + 1].group > newRow[i].group) {
                //    newRow[i + 1].group = newRow[i].group;
                //}
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

        crasivayaRow--;
        if (crasivayaRow < 0) {
            crasivayaRow = frameSize.Height * 2;
            SdelatKrasivo();
        }

        for (int i = 0; i < newRow.Length; i++) {
            newRow[i].InitWalls(new Vector2(i * cellSize, currentRow * cellSize), horizontalWallPrefab, verticalWallPrefab, horizontalSpikyWallPrefab, verticalSpikyWallPrefab, coinPrefab, boosterPrefab, holder, cellSize, spikyWallChance);
        }
        currentRow++;
    }

    void SdelatKrasivo() {
        var row = arr.Last();

        for (int i = 0; i < frameSize.Width - 1; i++) {
            if (row[i].group != row[i + 1].group) {
                row[i].needRightWall = false;
            }
            row[i].group = groupIndex++;
        }
    }

    void CreateFrame() {
        Point pos = new Point(0, 0);
        for (int i = 0; i < frameSize.Width; i++) {
            Instantiate(verticalWallPrefab, new Vector3(-cellSize / 4f, (-i + 1) * cellSize, 0f), Quaternion.identity);
            Instantiate(verticalWallPrefab, new Vector3((frameSize.Width - 0.25f) * cellSize, (-i + 1) * cellSize, 0f), Quaternion.identity);
        }
    }
    void ClearCell(Cell cell) {
        if (cell.bottomWall != null) {
            Destroy(cell.bottomWall);
        }

        if (cell.rightWall != null) {
            Destroy(cell.rightWall);
        }

        if (cell.coin != null) {
            Destroy(cell.coin);
        }

        if (cell.booster != null) {
            Destroy(cell.booster);
        }
    }

    // Update is called once per frame
    void Update() {
        if (arr.Count() > 0) {
            bool removeLine = false;
            bool haveBorder = false;
            var row = arr[0];
            for (int i = 0; i < row.Length; i++) {
                if (row[i].bottomWall != null && row[i].bottomWall.transform.position.y < cameraBottom - 2 ||
                    row[i].rightWall != null && row[i].rightWall.transform.position.y < cameraBottom - 2) {
                    removeLine = true;
                }

                haveBorder |= row[i].bottomWall != null || row[i].rightWall != null;
            }

            if (removeLine || !haveBorder) {
                for (int i = 0; i < row.Length; i++) {
                    ClearCell(row[i]);
                }

                arr.RemoveAt(0);
                GeneraneNewRow();
            }
        }
    }
}
