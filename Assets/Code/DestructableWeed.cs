using UnityEngine;
using UnityEngine.U2D;

public class DestructableWeed : DestructableBase
{
    public override void OnDestruction()
    {
        SpriteShapeRenderer spriteShapeRenderer = GetComponent<SpriteShapeRenderer>();
        if (spriteShapeRenderer != null)
        { spriteShapeRenderer.enabled = false; }

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
    }
}
