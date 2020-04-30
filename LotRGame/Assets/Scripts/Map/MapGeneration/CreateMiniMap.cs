using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMiniMap : MonoBehaviour
{
    //Function called from TileMapManager.cs to make the map texture
    public void CreateMapTexture(string mapNameExtras_ = "")
    {
        //Getting the height and width of the texture based on the size of the map
        int mapWidth = TileMapManager.globalReference.cols;
        int mapHeight = TileMapManager.globalReference.rows;

        //Creating a new map texture using the map width and height
        Texture2D mapTexture = new Texture2D(mapWidth * 2, (mapHeight * 2) + 1, TextureFormat.ARGB32, false);

        //Looping through each column
        for (int c = 0; c < mapWidth; ++c)
        {
            //Looping through each row
            for (int r = 0; r < mapHeight; ++r)
            {
                //Creating a color for the selected pixel
                Color pixelColor;


                //If this tile is one of the city tiles
                if (TileMapManager.globalReference.cityTiles.Contains(TileMapManager.globalReference.tileGrid[c][r]))
                {
                    pixelColor = Color.white;
                }
                //If this tile is one of the dungeon tiles
                else if (TileMapManager.globalReference.dungeonTiles.Contains(TileMapManager.globalReference.tileGrid[c][r]))
                {
                    pixelColor = Color.black;
                }
                //If this tile isn't a city or dungeon tile
                else
                {
                    //Setting the color based on the type of tile we're currently on
                    switch (TileMapManager.globalReference.tileGrid[c][r].type)
                    {
                        case LandType.Ocean:
                            pixelColor = Color.blue;
                            break;

                        case LandType.Grasslands:
                            pixelColor = Color.yellow;
                            break;

                        case LandType.Forest:
                            pixelColor = Color.green;
                            break;

                        case LandType.Desert:
                            pixelColor = new Color(1, 0.8f, 0.55f);
                            break;

                        case LandType.Swamp:
                            pixelColor = new Color(0, 1, 0.58f);
                            break;

                        case LandType.Mountain:
                            pixelColor = Color.grey;
                            break;

                        case LandType.Volcano:
                            pixelColor = Color.red;
                            break;

                        default:
                            pixelColor = Color.white;
                            break;
                    }
                }

                //Altering the pixel color based on the height of the tile
                float tileHeight = TileMapManager.globalReference.tileGrid[c][r].elevation;
                float maxHeight = 70;
                float colorOffset = tileHeight / maxHeight;
                colorOffset = 0.7f + (colorOffset * 0.3f);
                pixelColor = new Color(pixelColor.r * colorOffset,
                                        pixelColor.g * colorOffset,
                                        pixelColor.b * colorOffset, 1);

                //If we're on an even numbered column
                if (c % 2 == 0)
                {
                    //Setting the tile color to the pixel
                    mapTexture.SetPixel(c * 2, (r * 2) + 1, pixelColor);
                    mapTexture.SetPixel((c * 2) + 1, (r * 2) + 1, pixelColor);
                    mapTexture.SetPixel(c * 2, (r * 2) + 2, pixelColor);
                    mapTexture.SetPixel((c * 2) + 1, (r * 2) + 2, pixelColor);
                }
                //If we're on an odd numbered column
                else
                {
                    //Setting the tile color to the pixel
                    mapTexture.SetPixel(c * 2, r * 2, pixelColor);
                    mapTexture.SetPixel((c * 2) + 1, r * 2, pixelColor);
                    mapTexture.SetPixel(c * 2, (r * 2) + 1, pixelColor);
                    mapTexture.SetPixel((c * 2) + 1, (r * 2) + 1, pixelColor);
                }
            }
        }

        //Applying the pixels
        mapTexture.Apply();

        //Encoding the texture to a png
        byte[] bytes = mapTexture.EncodeToPNG();

        string saveFolder = GameData.globalReference.saveFolder;

        //Writing the file to the save folder
        System.IO.File.WriteAllBytes(Application.persistentDataPath + saveFolder + "/Map" + mapNameExtras_ + ".png", bytes);
    }
}
