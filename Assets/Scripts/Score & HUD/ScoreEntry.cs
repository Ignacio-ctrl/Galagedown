using System;

namespace Galagedon.Estructuras
{
    public class ScoreEntry : IComparable
    {
        public string PlayerName;
        public int Points;

        public ScoreEntry(string playerName, int points)
        {
            PlayerName = playerName;
            Points = points;
        }

        public int CompareTo(object obj)
        {// cualquier objeto instanciado se considera "mayor" que un objeto nulo. Si el arbol intenta compararse con la nada, devuelve 1 (mayor).
            if (obj == null) return 1;

            ScoreEntry other = obj as ScoreEntry;
            if (other != null)
            {
                // comparamos los puntos, si son iguales, desempata el nombre alfabeticamente, devuelve -1 si el nombre del jugador es menor que el otro, devuelve 1 si es mayor, y 0 si son iguales.
                int result = Points.CompareTo(other.Points);
                if (result == 0)
                {
                    result = string.Compare(PlayerName, other.PlayerName, StringComparison.Ordinal);
                }
                return result;
            }
            throw new ArgumentException("El objeto no es un ScoreEntry");
        }

        public override string ToString()
        {
            return $"{PlayerName}: {Points} pts";
        }
    }
}