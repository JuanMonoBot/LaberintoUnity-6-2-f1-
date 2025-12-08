using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Renderer))]
public class URP_AutoTiling : MonoBehaviour
{
    public Vector2 tilingMultiplier = Vector2.one;

    // Para un Cube: si el muro es largo en X y alto en Y, usa XY (lo más común).
    public enum AxisMode { XY, XZ, ZY }
    public AxisMode axes = AxisMode.XY;

    Renderer rend;
    MaterialPropertyBlock mpb;

    void OnEnable() => Apply();
    void OnValidate() => Apply();

    void Update()
    {
        // Actualizamos en estado de runtime
        if (Application.isPlaying) Apply();
    }

    void Apply()
    {
        if (!rend) rend = GetComponent<Renderer>();
        if (mpb == null) mpb = new MaterialPropertyBlock();

        Vector3 s = transform.lossyScale; // Escala global

        Vector2 baseTiling = axes switch
        {
            AxisMode.XY => new Vector2(s.x, s.y),
            AxisMode.XZ => new Vector2(s.x, s.z),
            AxisMode.ZY => new Vector2(s.z, s.y),
            _ => new Vector2(s.x, s.y)
        };
        
        baseTiling = new Vector2(Mathf.Abs(baseTiling.x), Mathf.Abs(baseTiling.y));
        Vector2 tiling = Vector2.Scale(baseTiling, tilingMultiplier);

        // Comprobamos si el material usa _BaseMap_ST o _MainTex_ST
        string st = "_BaseMap_ST";
        var mat = rend.sharedMaterial;
        if (mat != null && !mat.HasProperty(st) && mat.HasProperty("_MainTex_ST"))
            st = "_MainTex_ST";

        rend.GetPropertyBlock(mpb);
        mpb.SetVector(st, new Vector4(tiling.x, tiling.y, 0f, 0f));
        rend.SetPropertyBlock(mpb);
    }
}
