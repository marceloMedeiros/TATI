namespace TATI.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TATI.CadastroMotoristaContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TATI.CadastroMotoristaContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            Usuario usuarioAdm = new Usuario
            {
                Nome = "Administrador",
                Login = "adm",
                Senha = "123",
                Administrador = true
            };
            Usuario usuario = new Usuario
            {
                Nome = "Usuário",
                Login = "user",
                Senha = "123",
            };

            context.Usuarios.AddOrUpdate(usuarioAdm, usuario);
            context.SaveChanges();


        }
    }
}
