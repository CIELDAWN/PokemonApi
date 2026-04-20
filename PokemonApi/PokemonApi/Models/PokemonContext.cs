using Npgsql;
using NpgsqlTypes;
using PokemonApi.Helpers;

namespace PokemonApi.Models
{
    public class PokemonContext
    {
        private string __constr;
        public string ErrorMsg { get; private set; } = string.Empty;

        public PokemonContext(string pConstr)
        {
            __constr = pConstr;
        }

        public List<TipePokemon> ListTipe()
        {
            var list = new List<TipePokemon>();
            const string query = @"
                SELECT id_tipe, nama_tipe, keterangan, created_at, updated_at
                FROM   pokemon_db.tipe_pokemon
                ORDER BY id_tipe;";

            SqlDBHelper db = new SqlDBHelper(__constr);
            try
            {
                var cmd = db.getNpgsqlCommand(query);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(MapTipe(reader));
                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex) { ErrorMsg = ex.Message; }
            return list;
        }

        public List<Pokemon> ListPokemon()
        {
            var list = new List<Pokemon>();
            const string query = @"
                SELECT p.id_pokemon, p.nama, p.id_tipe, t.nama_tipe,
                       p.level, p.hp, p.created_at, p.updated_at
                FROM   pokemon_db.pokemon p
                JOIN   pokemon_db.tipe_pokemon t ON t.id_tipe = p.id_tipe
                ORDER BY p.id_pokemon;";

            SqlDBHelper db = new SqlDBHelper(__constr);
            try
            {
                var cmd = db.getNpgsqlCommand(query);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(MapPokemon(reader));
                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex) { ErrorMsg = ex.Message; }
            return list;
        }

        public Pokemon? GetPokemonById(int id)
        {
            Pokemon? result = null;
            const string query = @"
                SELECT p.id_pokemon, p.nama, p.id_tipe, t.nama_tipe,
                       p.level, p.hp, p.created_at, p.updated_at
                FROM   pokemon_db.pokemon p
                JOIN   pokemon_db.tipe_pokemon t ON t.id_tipe = p.id_tipe
                WHERE  p.id_pokemon = @id;";

            SqlDBHelper db = new SqlDBHelper(__constr);
            try
            {
                var cmd = db.getNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, id);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                    result = MapPokemon(reader);
                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex) { ErrorMsg = ex.Message; }
            return result;
        }

        public bool AddPokemon(PokemonRequest p)
        {
            const string query = @"
                INSERT INTO pokemon_db.pokemon (nama, id_tipe, level, hp)
                VALUES (@nama, @id_tipe, @level, @hp);";

            SqlDBHelper db = new SqlDBHelper(__constr);
            try
            {
                var cmd = db.getNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@nama", NpgsqlDbType.Varchar, p.nama);
                cmd.Parameters.AddWithValue("@id_tipe", NpgsqlDbType.Integer, p.id_tipe);
                cmd.Parameters.AddWithValue("@level", NpgsqlDbType.Integer, p.level);
                cmd.Parameters.AddWithValue("@hp", NpgsqlDbType.Integer, p.hp);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                db.closeConnection();
                return true;
            }
            catch (Exception ex) { ErrorMsg = ex.Message; return false; }
        }

        public bool UpdatePokemon(int id, PokemonRequest p)
        {
            const string query = @"
                UPDATE pokemon_db.pokemon
                SET    nama      = @nama,
                       id_tipe   = @id_tipe,
                       level     = @level,
                       hp        = @hp,
                       updated_at = NOW()
                WHERE  id_pokemon = @id;";

            SqlDBHelper db = new SqlDBHelper(__constr);
            try
            {
                var cmd = db.getNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@nama", NpgsqlDbType.Varchar, p.nama);
                cmd.Parameters.AddWithValue("@id_tipe", NpgsqlDbType.Integer, p.id_tipe);
                cmd.Parameters.AddWithValue("@level", NpgsqlDbType.Integer, p.level);
                cmd.Parameters.AddWithValue("@hp", NpgsqlDbType.Integer, p.hp);
                cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, id);
                int rows = cmd.ExecuteNonQuery();
                cmd.Dispose();
                db.closeConnection();
                return rows > 0;
            }
            catch (Exception ex) { ErrorMsg = ex.Message; return false; }
        }

        public bool DeletePokemon(int id)
        {
            const string query = @"
                DELETE FROM pokemon_db.pokemon
                WHERE id_pokemon = @id;";

            SqlDBHelper db = new SqlDBHelper(__constr);
            try
            {
                var cmd = db.getNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, id);
                int rows = cmd.ExecuteNonQuery();
                cmd.Dispose();
                db.closeConnection();
                return rows > 0;
            }
            catch (Exception ex) { ErrorMsg = ex.Message; return false; }
        }

        public List<MovePokemon> ListMoveByPokemon(int idPokemon)
        {
            var list = new List<MovePokemon>();
            const string query = @"
                SELECT id_move, id_pokemon, nama_move, power, created_at, updated_at
                FROM   pokemon_db.move_pokemon
                WHERE  id_pokemon = @id_pokemon
                ORDER BY id_move;";

            SqlDBHelper db = new SqlDBHelper(__constr);
            try
            {
                var cmd = db.getNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id_pokemon", NpgsqlDbType.Integer, idPokemon);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(MapMove(reader));
                cmd.Dispose();
                db.closeConnection();
            }
            catch (Exception ex) { ErrorMsg = ex.Message; }
            return list;
        }

        public bool AddMove(MoveRequest m)
        {
            const string query = @"
                INSERT INTO pokemon_db.move_pokemon (id_pokemon, nama_move, power)
                VALUES (@id_pokemon, @nama_move, @power);";

            SqlDBHelper db = new SqlDBHelper(__constr);
            try
            {
                var cmd = db.getNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id_pokemon", NpgsqlDbType.Integer, m.id_pokemon);
                cmd.Parameters.AddWithValue("@nama_move", NpgsqlDbType.Varchar, m.nama_move);
                cmd.Parameters.AddWithValue("@power", NpgsqlDbType.Integer, m.power);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                db.closeConnection();
                return true;
            }
            catch (Exception ex) { ErrorMsg = ex.Message; return false; }
        }

        public bool DeleteMove(int idMove)
        {
            const string query = @"
                DELETE FROM pokemon_db.move_pokemon
                WHERE id_move = @id_move;";

            SqlDBHelper db = new SqlDBHelper(__constr);
            try
            {
                var cmd = db.getNpgsqlCommand(query);
                cmd.Parameters.AddWithValue("@id_move", NpgsqlDbType.Integer, idMove);
                int rows = cmd.ExecuteNonQuery();
                cmd.Dispose();
                db.closeConnection();
                return rows > 0;
            }
            catch (Exception ex) { ErrorMsg = ex.Message; return false; }
        }

        private TipePokemon MapTipe(NpgsqlDataReader r) => new TipePokemon
        {
            id_tipe = r.GetInt32(r.GetOrdinal("id_tipe")),
            nama_tipe = r.GetString(r.GetOrdinal("nama_tipe")),
            keterangan = r.IsDBNull(r.GetOrdinal("keterangan")) ? "" : r.GetString(r.GetOrdinal("keterangan")),
            created_at = r.GetDateTime(r.GetOrdinal("created_at")),
            updated_at = r.GetDateTime(r.GetOrdinal("updated_at"))
        };

        private Pokemon MapPokemon(NpgsqlDataReader r) => new Pokemon
        {
            id_pokemon = r.GetInt32(r.GetOrdinal("id_pokemon")),
            nama = r.GetString(r.GetOrdinal("nama")),
            id_tipe = r.GetInt32(r.GetOrdinal("id_tipe")),
            nama_tipe = r.GetString(r.GetOrdinal("nama_tipe")),
            level = r.GetInt32(r.GetOrdinal("level")),
            hp = r.GetInt32(r.GetOrdinal("hp")),
            created_at = r.GetDateTime(r.GetOrdinal("created_at")),
            updated_at = r.GetDateTime(r.GetOrdinal("updated_at"))
        };

        private MovePokemon MapMove(NpgsqlDataReader r) => new MovePokemon
        {
            id_move = r.GetInt32(r.GetOrdinal("id_move")),
            id_pokemon = r.GetInt32(r.GetOrdinal("id_pokemon")),
            nama_move = r.GetString(r.GetOrdinal("nama_move")),
            power = r.GetInt32(r.GetOrdinal("power")),
            created_at = r.GetDateTime(r.GetOrdinal("created_at")),
            updated_at = r.GetDateTime(r.GetOrdinal("updated_at"))
        };
    }
}