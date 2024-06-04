using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count; //���� ����
    public float speed;

    float timer;
    Player player;

    void Awake()
    {
        player = GameManager.instance.player; 
    }


    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        if (id == 0) // Į�� ���
        {
            // Į�� �ð� �������� ȸ��
            transform.Rotate(Vector3.back * speed * Time.deltaTime);
        }
        else
        {
            timer += Time.deltaTime;
            if (timer > speed)
            {
                timer = 0f;
                Fire();
            }
        }
    }

    void Batch()
    {
        for (int index = 0; index < count; index++)
        {
            Transform bullet;

            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);

            if (id != 0) // Į�� �ƴ� ��쿡�� Rigidbody2D ����
            {
                bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero);
            }
        }
    }


    // ������
    public void LevelUp(float damage, int count)
    {
        this.damage = damage * Character.Damage;
        this.count += count; // ������ ������ �߰�

        if (id == 0)
            Batch();

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    //�ʱ�ȭ
    public void Init(ItemData data)
    {
        //�⺻ ����
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        //Property Set
        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        for(int index = 0; index < GameManager.instance.pool.prefabs.Length; index++)
        {
            if(data.projectile == GameManager.instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            } 
        }

        switch (id)
        {
            case 0:
                speed = 150 *Character.WeaponSpeed; // �ð����
                Batch();

                break;
            default:
                speed = 0.4f *Character.WeaponRate;
                break;
        }

        //Hand Set
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);
    }

   

    void Fire()
    {
        List<Transform> targets = player.scanner.GetNearestTargets(count);  // ���� Ÿ���� ������

        foreach (var target in targets)
        {
            if (target != null)
            {
                Vector3 targetPos = target.position;
                Vector3 dir = (targetPos - transform.position).normalized;
                Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.position = transform.position;  // �÷��̾� ��ġ���� �߻�
                bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);  // ȸ��
                bullet.GetComponent<Bullet>().Init(damage, -1, dir);  // ����� ����, ���� ���������� ���� ����

                AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
            }
        }
    }

}
