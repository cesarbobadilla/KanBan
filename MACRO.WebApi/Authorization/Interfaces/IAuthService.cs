using MACRO.Entidad.EntidadesDTO;
using System;
using System.Threading.Tasks;

namespace SCOM.WebApi.Authorization.Interfaces
{
    public interface IAuthService
    {
        Task<String> GenerateTokenAsync(Usuario usuario);

        Task<Usuario> ValidateTokenAsync(string TokenString);
    }
}