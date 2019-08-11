namespace TATI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inicia : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Documentoes",
                c => new
                    {
                        DocumentoID = c.Int(nullable: false, identity: true),
                        nomeArquivo = c.String(),
                        extensaoArquivo = c.String(),
                        dados = c.Binary(),
                        aprovado = c.Boolean(nullable: false),
                        reprovado = c.Boolean(nullable: false),
                        MotoristaID = c.Int(nullable: false),
                        UsuarioID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DocumentoID);
            
            CreateTable(
                "dbo.Mensagems",
                c => new
                    {
                        MensagemID = c.Int(nullable: false, identity: true),
                        DestinoUsuarioID = c.Int(nullable: false),
                        TextoMensagem = c.String(),
                        visualisado = c.Boolean(nullable: false),
                        UsuarioID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MensagemID);
            
            CreateTable(
                "dbo.Motoristas",
                c => new
                    {
                        MotoristaID = c.Int(nullable: false, identity: true),
                        MotoristaNome = c.String(),
                        Celular = c.String(),
                        Email = c.String(),
                        CPFouCNPJ = c.String(),
                        DataHoraCriacao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MotoristaID);
            
            CreateTable(
                "dbo.Protocoloes",
                c => new
                    {
                        ProtocoloID = c.Int(nullable: false, identity: true),
                        dados = c.Binary(),
                        DocumentoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProtocoloID);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        UsuarioID = c.Int(nullable: false, identity: true),
                        Login = c.String(),
                        Senha = c.String(),
                        Administrador = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UsuarioID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Usuarios");
            DropTable("dbo.Protocoloes");
            DropTable("dbo.Motoristas");
            DropTable("dbo.Mensagems");
            DropTable("dbo.Documentoes");
        }
    }
}
