using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.AccessControl;
#if UNITY_EDITOR
using UnityEditor.UI;
#endif
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[Serializable]
public class Unit : MonoBehaviour
{
    public static List<Unit> Units { private set; get; } = new List<Unit>();

    [Header("Статус")]
    public bool Paused = false;
    public bool Commandable = true;
    public bool Movable = true;
    public bool Invulnerable = false;
    public bool IsDead = false;
    public bool Stunned = false;
    public bool Silenced = false;
    public bool CanJump;
    public bool CanSlide;
    [Space]
    public bool OnGround;
    public bool OnWall;
    public byte SlidesCountMax = 2;
    private byte SlidesCount = 0;
    
    [Header("Характеристики")]
    public byte Health;
    public byte MaxHealth;
    public byte Mana;
    public byte MaxMana;

    public Unit Target;

    public enum UnitType { MELEE, RANGE }

    [Header("Бой")]
    public UnitType Type;
    public byte AttackDamage;
    public float AttackRange;
    [Range(0, 10)]
    public float AttackCooldown;
    public bool IsReadyToAttack = true;

    [Header("Передвижение")]
    [Range(0, 5)]
    public float MovementSpeed;

    private byte DoubleJumpsCount = 0;
    [Header("Прыжок")] 
    public byte DoubleJumpsMax = 0;
    public float JumpScale = 4f;

    [Header("Аниматор для проигрывания анимаций")]
    public Animator Animator;

    [Header("Коллайдер для физического взаимодействия")]
    public BoxCollider2D RigidbodyCollider;

    public List<Order> Queue = new List<Order>();
    public Order CurrentOrder;


    public bool AbilitiesFoldout;
    public float FoldoutHeight = 100;
    public List<Ability> Abilities = new List<Ability>();

    private Color DefaultColor;

    public Vector3 SightVector;

    private bool IsFalling = false;

    public void LateUpdate()
    {
        if (Queue.Count > 0)
        {
            if (Queue[0].State != global::Order.OrderState.EXECUTED)
            {
                if (Queue[0].State != global::Order.OrderState.PROCESSING)
                {
                    CurrentOrder = Queue[0];
                    Queue[0].Execute();
                }
            }
            else if (Queue[0].State == global::Order.OrderState.EXECUTED)
            {
                Queue.Remove(Queue[0]);
                CurrentOrder = null;
            }
        }

        if (Animator != null && !OnWall)
        {
            Animator.SetBool("On Ground", OnGround);
            if (!OnGround)
            {
                if (GetComponent<Rigidbody2D>().velocity.y < -2f)
                {
                    Animator.ResetTrigger("Hover");
                    Animator.SetTrigger("Fall");
                }
                else if (GetComponent<Rigidbody2D>().velocity.y > 0)
                {
                    Animator.ResetTrigger("Fall");
                }
                else if (GetComponent<Rigidbody2D>().velocity.y <= 0 && GetComponent<Rigidbody2D>().velocity.y >= -2f)
                {
                    Animator.ResetTrigger("Jump");
                    Animator.SetTrigger("Hover");
                }
            }
        }

        if (GetComponent<Rigidbody2D>().velocity.y < 0 && !OnGround)
        {
            if (!IsFalling) StartCoroutine(FallCoroutine());
        }
    }
    private IEnumerator FallCoroutine()
    {
        IsFalling = true;
        float speed = 0;
        while (!OnGround)
        {
            speed = Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y);
            yield return null;
        }

        if (speed > 13)
        {
            Damage(this, (byte)(speed * 2.5f));
        }

        //Sound.Play(Sound.PlayerSoundType.LANDING);
        IsFalling = false;
        StopCoroutine(FallCoroutine());
    }

    public void Start()
    {
        Units.Add(this);
        DefaultColor = GetComponent<SpriteRenderer>().material.color;
        if (Animator != null) Idle();
    }

    /*========== ПРИКАЗЫ ==========*/
    public void MoveTo(Vector2 _direction)
    {
        if (Movable && Commandable && !IsDead)
        {
            CheckFlip(_direction);            
            StopCoroutine(Move(_direction));
            if (Animator != null)
            {
                Idle();
                Animator.SetBool("Idle", false);
                Animator.SetBool("Walk", true);
            }
            StartCoroutine(Move(_direction));
        }
    } // Ходьба к указанной точке со скоростью MovementSpeed
    private IEnumerator Move(Vector2 _direction)
    {
        while (transform.position != new Vector3(_direction.x, _direction.y, transform.position.z))
        {
            if (Mathf.Abs(_direction.x - transform.position.x) >= (new Vector3(_direction.x - transform.position.x, 0, 0).normalized * MovementSpeed * Time.deltaTime).magnitude)
                transform.position += new Vector3(_direction.x - transform.position.x, 0, 0).normalized * MovementSpeed * Time.deltaTime;
            else
            {
                transform.position = _direction;
                break;
            }
            yield return null;
        }
        Idle();
        if (CurrentOrder.Name != "Патрулирование") CurrentOrder.Complete();
        StopCoroutine(Move(_direction));
    }
    public void MoveTo(Unit _target)
    {
        if (Movable && Commandable && !IsDead)
        {
            StopCoroutine(Move(_target));
            if (Animator != null)
            {
                Idle();
                Animator.SetBool("Idle", false);
                Animator.SetBool("Walk", true);
            }
            StartCoroutine(Move(_target));
        }
    } // Преследование указанной цели со скоростью MovementSpeed
    private IEnumerator Move(Unit _target)
    {
        float distance = Vector3.Distance(transform.position, _target.transform.position);
        while (Vector3.Distance(transform.position, _target.transform.position) >= GetPhysicRadius(this, _target))
        {
            distance = Vector3.Distance(transform.position, _target.transform.position);
            CheckFlip(_target);
            transform.position = Vector3.Lerp(transform.position, _target.transform.position, MovementSpeed / distance * Time.deltaTime);
            yield return null;
        }
        Idle();
        CurrentOrder.Complete();
        StopCoroutine(Move(_target));
    }

    public void Attack(Unit _target)
    {
        if (Commandable && !IsDead)
        {
            Target = _target;
            Queue.Clear();
            StartCoroutine(AttackCoroutine());
        }
    } // Атака цели
    private IEnumerator AttackCoroutine()
    {
        while (Target != null && !Target.IsDead && !IsDead)
        {
            if (Vector3.Distance(transform.position, Target.transform.position) <= GetAttackRadius(this, Target))
            {
                if (Animator != null)
                {
                    CheckFlip(Target);
                    Animator.SetBool("Walk", false);
                    Animator.SetBool("Idle", false);
                    Animator.SetBool("Attack", true);
                }
                yield return new WaitForSeconds(AttackCooldown);
            }            
            else if (Vector3.Distance(transform.position, Target.transform.position) > GetAttackRadius(this, Target))
            {
                bool flag = false;
                foreach (Order order in Queue)
                {
                    if (order == new Order(method => MoveTo(Target), "Следовать за " + Target.name))
                    {
                        flag = true;
                    }
                }
                if (!flag) Queue.Add(new Order(method => MoveTo(Target), "Следовать за " + Target.name));                
                yield return null;
            }
        }
        Idle();
        if (Queue.Count > 0) CurrentOrder.Complete();

        if (Target.IsDead) GetComponent<Enemy>().Patrol();
        Target = null;
    }
    /*==============================*/

            /*========== ДЕЙСТВИЯ ==========*/
    public void Jump()
    {
        if (CanJump && Commandable)
        {
            if (OnGround)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.up * JumpScale;
                OnGround = false;
                if (Animator != null)
                {
                    Animator.SetTrigger("Jump");
                }
            }
            else
            {
                if (DoubleJumpsCount < DoubleJumpsMax)
                {
                    GetComponent<Rigidbody2D>().velocity = Vector2.up * JumpScale;
                    DoubleJumpsCount++;
                    if (Animator != null)
                    {
                        Animator.SetTrigger("Jump");
                    }
                }
            }

            if (OnWall)
            {
                if (!GetComponent<SpriteRenderer>().flipX)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 1) * JumpScale;
                    GetComponent<SpriteRenderer>().flipX = true;
                }
                else if (GetComponent<SpriteRenderer>().flipX)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(1, 1) * JumpScale;
                    GetComponent<SpriteRenderer>().flipX = false;
                }
                Animator.SetTrigger("Jump");
            }
        }       
    }
    public void InstanceMoveTo(Vector2 _direction)
    {
        CheckFlip(transform.position + new Vector3(_direction.x, _direction.y));
        if (!OnWall) transform.Translate(new Vector3(_direction.x, _direction.y, transform.position.z));
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, _direction * (RigidbodyCollider.bounds.size.x / 2));
            if (hit.collider != null && hit.collider.tag == "Obstacle")
            {
                OnWall = false;
                if (Animator != null) Animator.ResetTrigger("Slide");
                GetComponent<Rigidbody2D>().gravityScale = 1;
            }
        }
    } // Мгновенное перемещение в указанную точку
    public void Heal(Unit _target, byte _value)
    {
        Debug.Log(_target.name + " " + _value);
        if (_target.Health + _value <= _target.MaxHealth) _target.Health += _value;
        else _target.Health = _target.MaxHealth;
    }
    public void Heal(Unit _target, byte _value, float _time)
    {
        StartCoroutine(PeriodicHeal(_target, _value, _time));
    }
    private IEnumerator PeriodicHeal(Unit _target, byte _value, float _time)
    {
        float elapsedTime = 0;
        while (elapsedTime < _time)
        {
            Heal(_target, _value);
            yield return new WaitForSeconds(1);
            elapsedTime ++;
        }
        StopCoroutine(PeriodicHeal(_target, _value, _time));
    }
    public void Damage(Unit _target, byte _value)
    {
        if (_target.Health - _value > 0)
        {
            _target.Health -= _value;

            Sound.Play(Sound.PlayerSoundType.HIT);

            if (_target == Player.Hero)
            {
                EventManager.PlayerAttacked += method => CameraScript.PulseVignette(Color.red, 0.8f);
                EventManager.PlayerAttacked += method => PlayerBar.Update(PlayerBar.PlayerBarType.HEALTH, -_value, 1f);
                Sound.Play(Sound.PlayerSoundType.HIT);
                EventManager.PlayerAttacked.Invoke(EventManager.PlayerAttacked.Method.GetParameters());
                EventManager.PlayerAttacked -= method => CameraScript.PulseVignette(Color.red, 0.8f);
                EventManager.PlayerAttacked -= method => PlayerBar.Update(PlayerBar.PlayerBarType.HEALTH, -_value, 1f);
            }
        }
        else if (_target.Health - _value <= 0)
        {         
            _target.Health = 0;
            _target.IsDead = true;
            _target.Commandable = false;
            _target.Movable = false;
            _target.CanJump = false;

            if (_target.Animator != null) _target.Animator.SetTrigger("Death");

            if (_target == Player.Hero)
            {
                EventManager.PlayerDied += method => Game.Defeat();
                EventManager.PlayerDied.Invoke(EventManager.PlayerDied.Method.GetParameters());
                EventManager.PlayerDied -= method => Game.Defeat();
            }
        }
    }
    public void Damage(Unit _target, byte _value, float _time)
    {
        StartCoroutine(PeriodicDamage(_target, _value, _time));
    }
    private IEnumerator PeriodicDamage(Unit _target, byte _value, float _time)
    {
        float elapsedTime = 0;
        while (elapsedTime < _time)
        {
            Damage(_target, _value);
            yield return new WaitForSeconds(1);
            elapsedTime++;
        }
        StopCoroutine(PeriodicDamage(_target, _value, _time));
    }
    public void Kill(Unit _target)
    {        
        _target.Damage(_target, _target.MaxHealth);
    }
    public void Stop()
    {
        Queue.Clear();
        CurrentOrder = null;
        StopAllCoroutines();
        Idle();
    }
    /*===============================*/

    /*=== ВСПОМОГАТЕЛЬНЫЕ ФУНКЦИИ ===*/
    public static Unit GetUnitByName(string _name)
    {
        foreach (Unit unit in Units)
        {
            if (unit.name == _name) return unit;
        }
        return null;
    }
    public void AddAbility(Ability _ability)
    {
        Abilities.Add(_ability);
    }
    public void RemoveAbility(Ability _ability)
    {
        Abilities.Remove(_ability);
    }

    public void CheckFlip(Unit _target)
    {
        CheckFlip(_target.transform.position);
    }
    public void CheckFlip(Vector2 _point)
    {
        if (_point.x - transform.position.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            SightVector = transform.position + Vector3.left;            
        }
        else if (_point.x - transform.position.x >= 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            SightVector = transform.position + Vector3.right;
        }
    }
    private void CheckFlip(Vector3 _point)
    {
        CheckFlip(new Vector2(_point.x, _point.y));
    }
    private static float GetPhysicRadius(Unit _first, Unit _second)
    {
        float _firstRadius = Vector3.Distance(_first.RigidbodyCollider.bounds.center, _first.RigidbodyCollider.bounds.min);
        float _secondRadius = Vector3.Distance(_second.RigidbodyCollider.bounds.center, _second.RigidbodyCollider.bounds.min);
        return _firstRadius + _secondRadius;
    }
    private static float GetAttackRadius(Unit _first, Unit _second)
    {
        if (_first.Type == UnitType.MELEE && _second.Type == UnitType.MELEE)
        {
            return GetPhysicRadius(_first, _second) * 1.05f;
        }
        else if (_first.Type == UnitType.MELEE && _second.Type == UnitType.RANGE)
        {
            float radius = GetPhysicRadius(_first, _second);
            return radius + _second.AttackRange;
        }
        else if (_first.Type == UnitType.RANGE && _second.Type == UnitType.MELEE)
        {
            float radius = GetPhysicRadius(_first, _second);
            return radius + _first.AttackRange;
        }
        else if (_first.Type == UnitType.RANGE && _second.Type == UnitType.RANGE)
        {
            float radius = GetPhysicRadius(_first, _second);
            return radius + _first.AttackRange + _second.AttackRange;
        }
        return 0;
    }    
    public void Remove()
    {
        Destroy(gameObject);
    }
    public static void RemoveUnits()
    {
        foreach (Unit _unit in Units)
        {
            if (_unit != null) _unit.Remove();
        }
    }
    public void Idle()
    {
        if (Animator != null)
        {
            Animator.SetBool("Attack", false);
            Animator.SetBool("Fall", false);
            Animator.SetBool("Jump", false);
            Animator.SetBool("Walk", false);
            Animator.SetBool("Slide", false);
            Animator.SetBool("Range Attack", false);
            Animator.SetBool("Idle", true);
        }
    }

    /*=== СОБЫТЫЙНЫЕ ФУНКЦИИ ===*/
    public void Hit()
    {
        if (Target != null && Vector3.Distance(transform.position, Target.transform.position) <= GetAttackRadius(this, Target) && !Target.IsDead && !IsDead && !Target.Invulnerable)
        {
            StopCoroutine(HitCoroutine(Target));
            Damage(Target, AttackDamage);
            StartCoroutine(HitCoroutine(Target));
        }        
    }
    public void Hit(Unit _target, byte _value)
    {
        StopCoroutine(HitCoroutine(_target));
        Damage(_target, _value);
        StartCoroutine(HitCoroutine(_target));
    }
    private IEnumerator HitCoroutine(Unit _target)
    {
        _target.GetComponent<SpriteRenderer>().material.color = DefaultColor + Color.red;
        yield return new WaitForSeconds(1);
        _target.GetComponent<SpriteRenderer>().material.color = DefaultColor;
        StopCoroutine(HitCoroutine(_target));
    }

    /*========== КОЛЛИЗИЯ ==========*/
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Касание с землёй
        if (collision.transform.tag == "Ground")
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal == Vector2.up) // Если нормаль хотя бы одной точки касания смотрит вверх - персонаж на земле
                {
                    OnGround = true;
                    OnWall = false;
                    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    DoubleJumpsCount = 0;
                    SlidesCount = 0;
                    if (Animator != null)
                    {
                        Animator.ResetTrigger("Jump");
                        Animator.ResetTrigger("Fall");
                    }
                }            
            }
        }

        if (collision.transform.tag == "Obstacle")
        {
            if (CanSlide && !OnGround)
            {
                foreach (ContactPoint2D contact in collision.contacts)
                {
                    if (contact.normal == Vector2.left || contact.normal == Vector2.right)
                    {
                        if (SlidesCount < SlidesCountMax)
                        {
                            OnWall = true;
                            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                            GetComponent<Rigidbody2D>().gravityScale = 0;
                            if (contact.normal == Vector2.left) GetComponent<SpriteRenderer>().flipX = false;
                            if (contact.normal == Vector2.right) GetComponent<SpriteRenderer>().flipX = true;
                            if (Animator != null) Animator.SetTrigger("Slide");
                            SlidesCount++;
                        }
                    }
                }
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        // Касание с землёй
        if (collision.transform.tag == "Ground")
        {
            OnGround = false;            
        }       
    }
}
