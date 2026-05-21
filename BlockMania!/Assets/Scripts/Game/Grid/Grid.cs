using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int columns = 0;
    public int rows = 0;
    public float squareGap = 0.1f;
    public GameObject gridSquare;
    public Vector2 startPosition = new Vector2(0.0f, 0.0f);
    public float squareScale = 0.5f;
    public float everySquareOffset = 0.0f;

    // Perbaikan: Diubah dari _offset menjadi offset agar pas dengan pemanggilan di bawah
    private Vector2 offset = new Vector2(0.0f, 0.0f);
    private List<GameObject> _gridSquares = new List<GameObject>();
    // Perbaikan: Diubah dari 'object' menjadi 'RectTransform' agar properti .rect bisa diakses
    private RectTransform square_rect;

    void Start()
    {
        // Mengambil komponen RectTransform dari prefab sebelum grid dibuat
        if (gridSquare != null)
        {
            square_rect = gridSquare.GetComponent<RectTransform>();
        }

        CreateGrid();
    }

    private void CreateGrid()
    {
        SpawnGridSquares();
        SetGridSquaresPositions();
    }

    private void SpawnGridSquares()
    {
        //0, 1, 2, 3, 4,
        //5, 6, 7, 8, 9
        int square_index = 0;

        for (var row = 0; row < rows; ++row)
        {
            for (var column = 0; column < columns; ++column)
            {
                _gridSquares.Add(Instantiate(gridSquare) as GameObject);
                _gridSquares[_gridSquares.Count - 1].transform.SetParent(this.transform);
                _gridSquares[_gridSquares.Count - 1].transform.localScale = new Vector3(squareScale, squareScale, squareScale);
                _gridSquares[_gridSquares.Count - 1].GetComponent<GridSquare>().SetImage(square_index % 2 == 0);
                square_index++;
            }
        }
    }

    private void SetGridSquaresPositions()
    {
        int column_number = 0;
        int row_number = 0;
        Vector2 square_gap_number = new Vector2(0.0f, 0.0f);
        bool row_moved = false;

        // Perbaikan: Menghilangkan garis bawah (_offset -> offset) dan memastikan tidak null
        if (square_rect != null)
        {
            offset.x = square_rect.rect.width * square_rect.transform.localScale.x + everySquareOffset;
            offset.y = square_rect.rect.height * square_rect.transform.localScale.y + everySquareOffset;
        }

        foreach (GameObject square in _gridSquares)
        {
            if (column_number + 1 > columns)
            {
                square_gap_number.x = 0;
                //gotonect column
                column_number = 0;
                row_number++;
                row_moved = false;
            }

            // Perbaikan: Menghilangkan garis bawah (_offset -> offset)
            var pos_x_offset = offset.x * column_number + (square_gap_number.x * squareGap);
            var pos_y_offset = offset.y * row_number + (square_gap_number.y * squareGap);

            if (column_number > 0 && column_number % 3 == 0)
            {
                square_gap_number.x++;
                pos_x_offset += squareGap;
            }

            if (row_number > 0 && row_number % 3 == 0 && row_moved == false)
            {
                row_moved = true;
                square_gap_number.y++;
                pos_y_offset += squareGap;
            }

            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPosition.x + pos_x_offset, startPosition.y - pos_y_offset);
            square.GetComponent<RectTransform>().localPosition = new Vector3(startPosition.x + pos_x_offset, startPosition.y - pos_y_offset, 0.0f);
            column_number++;
        }
    }
}