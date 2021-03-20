using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CubeUnit's position is its center
 */
public class CubeUnit : MonoBehaviour
{
    public Vector3Int indexVec;
    public int order;

    [SerializeField]
    public int scale = 1;

    [SerializeField]
    private Color colorOrange = new Color(1f, 0.5f, 0.5f);
    private Color colorTransparent = new Color(0, 0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = scale * new Vector3(indexVec.x, indexVec.y, indexVec.z);

        var front = gameObject.transform.Find("Front").gameObject;
        var back = gameObject.transform.Find("Back").gameObject;
        if (indexVec.x == order - 1)
        {
            // front
            front.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
            back.GetComponent<MeshRenderer>().material.SetColor("_Color", colorTransparent);
        }
        else if (indexVec.x == 0)
        {
            // back
            front.GetComponent<MeshRenderer>().material.SetColor("_Color", colorTransparent);
            back.GetComponent<MeshRenderer>().material.SetColor("_Color", colorOrange);
        }
        else
        {
            // other
            front.GetComponent<MeshRenderer>().material.SetColor("_Color", colorTransparent);
            back.GetComponent<MeshRenderer>().material.SetColor("_Color", colorTransparent);
        }

        var top = gameObject.transform.Find("Top").gameObject;
        var bottom = gameObject.transform.Find("Bottom").gameObject;
        if (indexVec.y == order - 1)
        {
            // top
            top.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
            bottom.GetComponent<MeshRenderer>().material.SetColor("_Color", colorTransparent);
        }
        else if (indexVec.y == 0)
        {
            // bottom
            top.GetComponent<MeshRenderer>().material.SetColor("_Color", colorTransparent);
            bottom.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.yellow);
        }
        else
        {
            // other
            top.GetComponent<MeshRenderer>().material.SetColor("_Color", colorTransparent);
            bottom.GetComponent<MeshRenderer>().material.SetColor("_Color", colorTransparent);
        }

        var left = gameObject.transform.Find("Left").gameObject;
        var right = gameObject.transform.Find("Right").gameObject;
        if (indexVec.z == order - 1)
        {
            // right
            left.GetComponent<MeshRenderer>().material.SetColor("_Color", colorTransparent);
            right.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.blue);
        }
        else if (indexVec.z == 0)
        {
            // left
            left.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
            right.GetComponent<MeshRenderer>().material.SetColor("_Color", colorTransparent);
        }
        else
        {
            // other
            left.GetComponent<MeshRenderer>().material.SetColor("_Color", colorTransparent);
            right.GetComponent<MeshRenderer>().material.SetColor("_Color", colorTransparent);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
