namespace PokemonApi.Models
{
    public class TipePokemon
    {
        public int id_tipe { get; set; }
        public string nama_tipe { get; set; } = string.Empty;
        public string keterangan { get; set; } = string.Empty;
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }

    public class Pokemon
    {
        public int id_pokemon { get; set; }
        public string nama { get; set; } = string.Empty;
        public int id_tipe { get; set; }
        public string nama_tipe { get; set; } = string.Empty; // dari JOIN
        public int level { get; set; }
        public int hp { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }

    public class MovePokemon
    {
        public int id_move { get; set; }
        public int id_pokemon { get; set; }
        public string nama_move { get; set; } = string.Empty;
        public int power { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }

    // DTO untuk request body (tidak include id dan timestamps)
    public class PokemonRequest
    {
        public string nama { get; set; } = string.Empty;
        public int id_tipe { get; set; }
        public int level { get; set; }
        public int hp { get; set; }
    }

    public class MoveRequest
    {
        public int id_pokemon { get; set; }
        public string nama_move { get; set; } = string.Empty;
        public int power { get; set; }
    }
}