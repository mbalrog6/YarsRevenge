using UnityEngine;

public class IonZone : MonoBehaviour
{
    private float _width;
    private float _height;

    public float Width
    {
        get => _width;
        set { _width = value; SetRectContainer(); }
    }

    public float Height
    {
        get => _height;
        set { _height = value; SetRectContainer(); }
    }

    public RectContainer IonRectContainer => _rectContainer;

    private RectContainer _rectContainer;

    private void Start()
    {
        _height = ScreenHelper.Instance.ScreenBounds.height;
        _width = 3f;
        _rectContainer = new RectContainer(this.gameObject, 0, 0, _width, _height);
        Vector3 position = new Vector3(
            ScreenHelper.Instance.ScreenBounds.xMin + (ScreenHelper.Instance.ScreenBounds.width / 3),
            ScreenHelper.Instance.ScreenBounds.center.y,
            0f);
        transform.position = position; 
        VisualizeIonField();
    }

    private void Update()
    {
        _rectContainer.UpdateToTargetPosition();
    }

    private void SetRectContainer()
    {
        _rectContainer.SetRectContainer(0, 0, _width, _height);
    }

    private void VisualizeIonField()
    {
        var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.name = "IonZone Visual";
        quad.transform.localScale = new Vector3(  3f, 16f, 1f );
        var qMaterial = quad.GetComponent<MeshRenderer>();
        
        var material = new Material(Shader.Find("Standard"));
        material.SetFloat("_Mode", 2);
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;
        
        material.SetColor( "_Color", new Color(.1f, .1f, .1f, .01f) );
        qMaterial.material = material;
        quad.transform.parent = this.gameObject.transform; 
        quad.transform.localPosition = new Vector3(0f, 0f, 1f);
    }
    
    private void OnDrawGizmos()
    {
        if (_rectContainer == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(_rectContainer.Bounds.xMin, _rectContainer.Bounds.yMin, 0f),
            new Vector3(_rectContainer.Bounds.xMin, _rectContainer.Bounds.yMax, 0f));
        Gizmos.DrawLine(new Vector3(_rectContainer.Bounds.xMax, _rectContainer.Bounds.yMin, 0f),
            new Vector3(_rectContainer.Bounds.xMax, _rectContainer.Bounds.yMax, 0f));
        Gizmos.DrawLine(new Vector3(_rectContainer.Bounds.xMin, _rectContainer.Bounds.yMin, 0f),
            new Vector3(_rectContainer.Bounds.xMax, _rectContainer.Bounds.yMin, 0f));
        Gizmos.DrawLine(new Vector3(_rectContainer.Bounds.xMin, _rectContainer.Bounds.yMax, 0f),
            new Vector3(_rectContainer.Bounds.xMax, _rectContainer.Bounds.yMax, 0f));
    }
}
