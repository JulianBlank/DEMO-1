using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Reflexion_winkel : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Ursprüngliche Bewegungsrichtung (vor dem Abprallen)
        Vector3 incomingVelocity = rb.velocity.normalized;

        // Normale am Auftreffpunkt
        Vector3 surfaceNormal = collision.contacts[0].normal;

        // Auftreffwinkel zur Oberfläche (Winkel zwischen Bewegungsrichtung und Normalen)
        float impactAngle = Vector3.Angle(incomingVelocity, surfaceNormal);
        float relativeImpactAngle = 90f - impactAngle;

        // Reflektierte Richtung berechnen (simulierte Abprallrichtung)
        Vector3 reflectedVelocity = Vector3.Reflect(incomingVelocity, surfaceNormal);

        // Reflexionswinkel (zwischen reflektierter Richtung und Normale)
        float reflectionAngle = Vector3.Angle(reflectedVelocity, surfaceNormal);
        float relativeReflectionAngle = 90f - reflectionAngle;

        // Debug-Ausgaben
        Debug.Log($"Kollision mit: {collision.gameObject.name}");
        Debug.Log($"→ Auftreffwinkel zur Oberfläche: {impactAngle:F2}° (relativ: {relativeImpactAngle:F2}°)");
        Debug.Log($"→ Reflexionswinkel zur Oberfläche: {reflectionAngle:F2}° (relativ: {relativeReflectionAngle:F2}°)");
    }
}
