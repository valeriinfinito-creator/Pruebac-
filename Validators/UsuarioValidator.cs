using DeportivoApp.Models;

namespace DeportivoApp.Validators
{
    public static class UsuarioValidator
    {
        public static (bool IsValid, string Message) Validar(Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Nombre))
                return (false, "El nombre es obligatorio");

            if (string.IsNullOrWhiteSpace(usuario.Documento))
                return (false, "El documento es obligatorio");

            if (string.IsNullOrWhiteSpace(usuario.Email))
                return (false, "El email es obligatorio");

            if (!usuario.Email.Contains("@"))
                return (false, "Email inválido");

            if (string.IsNullOrWhiteSpace(usuario.Telefono))
                return (false, "El teléfono es obligatorio");

            return (true, "OK");
        }
    }
}