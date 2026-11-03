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
        {
            if (obj == null) return 1;

            ScoreEntry other = obj as ScoreEntry;
            if (other != null)
            {
                // comparamos los puntos, si son iguales, desempata el nombre alfabeticamente
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