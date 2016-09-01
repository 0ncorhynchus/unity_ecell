using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

class TargetSystem {

    private string path = "/Users/kato/data/minde/";
    private List<List<Particle>> trajectories;

    public TargetSystem() {
        trajectories = new List<List<Particle>>();
        for (int i = 0; i < 100; ++i) {
            AddCSV(path + "minde" + i.ToString("D3") + ".csv");
        }
    }

    public int Count {
        get { return trajectories.Count; }
    }

    bool Contains(int index) {
        return index >= 0 && index < trajectories.Count;
    }

    public List<Particle> this[int i] {
        get { return trajectories[i]; }
    }

    public void AddCSV(string filepath) {
        trajectories.Add(ReadCSV(filepath));
    }

    private List<Particle> ReadCSV(string filepath) {
        FileInfo fi = new FileInfo(filepath);
        List<Particle> snapshot = new List<Particle>();
        using(StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8)) {
            sr.ReadLine(); // Skip The Header Line
            while (!sr.EndOfStream) {
                var line = sr.ReadLine();
                var values = line.Split(',');
                ParticleID pid = new ParticleID(
                        System.Convert.ToInt32(values[5]),
                        System.Convert.ToInt32(values[6]));
                Real3 coordinate = new Real3(
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
        return snapshot;
    }

}
