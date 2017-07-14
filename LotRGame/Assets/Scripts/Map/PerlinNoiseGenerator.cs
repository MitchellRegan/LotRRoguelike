using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseGenerator : MonoBehaviour
{
    //Gradient used to color tiles based on their height
    public Gradient colorGradient;

    //The how zoomed in on the perlin noise we are. Lower numbers = more zoomed in, higher = more zoomed out
    [Range(0.1f, 10f)]
    public float perlinZoomX = 2;
    [Range(0.1f, 10f)]
    public float perlinZoomY = 2;



    //Assigns an elevation map to each tile using perlin noise
    private void GeneratePerlinMapElevation(List<List<GameObject>> tileGrid_, Vector2 heightMinMax_, float offset_ = 0, bool colorize_ = false)
    {
        //Getting the total number of rows and columns
        int rows = tileGrid_[0].Count;
        int cols = tileGrid_.Count;

        //A random offset so that games aren't the same (unless the offset is input)
        float offset = offset_;
        if(offset_ == 0)
        {
            offset = Random.value * 100000f;
        }

        //Creating a new color for the sprites using perlin noise
        Color tileColor;

        //Looping through each column of tiles
        for (int c = 0; c < cols; ++c)
        {
            //Looping through each row in the current column
            for (int r = 0; r < rows; ++r)
            {
                //Checking to make sure that the current tile isn't null (adding extra tiles can do that)
                if (tileGrid_[c][r] != null)
                {
                    //Making sure that the current tile has a sprite
                    if (tileGrid_[c][r].GetComponent<SpriteRenderer>() != null)
                    {
                        //Setting the tile's color to a value on the gradient based on perlin noise
                        float xRand = (this.perlinZoomX * (c * 1.0f) / rows) + offset;
                        float yRand = (this.perlinZoomY * (r * 1.0f) / cols) + offset;
                        float perlin = Mathf.PerlinNoise(xRand, yRand);

                        //Setting the elevation of the tile based on the height min/max
                        float elevation = ((heightMinMax_.y - heightMinMax_.x) * perlin) + heightMinMax_.x;
                        tileGrid_[c][r].transform.position += new Vector3(0, elevation, 0);

                        //If we colorize the map using the color gradient
                        if(colorize_)
                        {
                            tileColor = this.colorGradient.Evaluate(perlin);
                            tileGrid_[c][r].GetComponent<SpriteRenderer>().color = tileColor;
                        }
                    }
                }
            }
        }
    }
}
