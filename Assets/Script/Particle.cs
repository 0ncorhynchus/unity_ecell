
public class Particle {

    public ParticleID pid;
    public Real3 coordinate;
    public float radius;
    public string species;

    public Particle(ParticleID pid, Real3 coordinate, float radius, string species) {
        this.pid = pid;
        this.coordinate = coordinate;
        this.radius = radius;
        this.species = species;
    }

}
