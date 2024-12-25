using UnityEngine;

public class BalancedOrnamentSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ornamentPrefab;
    [SerializeField] private GameObject[] treeTriangles;
    [SerializeField] private int maxOrnamentsPerRow = 5; // Maximum ornaments at the bottom-most row
    [SerializeField] private int minOrnamentsPerRow = 1; // Minimum ornaments at the top-most row
    [SerializeField] private int rowsPerTriangle = 4; // Number of rows per triangle

    void Start()
    {
        foreach (GameObject triangle in treeTriangles)
        {
            PlaceOrnamentsInTriangle(triangle);
        }
    }

    void PlaceOrnamentsInTriangle(GameObject triangle)
    {
        SpriteRenderer spriteRenderer = triangle.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) return;

        Bounds bounds = spriteRenderer.bounds;

        float minX = bounds.min.x;
        float maxX = bounds.max.x;
        float minY = bounds.min.y;
        float maxY = bounds.max.y;
        float centerX = bounds.center.x;

        for (int row = 0; row < rowsPerTriangle; row++)
        {
            float t = (float)row / (rowsPerTriangle - 1); // Normalized row index (0 to 1)
            float rowY = Mathf.Lerp(minY, maxY, t); // Height of the current row

            // Gradually decrease the number of ornaments per row from max to min
            int ornamentsInRow = Mathf.RoundToInt(Mathf.Lerp(maxOrnamentsPerRow, minOrnamentsPerRow, t));

            // Calculate horizontal span for this row
            float widthAtRowY = (maxX - minX) * ((rowY - minY) / (maxY - minY));
            float halfWidth = widthAtRowY / 2;

            for (int i = 0; i < ornamentsInRow; i++)
            {
                // Evenly space ornaments in the row
                float x = Mathf.Lerp(centerX - halfWidth, centerX + halfWidth, (float)(i + 1) / (ornamentsInRow + 1));

                // Instantiate the ornament
                Vector3 position = new Vector3(x, rowY, triangle.transform.position.z);
                Instantiate(ornamentPrefab, position, Quaternion.identity, triangle.transform);
            }
        }
    }
}
