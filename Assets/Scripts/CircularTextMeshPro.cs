using TMPro;
using UnityEngine;

/// <summary>
/// Curves TextMeshProUGUI text along a circular arc.
/// Uses only Unity and TextMeshPro; Odin Inspector is not required.
/// </summary>
[ExecuteAlways]
[DisallowMultipleComponent]
[RequireComponent(typeof(TextMeshProUGUI))]
public class CircularTextMeshPro : MonoBehaviour
{
    private const float MinimumLineRadius = 0.001f;

    [SerializeField]
    [Tooltip("The radius of the text's circular arc.")]
    private float m_radius = 1.0f;

    private TextMeshProUGUI m_TextComponent;

    /// <summary>
    /// Gets or sets the radius of the text arc.
    /// </summary>
    public float Radius
    {
        get => m_radius;
        set
        {
            if (Mathf.Approximately(m_radius, value))
                return;

            m_radius = value;
            RefreshText();
        }
    }

    private void Awake()
    {
        CacheTextComponent();
    }

    private void OnEnable()
    {
        CacheTextComponent();

        if (m_TextComponent == null)
            return;

        // Remove first to prevent accidental duplicate subscriptions.
        m_TextComponent.OnPreRenderText -= UpdateTextCurve;
        m_TextComponent.OnPreRenderText += UpdateTextCurve;

        RefreshText();
    }

    private void OnDisable()
    {
        if (m_TextComponent != null)
        {
            m_TextComponent.OnPreRenderText -= UpdateTextCurve;
        }
    }

    private void OnValidate()
    {
        CacheTextComponent();

        // OnValidate can be called during sensitive editor operations.
        // Marking the vertices dirty lets Unity rebuild them safely.
        if (m_TextComponent != null && isActiveAndEnabled)
        {
            m_TextComponent.SetVerticesDirty();
        }
    }

    private void CacheTextComponent()
    {
        if (m_TextComponent == null)
        {
            m_TextComponent = GetComponent<TextMeshProUGUI>();
        }
    }

    private void RefreshText()
    {
        CacheTextComponent();

        if (m_TextComponent == null || !isActiveAndEnabled)
            return;

        m_TextComponent.ForceMeshUpdate();
    }

    private void UpdateTextCurve(TMP_TextInfo textInfo)
    {
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo characterInfo = textInfo.characterInfo[i];

            if (!characterInfo.isVisible)
                continue;

            int vertexIndex = characterInfo.vertexIndex;
            int materialIndex = characterInfo.materialReferenceIndex;

            Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

            Vector3 characterMidBaselinePosition = new Vector3(
                (vertices[vertexIndex].x + vertices[vertexIndex + 2].x) * 0.5f,
                characterInfo.baseLine,
                0.0f
            );

            // Move the character so its midpoint is at the origin.
            vertices[vertexIndex] -= characterMidBaselinePosition;
            vertices[vertexIndex + 1] -= characterMidBaselinePosition;
            vertices[vertexIndex + 2] -= characterMidBaselinePosition;
            vertices[vertexIndex + 3] -= characterMidBaselinePosition;

            Matrix4x4 transformationMatrix = ComputeTransformationMatrix(
                characterMidBaselinePosition,
                textInfo,
                i
            );

            vertices[vertexIndex] =
                transformationMatrix.MultiplyPoint3x4(vertices[vertexIndex]);

            vertices[vertexIndex + 1] =
                transformationMatrix.MultiplyPoint3x4(vertices[vertexIndex + 1]);

            vertices[vertexIndex + 2] =
                transformationMatrix.MultiplyPoint3x4(vertices[vertexIndex + 2]);

            vertices[vertexIndex + 3] =
                transformationMatrix.MultiplyPoint3x4(vertices[vertexIndex + 3]);
        }
    }

    private Matrix4x4 ComputeTransformationMatrix(
        Vector3 characterMidBaselinePosition,
        TMP_TextInfo textInfo,
        int characterIndex)
    {
        TMP_CharacterInfo characterInfo =
            textInfo.characterInfo[characterIndex];

        float lineBaseline =
            textInfo.lineInfo[characterInfo.lineNumber].baseline;

        float radiusForThisLine = m_radius + lineBaseline;

        // Prevent division by zero while still allowing negative radii.
        if (Mathf.Abs(radiusForThisLine) < MinimumLineRadius)
        {
            radiusForThisLine =
                radiusForThisLine < 0.0f
                    ? -MinimumLineRadius
                    : MinimumLineRadius;
        }

        float circumference = 2.0f * Mathf.PI * radiusForThisLine;

        float angle =
            ((characterMidBaselinePosition.x / circumference - 0.5f) * 360.0f
             + 90.0f)
            * Mathf.Deg2Rad;

        float cosine = Mathf.Cos(angle);
        float sine = Mathf.Sin(angle);

        Vector3 newMidBaselinePosition = new Vector3(
            cosine * radiusForThisLine,
            -sine * radiusForThisLine,
            0.0f
        );

        float rotationAngle =
            -Mathf.Atan2(sine, cosine) * Mathf.Rad2Deg - 90.0f;

        return Matrix4x4.TRS(
            newMidBaselinePosition,
            Quaternion.AngleAxis(rotationAngle, Vector3.forward),
            Vector3.one
        );
    }
}