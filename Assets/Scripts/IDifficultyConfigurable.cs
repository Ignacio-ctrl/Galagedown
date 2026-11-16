// Contrato comun para los enemigos cuya dificultad se ajusta según la ronda.
// La EnemyFactory depende de esta interfaz, no de cada enemigo concreto.
public interface IDifficultyConfigurable
{
    void ConfigureDifficulty(float speedMultiplier, float cadenceMultiplier);
}