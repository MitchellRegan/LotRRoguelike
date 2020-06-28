using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHighlight : MonoBehaviour
{
    public Color defaultColor = Color.red;

    public SpriteRenderer tileSprite;
    public ParticleSystem tileParticle;


    // Start is called before the first frame update
    void Start()
    {
        this.SetHighlightColor(this.defaultColor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetHighlightColor(Color newColor_)
    {
        this.tileSprite.color = newColor_;
        this.tileParticle.startColor = newColor_;
    }
}
