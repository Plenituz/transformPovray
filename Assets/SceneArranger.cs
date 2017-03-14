using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class SceneArranger : MonoBehaviour {
    public static List<SceneObj> objs = new List<SceneObj>();
    public static List<LightObj> lights = new List<LightObj>();

	void Start () {
        StringBuilder builder = new StringBuilder();
        Camera cam = Camera.main;
        foreach(SceneObj obj in objs)
        {
            if(obj.include)
                builder.Append("#include \"" + obj.name + ".inc\"\n");
        }
        builder.Append("\n");

        builder.Append("camera{\n\tlocation <" + cam.transform.position.x + ", " + cam.transform.position.y + ", " + cam.transform.position.z +
            ">\n\tangle " + cam.fieldOfView*1.5f + "\n\tright x*image_width/image_height\n\tup y\n\t" +
            "rotate <" + cam.transform.eulerAngles.x + ", " +  cam.transform.eulerAngles.y + ", " + cam.transform.eulerAngles.z + ">\n}");
        builder.Append("\n\n");

        foreach(LightObj light in lights)
        {
            builder.Append(GetLightString(light));
            builder.Append("\n\n");
        }
        builder.Append("\n\n");

        foreach (SceneObj obj in objs)
        {
            builder.Append("object{\n\t" + obj.name + "\n\trotate <" + obj.transform.eulerAngles.x + ", " + 
                obj.transform.eulerAngles.y + ", " + obj.transform.eulerAngles.z +
                ">\n\ttranslate <" + obj.transform.position.x + ", " + obj.transform.position.y + ", " + obj.transform.position.z + ">\n}");
            builder.Append("\n\n");
        }

        File.WriteAllText("scene.pov", builder.ToString());
        /*
         camera {
           location <-5, 10, -15>
           angle 58 // direction <0, 0,  1.7>
           right x*image_width/image_height
           up y
           look_at <0,0,0>
        }*/

        /*
        object{
            doorman
            rotate <0, 0, 0>
            translate <0, 0, -5>
        }
        */
        /*
        light_source
        */



    }

    string GetLightString(LightObj lightObj)
    {
        Light l = lightObj.GetComponent<Light>();
        string str = string.Format(
@"light_source{{
    #declare intensity = {3};
    <{0}, {1}, {2}> rgb <intensity, intensity, intensity>
{4}
}}", l.transform.position.x, l.transform.position.y, l.transform.position.z, l.intensity, GetLightParamsString(l));
        //0 1 2 : x y z
        //3 : light intensity
        //4 light type
        //5 light params

        return str;
    }

    private string GetLightParamsString(Light l)
    {
        //spotlight | shadowless | cylinder | parallel
        Vector3 point_at = l.transform.position + l.transform.forward;
        switch (l.type)
        {
            case LightType.Directional:
                return string.Format(
@"    parallel
    point_at <{0}, {1}, {2}>", 
point_at.x, point_at.y, point_at.z);
            case LightType.Point:
                return string.Format(
@"    fade_distance {0}
    fade_power 1", 
l.range);
            case LightType.Spot:
                // radius, falloff, tightness and point_at
                return string.Format(
@"    spotlight
    fade_distance {0}
    fade_power 1
    radius {1}
    falloff 100
    tightness 100
    point_at <{2}, {3}, {4}>",
l.range, l.spotAngle/2f, point_at.x, point_at.y, point_at.z);
                break;
            case LightType.Area:
                Vector3 up = l.transform.up * l.areaSize.y;
                Vector3 right = l.transform.right * l.areaSize.x;
                return string.Format(
@"    #declare resolution = 5;
    area_light <{0}, {1}, {2}>, <{3}, {4}, {5}>, resolution, resolution
    adaptive 1
    jitter", 
up.x, up.y, up.z, right.x, right.y, right.z);
        }
        return "";
    }

    //TODO faire les materials dans le povrayconverter

}
