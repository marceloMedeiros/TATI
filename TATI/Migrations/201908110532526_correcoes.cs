namespace TATI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class correcoes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Mensagem", "UsuarioID", "dbo.Usuario");
            DropForeignKey("dbo.Protocolo", "DocumentoID", "dbo.Documento");
            DropIndex("dbo.Mensagem", new[] { "UsuarioID" });
            DropIndex("dbo.Protocolo", new[] { "DocumentoID" });
            RenameColumn(table: "dbo.Mensagem", name: "UsuarioID", newName: "Usuario_UsuarioID");
            RenameColumn(table: "dbo.Protocolo", name: "DocumentoID", newName: "Documento_DocumentoID");
            AddColumn("dbo.Motorista", "Nome", c => c.String());
            AlterColumn("dbo.Mensagem", "Usuario_UsuarioID", c => c.Int());
            AlterColumn("dbo.Protocolo", "Documento_DocumentoID", c => c.Int());
            CreateIndex("dbo.Mensagem", "Usuario_UsuarioID");
            CreateIndex("dbo.Protocolo", "Documento_DocumentoID");
            AddForeignKey("dbo.Mensagem", "Usuario_UsuarioID", "dbo.Usuario", "UsuarioID");
            AddForeignKey("dbo.Protocolo", "Documento_DocumentoID", "dbo.Documento", "DocumentoID");
            DropColumn("dbo.Motorista", "MotoristaNome");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Motorista", "MotoristaNome", c => c.String());
            DropForeignKey("dbo.Protocolo", "Documento_DocumentoID", "dbo.Documento");
            DropForeignKey("dbo.Mensagem", "Usuario_UsuarioID", "dbo.Usuario");
            DropIndex("dbo.Protocolo", new[] { "Documento_DocumentoID" });
            DropIndex("dbo.Mensagem", new[] { "Usuario_UsuarioID" });
            AlterColumn("dbo.Protocolo", "Documento_DocumentoID", c => c.Int(nullable: false));
            AlterColumn("dbo.Mensagem", "Usuario_UsuarioID", c => c.Int(nullable: false));
            DropColumn("dbo.Motorista", "Nome");
            RenameColumn(table: "dbo.Protocolo", name: "Documento_DocumentoID", newName: "DocumentoID");
            RenameColumn(table: "dbo.Mensagem", name: "Usuario_UsuarioID", newName: "UsuarioID");
            CreateIndex("dbo.Protocolo", "DocumentoID");
            CreateIndex("dbo.Mensagem", "UsuarioID");
            AddForeignKey("dbo.Protocolo", "DocumentoID", "dbo.Documento", "DocumentoID", cascadeDelete: true);
            AddForeignKey("dbo.Mensagem", "UsuarioID", "dbo.Usuario", "UsuarioID", cascadeDelete: true);
        }
    }
}
