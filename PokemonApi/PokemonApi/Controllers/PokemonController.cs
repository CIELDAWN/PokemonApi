using Microsoft.AspNetCore.Mvc;
using PokemonApi.Helpers;
using PokemonApi.Models;

namespace PokemonApi.Controllers
{
    [ApiController]
    [Route("api/pokemon")]
    public class PokemonController : ControllerBase
    {
        private string __constr;

        public PokemonController(IConfiguration configuration)
        {
            __constr = configuration.GetConnectionString("WebApiDatabase")!;
        }

        // GET api/pokemon
        // Ambil semua pokemon (beserta nama tipe dari JOIN)
        [HttpGet]
        public ActionResult GetAll()
        {
            PokemonContext ctx = new PokemonContext(__constr);
            var list = ctx.ListPokemon();
            return Ok(ApiResponse.Success(list));
        }

        // GET api/pokemon/{id}
        // Ambil detail satu pokemon by ID
        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            PokemonContext ctx = new PokemonContext(__constr);
            var pokemon = ctx.GetPokemonById(id);
            if (pokemon == null)
                return NotFound(ApiResponse.Error("Pokemon tidak ditemukan"));
            return Ok(ApiResponse.Success(pokemon));
        }

        // POST api/pokemon
        // Tambah pokemon baru
        // Body: { "nama": "Eevee", "id_tipe": 1, "level": 5, "hp": 55 }
        [HttpPost]
        public ActionResult Add([FromBody] PokemonRequest p)
        {
            if (string.IsNullOrWhiteSpace(p.nama))
                return BadRequest(ApiResponse.Error("Field 'nama' tidak boleh kosong"));
            if (p.id_tipe <= 0)
                return BadRequest(ApiResponse.Error("Field 'id_tipe' tidak valid"));
            if (p.level < 1 || p.level > 100)
                return BadRequest(ApiResponse.Error("Level harus antara 1 dan 100"));
            if (p.hp <= 0)
                return BadRequest(ApiResponse.Error("HP harus lebih dari 0"));

            PokemonContext ctx = new PokemonContext(__constr);
            bool result = ctx.AddPokemon(p);
            if (result)
                return StatusCode(201, ApiResponse.Success(new { message = "Pokemon berhasil ditambahkan" }));
            return StatusCode(500, ApiResponse.Error($"Gagal menambahkan pokemon: {ctx.ErrorMsg}"));
        }

        // PUT api/pokemon/{id}
        // Update data pokemon
        // Body: { "nama": "Raichu", "id_tipe": 1, "level": 30, "hp": 60 }
        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] PokemonRequest p)
        {
            if (string.IsNullOrWhiteSpace(p.nama))
                return BadRequest(ApiResponse.Error("Field 'nama' tidak boleh kosong"));
            if (p.level < 1 || p.level > 100)
                return BadRequest(ApiResponse.Error("Level harus antara 1 dan 100"));
            if (p.hp <= 0)
                return BadRequest(ApiResponse.Error("HP harus lebih dari 0"));

            PokemonContext ctx = new PokemonContext(__constr);
            bool result = ctx.UpdatePokemon(id, p);
            if (result)
                return Ok(ApiResponse.Success(new { message = "Pokemon berhasil diupdate" }));
            if (ctx.GetPokemonById(id) == null)
                return NotFound(ApiResponse.Error("Pokemon tidak ditemukan"));
            return StatusCode(500, ApiResponse.Error($"Gagal update pokemon: {ctx.ErrorMsg}"));
        }

        // DELETE api/pokemon/{id}
        // Hapus pokemon (akan cascade delete moves-nya juga)
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            PokemonContext ctx = new PokemonContext(__constr);
            bool result = ctx.DeletePokemon(id);
            if (result)
                return Ok(ApiResponse.Success(new { message = "Pokemon berhasil dihapus" }));
            return NotFound(ApiResponse.Error("Pokemon tidak ditemukan"));
        }
    }

    // ──────────────────────────────────────────────────────────

    [ApiController]
    [Route("api/tipe")]
    public class TipeController : ControllerBase
    {
        private string __constr;

        public TipeController(IConfiguration configuration)
        {
            __constr = configuration.GetConnectionString("WebApiDatabase")!;
        }

        // GET api/tipe
        // Ambil semua tipe pokemon
        [HttpGet]
        public ActionResult GetAll()
        {
            PokemonContext ctx = new PokemonContext(__constr);
            var list = ctx.ListTipe();
            return Ok(ApiResponse.Success(list));
        }
    }

    // ──────────────────────────────────────────────────────────

    [ApiController]
    [Route("api/pokemon/{idPokemon}/moves")]
    public class MoveController : ControllerBase
    {
        private string __constr;

        public MoveController(IConfiguration configuration)
        {
            __constr = configuration.GetConnectionString("WebApiDatabase")!;
        }

        // GET api/pokemon/{idPokemon}/moves
        // Ambil semua gerakan milik satu pokemon
        [HttpGet]
        public ActionResult GetMoves(int idPokemon)
        {
            PokemonContext ctx = new PokemonContext(__constr);
            // cek dulu pokemon-nya ada
            var pokemon = ctx.GetPokemonById(idPokemon);
            if (pokemon == null)
                return NotFound(ApiResponse.Error("Pokemon tidak ditemukan"));

            var moves = ctx.ListMoveByPokemon(idPokemon);
            return Ok(ApiResponse.Success(moves));
        }

        // POST api/pokemon/{idPokemon}/moves
        // Tambah gerakan ke pokemon
        // Body: { "nama_move": "Thunder", "power": 110 }
        [HttpPost]
        public ActionResult AddMove(int idPokemon, [FromBody] MoveRequest m)
        {
            if (string.IsNullOrWhiteSpace(m.nama_move))
                return BadRequest(ApiResponse.Error("Field 'nama_move' tidak boleh kosong"));
            if (m.power < 0)
                return BadRequest(ApiResponse.Error("Power tidak boleh negatif"));

            m.id_pokemon = idPokemon;

            PokemonContext ctx = new PokemonContext(__constr);
            // cek dulu pokemon-nya ada
            var pokemon = ctx.GetPokemonById(idPokemon);
            if (pokemon == null)
                return NotFound(ApiResponse.Error("Pokemon tidak ditemukan"));

            bool result = ctx.AddMove(m);
            if (result)
                return StatusCode(201, ApiResponse.Success(new { message = "Move berhasil ditambahkan" }));
            return StatusCode(500, ApiResponse.Error($"Gagal menambahkan move: {ctx.ErrorMsg}"));
        }

        // DELETE api/pokemon/{idPokemon}/moves/{idMove}
        // Hapus gerakan
        [HttpDelete("{idMove}")]
        public ActionResult DeleteMove(int idPokemon, int idMove)
        {
            PokemonContext ctx = new PokemonContext(__constr);
            bool result = ctx.DeleteMove(idMove);
            if (result)
                return Ok(ApiResponse.Success(new { message = "Move berhasil dihapus" }));
            return NotFound(ApiResponse.Error("Move tidak ditemukan"));
        }
    }
}