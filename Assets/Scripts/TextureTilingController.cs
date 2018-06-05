using System.Collections;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class TextureTilingController : MonoBehaviour
{

	// Give us the texture so that we can scale proportianally the width according to the height variable below
	// We will grab it from the meshRenderer
	public Material mat;
	public float textureToMeshZ = 2f; // Use this to contrain texture to a certain size

	Vector3 prevScale = Vector3.one;
	float prevTextureToMeshZ = -1f;

	// Use this for initialization
	void Start()
	{
		this.prevScale = gameObject.transform.lossyScale;
		this.prevTextureToMeshZ = this.textureToMeshZ;

		this.UpdateTiling();
	}

	// Update is called once per frame
	void Update()
	{
		// If something has changed
		if (gameObject.transform.lossyScale != prevScale || !Mathf.Approximately(this.textureToMeshZ, prevTextureToMeshZ))
			this.UpdateTiling();

		// Maintain previous state variables
		this.prevScale = gameObject.transform.lossyScale;
		this.prevTextureToMeshZ = this.textureToMeshZ;
	}

	[ContextMenu("UpdateTiling")]
	void UpdateTiling()
	{
		// A Unity plane is 10 units x 10 units
		float planeSizeX = 10f;
		float planeSizeZ = 10f;

		// Figure out texture-to-mesh width based on user set texture-to-mesh height
		Texture tex = this.mat.mainTexture;
		float textureToMeshX = ((float) tex.width / tex.height) * this.textureToMeshZ;

		Vector2 scal = new Vector2(planeSizeX * gameObject.transform.lossyScale.x / textureToMeshX, planeSizeZ * gameObject.transform.lossyScale.z / textureToMeshZ);
		Renderer render = gameObject.GetComponent<Renderer>();

		if (render)
		{
			foreach (Material mat in render.materials)
			{
				Shader shader = mat.shader;
				for (int i = 0; i < ShaderUtil.GetPropertyCount(shader); i++)
				{
					if (ShaderUtil.GetPropertyType(shader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
					{
						mat.SetTextureScale(ShaderUtil.GetPropertyName(shader, i), scal);
					}
				}
			}
		}
	}
}