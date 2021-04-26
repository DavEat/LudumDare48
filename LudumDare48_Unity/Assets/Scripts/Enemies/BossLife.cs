using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLife : MonoBehaviour
{
    [SerializeField] float life = 100;
    [SerializeField] float maxLife = 100;

    [SerializeField] GameObject[] m_lifeSteps = null;
    [SerializeField] float[] m_lifeStepsPercent = { .33f, .66f, 1 };
    int currentLifeStep = 0;

    [SerializeField] LifeBar m_lifeBar = null;

    [SerializeField] Material m_material;
    [SerializeField] MeshRenderer[] m_meshRenderers;
    string propertyName = "_Emmission";
    public Vector3 position { get { return m_transform.position; } }

    Transform m_transform = null;
    EnemySpawner m_spawner = null;

    Coroutine m_coroutine = null;

    void Start()
    {
        m_transform = GetComponent<Transform>();
        m_spawner = GetComponent<EnemySpawner>();

        GameManager.inst.nextLevel += ResetObj;
        ResetObj();

        m_material = new Material(m_material);
        foreach (MeshRenderer renderer in m_meshRenderers)
        {
            renderer.sharedMaterial = m_material;
        }
    }

    public void SetDamage(float damage)
    {
        life -= damage;

        if (!m_spawner.asBeenAttack)
            m_spawner.asBeenAttack = true;

        float percent = life / maxLife;

        if (currentLifeStep < m_lifeStepsPercent.Length && m_lifeStepsPercent[currentLifeStep] <=  1 - percent)
        {
            m_lifeSteps[currentLifeStep].SetActive(false);
            m_lifeSteps[++currentLifeStep].SetActive(true);
        }

        m_lifeBar.UpdateLife(percent * .66f);

        if (life <= 0)
        {
            EnemyManager.inst.deathBoss.Add(this);
            Debug.Log("Horra Boss is Death");

            m_spawner.enabled = false;
            m_spawner.GetComponent<Collider>().enabled = false;
        }
        else m_coroutine = StartCoroutine(MaterialDelay(.5f, 2));
    }

    IEnumerator MaterialDelay(float delay, int step)
    {
        m_material.SetFloat(propertyName, step);
        yield return new WaitForSeconds(delay);
        m_material.SetFloat(propertyName, 1);
    }

    public void ResetObj()
    {
        if (GameManager.inst.frontSpawner != m_spawner.frontSpawner)
            return;

        EnemyManager.inst.boss.Add(this);

        m_transform = GetComponent<Transform>();
        m_spawner = GetComponent<EnemySpawner>();

        life = maxLife;

        currentLifeStep = 0;

        m_lifeSteps[0].SetActive(true);
        for (int i = 1; i < m_lifeSteps.Length; i++)
            m_lifeSteps[i].SetActive(false);

        m_spawner.GetComponent<Collider>().enabled = true;
        m_spawner.asBeenAttack = false;
        m_spawner.enabled = true;

        if (m_coroutine != null)
            StopCoroutine(m_coroutine);

        m_lifeBar.UpdateLife(.66f);

        m_material.SetFloat(propertyName, 0);
    }
}