using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ObjectLoader : MonoBehaviour {

    public float scale;
    public float radiusScale;
    public Text stepViewer;

    public int updatePerFrame;

    private bool playing;
    private int index;
    private int frame;
    private TargetSystem system;
    private Dictionary<string, GameObject> prefabs;
    private Dictionary<ParticleID, GameObject> particles;
    private List<ParticleID> not_updated;

    void Start() {
        playing = false;
        index = 0;
        frame = 0;
        updatePerFrame = 10;
        try {
            system = new TargetSystem();
        } catch (System.Exception e) {
            Debug.Log(e.Message);
        }
        prefabs = new Dictionary<string, GameObject>();
        particles = new Dictionary<ParticleID, GameObject>();
        not_updated = new List<ParticleID>();

        stepViewer.GetComponent<Text>().text = index.ToString();
        foreach(Particle particle in system[index]) {
            Vector3 position = Real2Vector(particle.coordinate);
            particles.Add(particle.pid, CreateParticle(position, particle.radius, particle.species));
        }
    }

    void Update() {
        if (Input.GetButtonDown("Play")) {
            playing = !playing;
        }
        if (playing) {
            if (frame++ == updatePerFrame) {
                frame = 0;
                Step();
            }
        }
    }

    void Step() {
        if (++index >= system.Count) {
            index = 0;
        }
        stepViewer.GetComponent<Text>().text = index.ToString();
        not_updated.Clear();
        foreach(ParticleID pid in particles.Keys) {
            not_updated.Add(pid);
        }
        foreach(Particle particle in system[index]) {
            Vector3 position = Real2Vector(particle.coordinate);
            if (particles.ContainsKey(particle.pid)) {
                particles[particle.pid].GetComponent<Transform>().position = position;
                not_updated.Remove(particle.pid);
            } else {
                particles.Add(particle.pid, CreateParticle(position, particle.radius, particle.species));
            }
        }
        foreach(ParticleID pid in not_updated) {
            GameObject.Destroy(particles[pid]);
            particles.Remove(pid);
        }
    }

    Vector3 Real2Vector(Real3 real3) {
        return new Vector3(real3.x * scale, real3.y * scale, real3.z * scale);
    }

    GameObject CreateParticle(Vector3 position, float radius, string species) {
        if (!prefabs.ContainsKey(species)) {
            GameObject prefab = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            float scaledRadius = radius * scale * radiusScale;
            prefab.GetComponent<Transform>().localScale = new Vector3(scaledRadius, scaledRadius, scaledRadius);
            prefab.GetComponent<Renderer>().material.color = RandomColor();
            prefabs.Add(species, prefab);
        }

        return (GameObject) Instantiate(prefabs[species], position, Quaternion.identity);
    }

    Color RandomColor() {
        return new Color(Random.value, Random.value, Random.value, 1.0f);
    }

}
