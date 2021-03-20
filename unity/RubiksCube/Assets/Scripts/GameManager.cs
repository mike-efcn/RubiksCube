using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int cubeOrder = 3;

    [SerializeField]
    private GameObject cubePrefab;

    private List<GameObject> cubes = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Game Start");
        var mainCamera = GameObject.Find("MainCamera");
        if (mainCamera == null)
        {
            Debug.LogError("Cannot find main camera");
            return;
        }

        mainCamera.transform.position = 25 * cubeOrder * new Vector3(1, 1, 1);
        mainCamera.transform.LookAt(new Vector3(0, 0, 0));

        resetCubes();
    }

    private void resetCubes()
    {
        foreach (var cube in cubes)
        {
            Destroy(cube);
        }
        cubes.Clear();

        var anchor = GameObject.Find("Anchor");
        if (anchor == null)
        {
            Debug.LogError("Cannot find anchor");
            return;
        }

        for (int xi = 0; xi < cubeOrder; xi += 1)
        {
            for (int yi = 0; yi < cubeOrder; yi += 1)
            {
                for (int zi = 0; zi < cubeOrder; zi += 1)
                {
                    var cube = Instantiate(cubePrefab, anchor.transform);
                    cube.GetComponent<CubeUnit>().indexVec = new Vector3Int(xi, yi, zi);
                    cube.GetComponent<CubeUnit>().order = cubeOrder;
                    cubes.Add(cube);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
