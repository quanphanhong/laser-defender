using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPDisplay : MonoBehaviour
{
    [SerializeField] float maxHP;
    [SerializeField] bool isEnemy = false;
    float currentHP;

    SpriteRenderer remainingHPRenderer;

    private void Start()
    {
        remainingHPRenderer = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();

        currentHP = maxHP;
        UpdateHPBarColor(currentHP / maxHP);
    }

    public void SetMaxHP(float maxHP) {this.maxHP = maxHP; }

    public void UpdateHPBar(float hp)
    {
        currentHP = hp;
        float portion = Mathf.Clamp(currentHP / maxHP, 0f, 1f);

        transform.localScale = new Vector3(portion, transform.localScale.y, transform.localScale.z);
        UpdateHPBarColor(portion);
    }

    private void UpdateHPBarColor(float portion)
    {
        if (portion < 0.5f || isEnemy) ChangeHPBarColor(Color.red);
        else ChangeHPBarColor(Color.green);
    }

    private void ChangeHPBarColor(Color color)
    {
        remainingHPRenderer.color = color;
    }
}
