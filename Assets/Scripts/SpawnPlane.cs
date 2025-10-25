using UnityEngine;

public class SpawnPlane : MonoBehaviour
{
	[Header("Grid Settings")]
	[Tooltip("Number of points per line in X direction")]
	public int pointCountInLine = 5;
	public float Length = 10f;

	[Header("Enemy Settings")]
	public GameObject EnemyPrefab;

	[SerializeField, HideInInspector]
	public Vector3[,] pointPositions;

	private Camera Camera;

	void Start()
	{
		Camera = Camera.main;
	}

	private void OnDrawGizmos()
	{
		if (pointCountInLine < 2)
		{
			pointPositions = null;
			return;
		}

		RecalculatePoints();

		Gizmos.color = Color.red;

		for (int x = 0; x < pointPositions.GetLength(0); x++)
		{
			for (int z = 0; z < pointPositions.GetLength(1); z++)
			{
				Gizmos.DrawSphere(pointPositions[x, z], 0.1f);
			}
		}
	}

	private void RecalculatePoints()
	{
		float sizeX = 1f;
		float sizeZ = 1f;
		MeshFilter mf = GetComponent<MeshFilter>();
		if (mf != null && mf.sharedMesh != null)
		{
			Vector3 meshSize = mf.sharedMesh.bounds.size;
			Vector3 lossyScale = transform.lossyScale;
			sizeX = meshSize.x * lossyScale.x;
			sizeZ = meshSize.z * lossyScale.z;
		}

		// Calculate spacing so that X and Z are always the same, and fill the plane
		int countX = Mathf.Max(2, pointCountInLine);
		float spacing = sizeX / (countX - 1);
		int countZ = Mathf.Max(2, Mathf.RoundToInt(sizeZ / spacing) + 1);
		float usedZ = spacing * (countZ - 1);
		Vector3 origin = transform.position - new Vector3(sizeX, 0, usedZ) * 0.5f;

		pointPositions = new Vector3[countX, countZ];
		for (int x = 0; x < countX; x++)
		{
			for (int z = 0; z < countZ; z++)
			{
				Vector3 point = origin + new Vector3(x * spacing, 0, z * spacing);
				pointPositions[x, z] = point;
			}
		}
	}

	void Update()
	{
		var cameraDistance = Mathf.Abs(Camera.transform.position.z - transform.position.z);
		if (cameraDistance <= 0.1f)
		{
			transform.position += new Vector3(0, 0, Length);
			RecalculatePoints();
			SpawnEnemies();
		}
	}

	public void SpawnEnemies()
	{
		var playerLocation = GameObject.FindWithTag("Player").transform;

		for (int x = 0; x < pointPositions.GetLength(0); x += 2)
		{
			for (int z = 2; z < pointPositions.GetLength(1); z++)
			{
				if (x % 2 != 0 || z < 2)
				{
					continue;
				}

				var point = pointPositions[x, z];
				point.y += 1f;
				var obj = PoolingSystem.Instance.SpawnObject(EnemyPrefab, point, Quaternion.identity);
				obj.GetComponent<EnemyController>().PlayerLocation = playerLocation;
			}
		}
	}
}
