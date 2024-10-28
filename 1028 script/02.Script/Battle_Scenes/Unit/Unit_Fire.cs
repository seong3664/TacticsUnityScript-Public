using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Unit_Fire : MonoBehaviour
{
    LineRenderer lineRenderer;
    float F_Time;
    public Transform Firepos;
    public ParticleSystem MuzleFire;
    Light Light;
    Color startColor;
    Quaternion StartRot;
    
    AimCheck aimCheck;
    int HitChance;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        startColor = lineRenderer.material.color;
        StartRot = Firepos.rotation;
        Firepos = transform.GetChild(1).GetComponent<Transform>();
        //MuzleFire = transform.GetChild(1).GetComponent<ParticleSystem>();
        aimCheck = GameObject.Find("Enemy_Manager").GetComponent<AimCheck>();
        Light = MuzleFire.gameObject.transform.GetChild(1).GetComponent<Light>();
    }
    private void Start()
    {

        MuzleFire.Stop();
        Light.enabled = false;
    }
    private void Update()
    {
        Debug.DrawRay(Firepos.position, Firepos.forward * 30f, Color.red, 1f);
    }
    public void FireRay( Transform Target, int Damege)
    {

        RaycastHit hit;
        int aiming = transform.GetComponent<UnitStat>().stat.Aiming;
        Vector3 direction = Target.position - transform.position;
        direction.y = 0; // Y�� ȸ���� �����ϹǷ� Y ���� 0���� ����

        // Ÿ�� ���������� Y�� ȸ���� ���
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // ���� ������ ȸ������ Y�ุ Ÿ�� �������� ����
        transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);

        Ray ray = new Ray(Firepos.position, (Target.position + Vector3.up * 1.5273f - Firepos.position).normalized);
        UnitStat stat = transform.GetComponent<UnitStat>();
        stat.stat.Action -= 2; //�߻�ÿ� �ൿ�� 2 ����.�̵�->������ ���������� ����->�̵��� �Ұ��� �ϵ���
        stat.BulletCount -= 1;
        StartCoroutine(MuzleFireOn());
        if (Physics.Raycast(ray, out hit, 30f))      //����ĳ��Ʈ �߻�
        {
            lineRenderer.SetPosition(0, Firepos.position);  //���� ������ Ű��
            HitChance = aimCheck.HitChanceCheckTransform(transform, Target);
            OnDamege target = hit.collider.GetComponent<OnDamege>();
            if (Random.Range(0, 101) < HitChance)
            {
                if (target != null)
                    target.OnDamege(Damege, hit.point, hit.normal);
                Debug.Log(hit.normal.ToString());
                lineRenderer.SetPosition(1, hit.point);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y + 5f, 0);
                lineRenderer.SetPosition(1, Firepos.forward * 30f);
                if (target != null)
                    target.OnMissed();
            }
            Debug.DrawRay(Firepos.position, Firepos.forward, Color.red, 100f);

            StartCoroutine(Firing());

           
           

        }
    }
    
    IEnumerator MuzleFireOn()
    {
        MuzleFire.Play();
        Light.enabled = true;
        yield return new WaitForSeconds(0.1f);
        MuzleFire.Stop();
        Light.enabled = false;
    }
    IEnumerator Firing()
    {
        F_Time = 0f;
        lineRenderer.enabled = true;
        while (F_Time <= 0.6f)
        {
            F_Time += Time.deltaTime;
            float alpha = Mathf.Lerp(0.6f, 0.0f, F_Time / 0.6f);
            yield return null;
            // ���η������� ���� ���� (���İ��� ����)
            Color newColor = new Color(startColor.r, startColor.g, startColor.b, alpha);
            lineRenderer.material.color = newColor;
        }
        lineRenderer.enabled = false;

    }
    
}
