using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour
{
    [SerializeField] Water m_water = null;
    [SerializeField] [Range(0.0f, 5.0f)] float m_dropRate = 0.1f;
    [SerializeField] [Range(0.0f, 5.0f)] float m_dropStrengthMin = 1.0f;
    [SerializeField] [Range(0.0f, 5.0f)] float m_dropStrengthMax = 2.0f;

    float m_dropTime = 0.0f;

    private void Start()
    {
        m_dropTime = 0.0f;
    }

    private void Update()
    {
        m_dropTime += Time.deltaTime;
        if (m_dropTime >= m_dropRate)
        {
            m_dropTime = 0.0f;
            RainDrop();
        }
    }

    private void RainDrop()
    {
        float x = Random.Range(0.0f, m_water.MeshWidth);
        float y = Random.Range(0.0f, m_water.MeshHeight);
        Vector3 offset = new Vector3(-m_water.MeshWidth / 2.0f + x, 10.0f, -m_water.MeshHeight / 2.0f + y);
        Vector3 origin = m_water.transform.position + offset;
        Ray ray = new Ray(origin, Vector3.down);
        float strength = Random.Range(m_dropStrengthMin, m_dropStrengthMax);
        m_water.Touch(ray, strength);
    }
}
