using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMeshGenerator : MonoBehaviour
{
    //The triangle verts that we can see and edit in editor mode
    public List<Triangle> meshTriangles = new List<Triangle>(6);

    //The mesh that this script will create
    private Mesh meshToMake;
    //The vertices that make up the meshToMake
    private Vector3[] vertices;



    //Colors in the wireframe for each triangle so we can see the mesh in editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        foreach(Triangle tri in this.meshTriangles)
        {
            Gizmos.DrawLine(tri.vert1, tri.vert2);
            Gizmos.DrawLine(tri.vert2, tri.vert3);
            Gizmos.DrawLine(tri.vert3, tri.vert1);
        }
    }


	// Use this for initialization
	private void Awake ()
    {
		
	}
}

[System.Serializable]
public class Triangle
{
    public Vector3 vert1;
    public Vector3 vert2;
    public Vector3 vert3;
}