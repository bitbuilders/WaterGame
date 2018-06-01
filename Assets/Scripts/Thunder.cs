using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    [SerializeField] [Range(1.0f, 30.0f)] float m_rate = 20.0f;
    [SerializeField] [Range(1.0f, 30.0f)] float m_rateOffset = 5.0f;
    [SerializeField] [Range(0.0f, 1.0f)] float m_pitchRange = 0.5f;
    [SerializeField] Light m_light = null;
    [SerializeField] Water m_water = null;

    AudioSource m_audioSource;
    float m_actualRate = 0.0f;
    float m_time = 0.0f;

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_actualRate = 10.0f;
        m_light.enabled = false;
    }

    private void Update()
    {
        m_time += Time.deltaTime;
        if (m_time >= m_actualRate)
        {
            m_time = 0.0f;
            m_actualRate = Random.Range(m_rate - m_rateOffset, m_rate + m_rateOffset);
            BOOM();
        }
    }

    private void BOOM()
    {
        m_audioSource.pitch = Random.Range(1.0f - m_pitchRange, 1.0f + m_pitchRange);
        m_audioSource.Play();
        StartCoroutine(FlashLight(2.0f, 1.0f));
        Ray ray = new Ray(Vector3.up * 5.0f, Vector3.down);
        m_water.Touch(ray, 100.0f);
    }

    private IEnumerator FlashLight(float fadeSpeed, float duration)
    {
        yield return new WaitForSeconds(duration);
        m_light.enabled = true;
        float intensity = m_light.intensity;

        for (float i = 1.0f; i >= 0.0f; i -= Time.deltaTime * fadeSpeed)
        {
            m_light.intensity = intensity * i;
            yield return null;
        }
        m_light.intensity = intensity;
        m_light.enabled = false;
    }
}
