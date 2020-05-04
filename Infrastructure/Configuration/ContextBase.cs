using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Configuration
{
    public class ContextBase : DbContext
    {
        public ContextBase(DbContextOptions<ContextBase> options) : base(options)
        {   
            Database.EnsureCreated();
        }

        public DbSet<ContaCorrente> ContaCorrente { get; set; }
        public DbSet<ContaCorrenteLancamentos> ContaCorrenteLancamentos { get; set; }
        public DbSet<Agencia> Agencia { get; set; }
        public DbSet<Usuario> Usuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Se não estiver configurado no projeto UI
            if (!optionsBuilder.IsConfigured) //TO DO: REVER ISSO, POIS DEVE PEGAR DO APPSETTINGSSS.JSON
                optionsBuilder.UseSqlServer(GetStringConnectionConfig());
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Seed de dados mock

            //Estamos populando automaticamente a base para testes

            byte[] passwordHashUsuario1, passwordSaltUsuario1, passwordHashUsuario2, passwordSaltUsuario2;
            CreatePasswordHash("teste001", out passwordHashUsuario1, out passwordSaltUsuario1);
            CreatePasswordHash("teste002", out passwordHashUsuario2, out passwordSaltUsuario2);

            modelBuilder.Entity<Usuario>().HasData(
                        new Usuario
                        {
                            Id = 1,
                            Username = "batman",
                            Nome = "Batman da Silva",
                            PasswordHash = passwordHashUsuario1,
                            PasswordSalt = passwordSaltUsuario1,
                            DataCriacao = DateTime.Now
                        },
                        new Usuario { Id = 2, Username = "robin", Nome = "Robin Rodrigues", PasswordHash = passwordHashUsuario2, PasswordSalt = passwordSaltUsuario2, DataCriacao = DateTime.Now });

            modelBuilder.Entity<Agencia>().HasData(
                new Agencia { Id = 1, Nome = "Agência 1", Digito = "1", DataCriacao = DateTime.Now },
                new Agencia { Id = 2, Nome = "Agência 2", Digito = "2", DataCriacao = DateTime.Now });

            modelBuilder.Entity<ContaCorrente>().HasData(
                new
                {
                    Id = 1,
                    Digito = "1",
                    UsuarioId = 1,
                    AgenciaId = 1,
                    DataCriacao = DateTime.Now
                },
                new
                {
                    Id = 2,
                    Digito = "2",
                    UsuarioId = 2,
                    AgenciaId = 2,
                    DataCriacao = DateTime.Now
                }
             );

            modelBuilder.Entity<ContaCorrenteLancamentos>().HasData(
                new
                {
                    Id = 1,
                    DataCriacao = DateTime.Now,
                    Valor = (decimal)5,
                    ContaCorrenteId = 1,
                    TipoLancamento = Entities.Enums.TipoLancamentoEnum.Credito,
                    DataLancamento = DateTime.Now.AddDays(-10)
                },
                new
                {
                    Id = 2,
                    DataCriacao = DateTime.Now,
                    Valor = (decimal)10,
                    ContaCorrenteId = 1,
                    TipoLancamento = Entities.Enums.TipoLancamentoEnum.Credito,
                    DataLancamento = DateTime.Now.AddDays(-8)
                },
                new
                {
                    Id = 3,
                    DataCriacao = DateTime.Now,
                    Valor = (decimal)5,
                    ContaCorrenteId = 1,
                    TipoLancamento = Entities.Enums.TipoLancamentoEnum.Debito,
                    DataLancamento = DateTime.Now
                },
                new
                {
                    Id = 4,
                    DataCriacao = DateTime.Now,
                    Valor = (decimal)15,
                    ContaCorrenteId = 2,
                    TipoLancamento = Entities.Enums.TipoLancamentoEnum.Credito,
                    DataLancamento = DateTime.Now.AddDays(-8)
                },
                new
                {
                    Id = 5,
                    DataCriacao = DateTime.Now,
                    Valor = (decimal)20,
                    ContaCorrenteId = 2,
                    TipoLancamento = Entities.Enums.TipoLancamentoEnum.Debito,
                    DataLancamento = DateTime.Now.AddDays(-8)
                },
                new
                {
                    Id = 6,
                    DataCriacao = DateTime.Now,
                    Valor = (decimal)7,
                    ContaCorrenteId = 2,
                    TipoLancamento = Entities.Enums.TipoLancamentoEnum.Debito,
                    DataLancamento = DateTime.Now
                }
             );
            #endregion
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private string GetStringConnectionConfig()
        {
            string strCon = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=testefranciscoDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            return strCon;
        }
    }
}
