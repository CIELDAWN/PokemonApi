namespace PokemonApi.Helpers
{
    // Format sukses  : { "status": "success", "data": ... }
    // Format error   : { "status": "error",   "message": "..." }
    public static class ApiResponse
    {
        public static object Success(object? data = null)
            => new { status = "success", data };

        public static object Error(string message)
            => new { status = "error", message };
    }
}