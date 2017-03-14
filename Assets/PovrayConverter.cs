using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEditor;

public class PovrayConverter : MonoBehaviour {
    static List<StringBuilder> all = new List<StringBuilder>();
    static bool hasMaster = false;

    private bool isMaster = false;

	void Awake () {

        if (!hasMaster)
        {
            hasMaster = true;
            isMaster = true;
        }

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vectors = mesh.vertices;
        int[] triangles = mesh.triangles;
        Vector3[] normals = mesh.normals;
        Vector2[] uvs = mesh.uv;
        StringBuilder builder = new StringBuilder();

        //vertex
        builder.Append("object{\n//" + name + "\n");
        builder.Append("mesh2{\n\tvertex_vectors{\n\t\t" + vectors.Length + ",\n\t\t");
        for(int i = 0; i < vectors.Length; i++)
        {
            //vectors[i] = transform.TransformPoint(vectors[i]);
            builder.Append("<" + vectors[i].x + "," + vectors[i].y + "," + vectors[i].z + ">");
            if (i != vectors.Length - 1)
                builder.Append(", ");
            if ((i+1) % 3 == 0 && i != vectors.Length - 1)
                builder.Append("\n\t\t");
        }

        //normals
        builder.Append("\n\t}\n\tnormal_vectors{\n\t\t" + normals.Length + ",\n\t\t");
        for(int i = 0; i < normals.Length; i++)
        {
            builder.Append("<" + normals[i].x + "," + normals[i].y + "," + normals[i].z + ">");
            if (i != vectors.Length - 1)
                builder.Append(", ");
            if ((i + 1) % 3 == 0 && i != vectors.Length - 1)
                builder.Append("\n\t\t");
        }

        //uv
        builder.Append("\n\t}\n\tuv_vectors{\n\t\t" + uvs.Length + ",\n\t\t");
        for(int i = 0; i < uvs.Length; i++)
        {
            builder.Append("<" + uvs[i].x + "," + uvs[i].y + ">");
            if (i != vectors.Length - 1)
                builder.Append(", ");
            if ((i + 1) % 3 == 0 && i != vectors.Length - 1)
                builder.Append("\n\t\t");
        }

        //triangles
        builder.Append("\n\t}\n\tface_indices{\n\t\t" + triangles.Length/3 + ",\n\t\t");
        for(int i = 0; i < triangles.Length; i += 3)
        {
            builder.Append("<" + triangles[i] + "," + triangles[i + 1] + "," + triangles[i + 2] + ">");
            if (i != triangles.Length - 3)
                builder.Append(", ");
            if ((i + 1) % 2 == 0 && i != triangles.Length - 3)
                builder.Append("\n\t\t");
        }
        builder.Append("\n\t}\n//" + name);//material goes here
        builder.Append(string.Format(@"
    material{{
        texture{{
            pigment{{
                uv_mapping image_map{{tga {0} }}
            }}
        }}
    }}", "\"" + GetComponent<MeshRenderer>().material.mainTexture.name + ".tga\""));
        builder.Append("\n}");

        /*
        material{
	        texture{
	            pigment{
	                uv_mapping image_map{tga "t_doorMan_d.tga"}
	            }
	        }
	    }
    */
        builder.Append("\n}");//close "object" tag

        all.Add(builder);
	}

    void Start()
    {
        if (!isMaster)
            return;
        StringBuilder main = new StringBuilder();
        main.Append("#declare " + transform.root.name + " = union{\n");
        for(int i = 0; i < all.Count; i++)
        {
            main.Append(all[i].ToString());
            if(i != all.Count-1)
                main.Append("\n\n");
        }
        main.Append("}");
        File.WriteAllText(transform.root.name + ".inc", main.ToString());
    }
}
