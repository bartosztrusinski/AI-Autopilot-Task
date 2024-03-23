using UnityEngine;
using UnityEngine.AI;

public class Autopilot : MonoBehaviour
{
  private float distanceToTarget;
  private Transform target;
  private float lastFireTime;

  [Header("Movement")]
  public float moveSpeed = 5f;
  public float rotationSpeed = 0.2f;

  [Header("Shooting")]
  public GameObject projectile;
  public Transform turretBase;
  public float projectileSpeed = 15f;
  public float cooldownTime = 0.4f;
  public float distanceToShoot = 10f;

  private void Awake()
  {
    target = GameObject.Find("Target").transform;
  }

  private void Update() 
  {
    RotateTurret();

    if(CalculateDistance(transform.position, target.position) > distanceToShoot) 
    {
      AutoPilot();
    } 
    else if(Time.time > lastFireTime + cooldownTime) 
    {
      ShootProjectile();
      lastFireTime = Time.time;
    }
  }

  private void AutoPilot()
  {
    Vector3 targetDirection = target.position - transform.position;
    float distance = CalculateDistance(transform.position, target.position);

    LookAtTarget(targetDirection);
    transform.position += transform.forward * moveSpeed * Time.deltaTime;

  }

  private void LookAtTarget(Vector3 target) {
    Vector3 playerForward = transform.forward;
    float angle = CalculateAngle(playerForward, target);
    int clockwise = CalculateCrossProduct(playerForward, target).y < 0 ? -1 : 1;

    if(angle > 2)
      transform.Rotate(0, angle * clockwise * rotationSpeed, 0);
  }

  private float CalculateDistance(Vector3 a, Vector3 b) 
  {
    return Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.z - b.z, 2));
  }

  private float CalculateAngle(Vector3 a, Vector3 b) {
    float dotProduct = CalculateDotProduct(a, b);
    float aLength = CalculateVectorLength(a);
    float bLength = CalculateVectorLength(b);

    return Mathf.Acos(dotProduct / (aLength * bLength)) * Mathf.Rad2Deg;
  }

  private float CalculateDotProduct(Vector3 a, Vector3 b) {
    return a.x * b.x + a.z * b.z;
  }

  private Vector3 CalculateCrossProduct(Vector3 a, Vector3 b) {
    return new Vector3(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
  }

  private float CalculateVectorLength(Vector3 vector) {
    return Mathf.Sqrt(Mathf.Pow(vector.x, 2) + Mathf.Pow(vector.z, 2));
  }

  private void ShootProjectile()
  {
    GameObject projectileInstance = Instantiate(projectile, turretBase.transform.position, turretBase.transform.rotation);
    projectileInstance.GetComponent<Rigidbody>().velocity = projectileSpeed * turretBase.forward;
  }

  private float? CalculateProjectileAngle(bool low)
  {
    Vector3 targetDirection = target.position - transform.position;
    float y = targetDirection.y;
    targetDirection.y = 0f;
    float x = CalculateVectorLength(targetDirection) - 1f;
    float gravity = 9.8f;
    float projSpeedSqr = projectileSpeed * projectileSpeed;
    float underTheSquareRoot = (projSpeedSqr * projSpeedSqr) - gravity * (gravity * x * x + 2 * y * projSpeedSqr);

    if(underTheSquareRoot >= 0f)
    {
      float root = Mathf.Sqrt(underTheSquareRoot);
      float highAngle = projSpeedSqr + root;
      float lowAngle = projSpeedSqr - root;

      float angle = low ? lowAngle : highAngle;

      return Mathf.Atan2(angle, gravity * x) * Mathf.Rad2Deg;
    }

    return null;
  }

  void RotateTurret()
  {
    float? angle = CalculateProjectileAngle(true);

    if(angle != null)
    {
      turretBase.localEulerAngles = new Vector3(360f - (float)angle, 0f, 0f);
    }
  }

}
