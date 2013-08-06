using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class RayGun : MonoBehaviour
{
    // "Gravity Gun" stuff.
    public enum RayGunState { Free = 0, Catch, Occupied, Release };
    public float holdDistance = 4.0f;
    public RayGunState currentState = RayGunState.Free;

    // Laser stuff.
    Vector2 mouse;
    RaycastHit hit;
    float range = 100.0f;
    LineRenderer line;
    public Material lineMaterial;

    public GameObject lastHitEnemy;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.SetVertexCount(2);
        line.renderer.material = lineMaterial;
        line.SetWidth(0.1f, 0.25f);
    }

    void LateUpdate()
    {
        Transform cam = GameObject.FindGameObjectWithTag("Player").camera.transform;
        Color lineColor = line.material.GetColor("_Color");

        switch (currentState)
        {
            case RayGunState.Free:
                if (Input.GetButton("Fire1"))
                {
                    Ray ray = new Ray(cam.position, cam.forward);
                    if (Physics.Raycast(ray, out hit, range) && lineColor.a > 0)
                    {
                        line.enabled = true;
                        line.SetPosition(0, transform.position);
                        line.SetPosition(1, hit.point + hit.normal);
                        line.useWorldSpace = true;
                        if (hit.collider.tag == "Enemy")
                        {
                            lastHitEnemy = hit.collider.gameObject;
                            lastHitEnemy.GetComponent<Enemy>().caughtInLaser = true;
                            line.SetPosition(1, lastHitEnemy.transform.position);
                            currentState = RayGunState.Catch;
                        }
                        else if (lastHitEnemy != null)
                        {
                            lastHitEnemy.GetComponent<Enemy>().caughtInLaser = false;
                            lastHitEnemy = null;
                        }

                        line.material.SetColor("_Color", new Color(lineColor.r,
                                                        lineColor.g,
                                                        lineColor.b,
                                                        lineColor.a - 0.01f));
                    }
                }
                else
                {
                    line.enabled = false;
                    line.material.SetColor("_Color", new Color(lineColor.r,
                                                    lineColor.g,
                                                    lineColor.b,
                                                    1.0f));
                }
                break;
            case RayGunState.Catch:
                if (lastHitEnemy != null)
                {
                    lastHitEnemy.rigidbody.MovePosition(transform.position + transform.forward * holdDistance);
                    line.SetPosition(0, transform.position);
                    line.SetPosition(1, lastHitEnemy.transform.position);
                    currentState = RayGunState.Occupied;
                }
                else
                    currentState = RayGunState.Free;
                break;
            case RayGunState.Occupied:
                if (lastHitEnemy != null)
                {
                    lastHitEnemy.rigidbody.MovePosition(transform.position + transform.forward * holdDistance);
                    line.SetPosition(0, transform.position);
                    line.SetPosition(1, lastHitEnemy.transform.position);
                    if (Input.GetButton("Fire1"))
                        currentState = RayGunState.Release;
                }
                else
                    currentState = RayGunState.Free;
                break;
            case RayGunState.Release:
                    line.enabled = false;
                    line.material.SetColor("_Color", new Color(lineColor.r,
                                                    lineColor.g,
                                                    lineColor.b,
                                                    1.0f));
                    if (lastHitEnemy != null)
                    {
                        lastHitEnemy.GetComponent<Enemy>().caughtInLaser = false;
                        lastHitEnemy = null;
                    }
                    currentState = RayGunState.Free;
                break;
            default:
                break;
        }
    }
}