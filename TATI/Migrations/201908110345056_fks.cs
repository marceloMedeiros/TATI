namespace TATI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documento", "Motorista_MotoristaID", c => c.Int());
            AddColumn("dbo.Documento", "Usuario_UsuarioID", c => c.Int());
            CreateIndex("dbo.Documento", "Motorista_MotoristaID");
            CreateIndex("dbo.Documento", "Usuario_UsuarioID");
            CreateIndex("dbo.Mensagem", "UsuarioID");
            CreateIndex("dbo.Protocolo", "DocumentoID");
            AddForeignKey("dbo.Documento", "Motorista_MotoristaID", "dbo.Motorista", "MotoristaID");
            AddForeignKey("dbo.Documento", "Usuario_UsuarioID", "dbo.Usuario", "UsuarioID");
            AddForeignKey("dbo.Mensagem", "UsuarioID", "dbo.Usuario", "UsuarioID", cascadeDelete: true);
            AddForeignKey("dbo.Protocolo", "DocumentoID", "dbo.Documento", "DocumentoID", cascadeDelete: true);
            DropColumn("dbo.Documento", "MotoristaID");
            DropColumn("dbo.Documento", "UsuarioID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Documento", "UsuarioID", c => c.Int(nullable: false));
            AddColumn("dbo.Documento", "MotoristaID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Protocolo", "DocumentoID", "dbo.Documento");
            DropForeignKey("dbo.Mensagem", "UsuarioID", "dbo.Usuario");
            DropForeignKey("dbo.Documento", "Usuario_UsuarioID", "dbo.Usuario");
            DropForeignKey("dbo.Documento", "Motorista_MotoristaID", "dbo.Motorista");
            DropIndex("dbo.Protocolo", new[] { "DocumentoID" });
            DropIndex("dbo.Mensagem", new[] { "UsuarioID" });
            DropIndex("dbo.Documento", new[] { "Usuario_UsuarioID" });
            DropIndex("dbo.Documento", new[] { "Motorista_MotoristaID" });
            DropColumn("dbo.Documento", "Usuario_UsuarioID");
            DropColumn("dbo.Documento", "Motorista_MotoristaID");
        }
    }
}
