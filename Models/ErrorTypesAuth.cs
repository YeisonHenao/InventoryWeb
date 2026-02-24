

public enum ErrorTypesAuth
{
    // Registro e inicio de sesión
    EmailAlreadyRegistered,
    InvalidCredentials,
    UserNotFound,
    PasswordMismatch,
    TokenGenerationFailed,
    UserAccessUpdateFailed,


    // Recuperación de contraseña
    EmailNotRegistered,

}


public class AuthException : Exception
{
    public ErrorTypesAuth Tipo { get; }

    public AuthException(ErrorTypesAuth tipo, string mensaje) : base(mensaje)
    {
        Tipo = tipo;
    }
}