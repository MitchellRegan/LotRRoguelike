using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNoiseGenerator : MonoBehaviour
{
    public int width = 256;
    public int height = 256;

    //Texture representation of map
    private Texture2D texture;



	// Use this for initialization
	private void OnEnable()
    {
        //Making sure the plane is scaled correctly
        this.SetScale();

        //Setting our texture as this object's main texture
        this.texture = new Texture2D(this.width, this.height, TextureFormat.RGB24, true);
        this.texture.name = "Procedural Texture";
        this.GetComponent<MeshRenderer>().material.mainTexture = this.texture;
        this.FillTexture();


    }



    private void SetScale()
    {
        if(this.width > this.height)
        {
            this.transform.localScale = new Vector3(1, ( (1.0f * this.height) / (1.0f * this.width) ), 1);
        }
        else if(this.height > this.width)
        {
            this.transform.localScale = new Vector3(( (1.0f *this.width) / (1.0f * this.height) ), 1, 1);
        }
    }



    public void FillTexture()
    {
        float widthStep = 1f / this.width;
        float heightStep = 1f / this.height;

        //Looping through the width and height
        for(int w = 0; w < this.width; ++w)
        {
            for(int h = 0; h < this.height; ++h)
            {
                Color gradientColor = new Color(w * widthStep, h * heightStep, 0);
                //Setting each tile (pixel) as ocean blue
                this.texture.SetPixel(w, h, gradientColor);
            }
        }

        //Applying the texture change
        this.texture.Apply();
    }
}
