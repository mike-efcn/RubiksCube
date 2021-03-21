using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int cubeOrder = 3;

    [SerializeField]
    private float rotatingDuration = 1f;

    [SerializeField]
    private GameObject cubePrefab;

    private Vector3 center = Vector3.zero;
    private List<GameObject> allCubes = new List<GameObject>();

    private bool rotating = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Game Start");

        initCameras();
        resetCubes();
    }

    private void initCameras()
    {
        var mainCamera = GameObject.Find("MainCamera");
        if (mainCamera == null)
        {
            Debug.LogError("Cannot find main camera");
        }
        else
        {
            mainCamera.transform.position = 25 * cubeOrder * new Vector3(1, 1, 1);
            mainCamera.transform.LookAt(new Vector3(0, 0, 0));
        }

        var secondCamera = GameObject.Find("SecondCamera");
        if (secondCamera == null)
        {
            Debug.LogError("Cannot find second camera");
        }
        else
        {
            secondCamera.transform.position = -25 * cubeOrder * new Vector3(1, 1, 1);
            secondCamera.transform.LookAt(new Vector3(0, 0, 0));
        }
    }

    private void resetCubes()
    {
        foreach (var cube in allCubes)
        {
            Destroy(cube);
        }
        allCubes.Clear();

        var anchor = GameObject.Find("Anchor");
        if (anchor == null)
        {
            Debug.LogError("Cannot find anchor");
            return;
        }

        for (int zi = 0; zi < cubeOrder; zi += 1)
            for (int yi = 0; yi < cubeOrder; yi += 1)
                for (int xi = 0; xi < cubeOrder; xi += 1)
                {
                    var cube = Instantiate(cubePrefab, anchor.transform);
                    cube.GetComponent<CubeUnit>().InitComponent(
                        new Vector3Int(xi, yi, zi),
                        cubeOrder
                    );
                    allCubes.Add(cube);
                }

        for (int i = 0; i < allCubes.Count; i += 1)
        {
            var cube = allCubes[i].GetComponent<CubeUnit>();
            Debug.Log(string.Format("{0} => {1}", i, cubeIndex(cube.indexVec, cubeOrder)));
        }
        Debug.LogFormat(
            "{0}:{1}\n{2}:{3}",
            allCubes[0].GetComponent<CubeUnit>().indexVec,
            allCubes[0].transform.position,
            allCubes[allCubes.Count - 1].GetComponent<CubeUnit>().indexVec,
            allCubes[allCubes.Count - 1].transform.position
        );
        center = 0.5f * (allCubes[0].transform.position + allCubes[allCubes.Count - 1].transform.position);
    }

    private static int cubeIndex(Vector3Int vec, int order)
    {
        return vec.x + vec.y * order + vec.z * order * order;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void cubeAction(string action)
    {
        Debug.Log(action);
        if (rotating)
        {
            Debug.LogWarning("Rotating");
            return;
        }

        var cubes = ImpactedCubes(action);
        var axis = RotationAxis(action);
        var angle = RotationAngle(action);
        if (cubes.Count == 0 || axis == null || Mathf.Abs(angle) < float.Epsilon)
        {
            return;
        }

        StartCoroutine(RotateCubes(
            cubes,
            center,
            (Vector3)axis,
            angle,
            rotatingDuration
        ));
    }

    private List<GameObject> ImpactedCubes(string action)
    {
        List<GameObject> cubes;
        switch (action)
        {
            case "X":
            case "Xi":
            case "Y":
            case "Yi":
            case "Z":
            case "Zi":
                cubes = allCubes;
                break;
            default:
                Debug.LogErrorFormat("Unknown cubes for action: {0}", action);
                return new List<GameObject>();
        }
        return cubes;
    }

    private Vector3? RotationAxis(string action)
    {
        switch (action)
        {
            case "R":
            case "Ri":
            case "R2":
            case "X":
            case "Xi":
                return Vector3.forward;
            case "L":
            case "Li":
            case "L2":
                return Vector3.back;
            case "U":
            case "Ui":
            case "U2":
            case "Y":
            case "Yi":
                return Vector3.up;
            case "D":
            case "Di":
            case "D2":
                return Vector3.down;
            case "F":
            case "Fi":
            case "F2":
            case "Z":
            case "Zi":
                return Vector3.right;
            case "B":
            case "Bi":
            case "B2":
                return Vector3.left;
            default:
                Debug.LogErrorFormat("Unknown axis for action: {0}", action);
                return null;
        }
    }

    private float RotationAngle(string action)
    {
        switch (action)
        {
            case "R":
            case "U":
            case "F":
            case "L":
            case "D":
            case "B":
            case "X":
            case "Y":
            case "Z":
                return 90f;
            case "Ri":
            case "Ui":
            case "Fi":
            case "Li":
            case "Di":
            case "Bi":
            case "Xi":
            case "Yi":
            case "Zi":
                return -90f;
            case "R2":
            case "U2":
            case "F2":
            case "L2":
            case "D2":
            case "B2":
                return 180f;
            default:
                Debug.LogErrorFormat("Unknown angle for action: {0}", action);
                return 0f;
        }
    }

    public IEnumerator RotateCubes(
        List<GameObject> cubes,
        Vector3 point,
        Vector3 axis,
        float angle,
        float duration
    )
    {
        rotating = true;

        var t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            var deltaAngle = Time.deltaTime / duration * angle;
            foreach (var cube in cubes)
            {
                cube.transform.RotateAround(point, axis, deltaAngle);
            }
            yield return null;
        }

        #region fix fraction position and rotation
        foreach (var cube in cubes)
        {
            var oldVec = cube.transform.position;
            cube.transform.position = new Vector3(
                (int)Math.Round(oldVec.x, 0),
                (int)Math.Round(oldVec.y, 0),
                (int)Math.Round(oldVec.z, 0)
            );
            Debug.LogFormat(
                "{0} => pos:{1},rot:{2}",
                cube.GetComponent<CubeUnit>().indexVec,
                cube.transform.position,
                cube.transform.rotation
            );
        }
        #endregion

        rotating = false;
        yield return null;
    }
}
