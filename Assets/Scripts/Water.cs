using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
	[HeaderAttribute("Water")]
	[SerializeField] [Range(0.0f, 1.0f)] float m_dampening = 0.04f;
    [SerializeField] [Range(1.0f, 90.0f)] float m_simulationFPS = 60.0f;

	[HeaderAttribute("Mesh")]
	[SerializeField] [Range(2, 200)] int m_xMeshVertexNum = 2;
	[SerializeField] [Range(2, 200)] int m_zMeshVertexNum = 2;
	[SerializeField] [Range(1.0f, 200.0f)] float m_xMeshSize = 40.0f;
	[SerializeField] [Range(1.0f, 200.0f)] float m_zMeshSize = 40.0f;

    public float MeshWidth { get { return m_xMeshSize; } }
    public float MeshHeight { get { return m_zMeshSize; } }

    MeshFilter m_meshFilter = null;
	MeshCollider m_meshCollider = null;
	Mesh m_mesh = null;
	Vector3[] m_vertices;

	int frame = 0;
	float[,] m_buffer1;
	float[,] m_buffer2;

	float m_simulationTime = 0.0f;

	void Start()
	{
		m_meshFilter = GetComponent<MeshFilter>();
		m_meshCollider = GetComponent<MeshCollider>();
		m_mesh = m_meshFilter.mesh;
		MeshGenerator.Plane(m_meshFilter, m_xMeshSize, m_zMeshSize, m_xMeshVertexNum, m_zMeshVertexNum);
		m_vertices = m_mesh.vertices;

		m_buffer1 = new float[m_xMeshVertexNum, m_zMeshVertexNum];
		m_buffer2 = new float[m_xMeshVertexNum, m_zMeshVertexNum];
	}

	void Update()
	{
		m_simulationTime = m_simulationTime + Time.deltaTime;
		while (m_simulationTime > (1.0f / m_simulationFPS))
		{
			if (frame % 2 == 0) UpdateSimulation(ref m_buffer1, ref m_buffer2);
			else UpdateSimulation(ref m_buffer2, ref m_buffer1);
			frame++;
			m_simulationTime = m_simulationTime - (1.0f / m_simulationFPS);
		}

		for (int i = 0; i < m_xMeshVertexNum; i++)
		{
			for (int j = 0; j < m_zMeshVertexNum; j++)
			{
				float height = (frame % 2 == 0) ? m_buffer1[i, j] : m_buffer2[i, j];
				m_vertices[i + (j * m_xMeshVertexNum)].y = height;
			}
		}

		m_mesh.vertices = m_vertices;
		m_mesh.RecalculateNormals();
		m_mesh.RecalculateBounds();
		m_meshCollider.sharedMesh = m_mesh;
	}

    struct Pos
    {
        public Pos(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X;
        public int Y;
    }
	void UpdateSimulation(ref float[,] current, ref float[,] previous)
	{
        for (int x = 0; x < previous.GetLength(1); x++)
        {
            for (int y = 0; y < previous.GetLength(0); y++)
            {
                int numOfNums = 0;
                float value = 0.0f;
                value = SumCircle(previous, x, y, out numOfNums);
                value /= (numOfNums / 1.5f);
                value -= current[y, x];
                value -= (value * m_dampening);
                current[y, x] = value;
            }
        }
	}

    float SumCircle(float[,] array, int x, int y, out int numOfNums)
    {
        numOfNums = 0;
        float sum = 0.0f;

        sum = GetValue(array, x - 1, y, ref numOfNums) +
                GetValue(array, x + 1, y, ref numOfNums) +
                GetValue(array, x, y - 1, ref numOfNums) +
                GetValue(array, x, y + 1, ref numOfNums) +
                GetValue(array, x + 1, y + 1, ref numOfNums) +
                GetValue(array, x - 1, y + 1, ref numOfNums) +
                GetValue(array, x + 1, y - 1, ref numOfNums) +
                GetValue(array, x - 1, y - 1, ref numOfNums) +
                GetValue(array, x, y + 2, ref numOfNums) +
                GetValue(array, x, y - 2, ref numOfNums) +
                GetValue(array, x + 2, y, ref numOfNums) +
                GetValue(array, x - 2, y, ref numOfNums);

        return sum;
    }

    float GetValue(float[,] array, int x, int y, ref int numOfNums)
    {
        float value = 0.0f;
        
        if (x >= 0 && y >= 0 && x < array.GetLength(1) && y < array.GetLength(0))
        {
            value = array[y, x];
            numOfNums++;
        }

        return value;
    }

	public void Touch(Ray ray, float strength)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, out raycastHit))
		{
			MeshCollider meshCollider = raycastHit.collider as MeshCollider;
			if (meshCollider == m_meshCollider)
			{
				int[] triangles = m_mesh.triangles;
				int index = triangles[raycastHit.triangleIndex * 3];
				int x = index % m_xMeshVertexNum;
				int z = index / m_xMeshVertexNum;
				m_buffer1[x, z] = strength;
			}
            else
            {
                print("fail");

            }
        }
        else
        {
            print("fail");
        }
	}
}
