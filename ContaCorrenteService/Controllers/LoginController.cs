using AutoMapper;
using ContaCorrenteService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ContaCorrenteService.Controllers
{
    public class LoginController : Controller
    {
        private readonly InterfaceUsuarioApp _interfaceUsuario;
        private IMapper _mapper;

        public LoginController(InterfaceUsuarioApp interfaceUsuario, IMapper mapper)
        {
            _interfaceUsuario = interfaceUsuario;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("login")]        
        public async Task<ActionResult<Models.UsuarioLogin>> Autenticar([FromBody]Models.Usuario model)
        {
            try
            {
                if (model == null)
                    return NotFound(new { message = "Usuário ou senha inválidos" });

                var usuario = await Task.Run(() => _interfaceUsuario.Get(model.Username, model.Password));
                if (usuario == null)
                    return NotFound(new { message = "Usuário não localizado" });

                Models.UsuarioLogin usuarioLogin = new Models.UsuarioLogin {
                    Nome = usuario.Nome,
                    Token = GenerateToken(usuario),
                    ContasCorrentes = new List<Entities.Entities.ContaCorrente>(usuario.ContaCorrentes)
                };

                return usuarioLogin;
            }
            catch (Exception)
            {
                return Problem("Erro ao tentar efetuar login", null, 500, "Login error");
            }
        }

        private static string GenerateToken(Entities.Entities.Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.Username.ToString()),
                    //new Claim(ClaimTypes.Role, usuario.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}