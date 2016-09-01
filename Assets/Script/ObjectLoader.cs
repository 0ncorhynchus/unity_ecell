using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class ObjectLoader : MonoBehaviour {

    private string path = "/Users/kato/data/minde/";
    public float scale;
    public float radiusScale;

    private int index;
    private Dictionary<string, GameObject> prefabs;
    private List<List<Particle>> trajectories;
    private Dictionary<ParticleID, GameObject> particles;
    private List<ParticleID> not_updated;

    void Start() {
        index = 0;
        prefabs = new Dictionary<string, GameObject>();
        trajectories = new List<List<Particle>>();
        particles = new Dictionary<ParticleID, GameObject>();
        not_updated = new List<ParticleID>();
        for (int i = 0; i < 100; ++i) {
            trajectories.Add(ReadCSV(path + "minde" + i.ToString("D3") + ".csv"));
        }
        foreach(Particle particle in trajectories[index]) {
            particles.Add(particle.pid, CreateParticle(particle.coordinate, particle.radius, particle.species));
        }
    }

    void Update() {
        if (++index >= trajectories.Count) {
            index = 0;
        }
        not_updated.Clear();
        foreach(ParticleID pid in particles.Keys) {
            not_updated.Add(pid);
        }
        foreach(Particle particle in trajectories[index]) {
            if (particles.ContainsKey(particle.pid)) {
                particles[particle.pid].GetComponent<Transform>().position = particle.coordinate * scale;
                not_updated.Remove(particle.pid);
            } else {
                particles.Add(particle.pid, CreateParticle(particle.coordinate, particle.radius, particle.species));
            }
        }
        foreach(ParticleID pid in not_updated) {
            GameObject.Destroy(particles[pid]);
            particles.Remove(pid);
        }
    }

    List<Particle> ReadCSV(string file) {
        FileInfo fi = new FileInfo(file);
        List<Particle> snapshot = new List<Particle>();
        try {
            using(StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8)) {
                sr.ReadLine(); // Skip The Header Line
                while (!sr.EndOfStream) {
                    var line = sr.ReadLine();
                    var values = line.Split(',');
                    ParticleID pid = new ParticleID(
                            System.Convert.ToInt32(values[5]),
                            System.Convert.ToInt32(values[6]));
                    Vector3 coordinate = new Vector3(
                            System.Convert.ToSingle(values[0]),
                            System.Convert.ToSingle(values[1]),
                            System.Convert.ToSingle(values[2]));

                    snapshot.Add(new Particle(
                                pid,
                                coordinate,
                                /* radius = */ System.Convert.ToSingle(values[3]),
                                /* species = */ values[4]));
                }
            }
        } catch(System.Exception e) {
            Debug.Log(e.Message);
        }
        return snapshot;
    }

    GameObject CreateParticle(Vector3 coordinate, float radius, string species) {
        if (!prefabs.ContainsKey(species)) {
            GameObject prefab = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            float scaledRadius = radius * scale * radiusScale;
            prefab.GetComponent<Transform>().localScale = new Vector3(scaledRadius, scaledRadius, scaledRadius);
            prefab.GetComponent<Renderer>().material.color = RandomColor();
            prefabs.Add(species, prefab);
        }

        return (GameObject) Instantiate(prefabs[species], coordinate * scale, Quaternion.identity);
    }

    Color RandomColor() {
        return new Color(Random.value, Random.value, Random.value, 1.0f);
    }

}
