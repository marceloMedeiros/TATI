namespace TATI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration3 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Documentoes", newName: "Documento");
            RenameTable(name: "dbo.Mensagems", newName: "Mensagem");
            RenameTable(name: "dbo.Motoristas", newName: "Motorista");
            RenameTable(name: "dbo.Protocoloes", newName: "Protocolo");
            RenameTable(name: "dbo.Usuarios", newName: "Usuario");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Usuario", newName: "Usuarios");
            RenameTable(name: "dbo.Protocolo", newName: "Protocoloes");
            RenameTable(name: "dbo.Motorista", newName: "Motoristas");
            RenameTable(name: "dbo.Mensagem", newName: "Mensagems");
            RenameTable(name: "dbo.Documento", newName: "Documentoes");
        }
    }
}
