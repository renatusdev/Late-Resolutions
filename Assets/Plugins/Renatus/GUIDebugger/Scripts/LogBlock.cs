using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogBlock : MonoBehaviour
{
    private readonly static float s_DEFAULT_FADEOUT_TIME = 3;

    [SerializeField] private Image m_Background;
    [SerializeField] private Text m_Text;

    Color m_OriginalBackgroundColor;
    Color m_OriginalTextColor;
    Pool m_LogBlocksPool;

    public void Initialize(string text, Pool pool) {
        m_OriginalBackgroundColor = m_Background.color;
        m_OriginalTextColor = m_Text.color;

        m_LogBlocksPool = pool;
        m_Text.text = text;
        StartCoroutine(CoFadeOut());
    }

    private IEnumerator CoFadeOut() {
        float t = 0;
        float duration = s_DEFAULT_FADEOUT_TIME;

        float originalTextAlpha = m_Text.color.a;
        float originalBackgroundAlpha = m_Background.color.a;

        while(t < duration) {
            float textAlpha = Mathf.Lerp(originalTextAlpha, 0, t/duration);
            float backgroundAlpha = Mathf.Lerp(originalBackgroundAlpha, 0, t/duration);

            m_Text.color = new Color(m_Text.color.r, m_Text.color.g, m_Text.color.b, textAlpha);
            m_Background.color = new Color(m_Background.color.r, m_Background.color.g, m_Background.color.b, backgroundAlpha);

            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Reset();
        m_LogBlocksPool.Enqueue(this.gameObject);
    }

    private void Reset() {
        m_Background.color = m_OriginalBackgroundColor;
        m_Text.color = m_OriginalTextColor;
    }
}
