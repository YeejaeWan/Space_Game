using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float damage;
    public int per; //����

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();    
    }
    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        // ���� ���͸� ����ȭ�Ͽ� ���
        Vector3 normalizedDir = dir.normalized;
        rigid.velocity = normalizedDir * 15f;  // �ӵ� ����
    }


    //test
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -100)
            return;

        per--;

        if(per < 0)
        {   
            //0���� �۾����� �Ѿ��� �����
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area") || per == -100)
            return;

        gameObject.SetActive(false);
    }

}