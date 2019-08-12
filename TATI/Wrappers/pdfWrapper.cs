using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;

namespace TATI.Wrappers
{
    class PdfWrapper
    {

        private List<Object> textos;

        public PdfWrapper()
        {
            textos = new List<Object>();
        }

        public byte[] gerarPDF()
        {
            byte[] bytes;
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {

                Document document = new Document(PageSize.A4, 10, 10, 10, 10);

                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                foreach (object texto in textos)
                {
                    document.Add((IElement)texto);
                }

                document.Close();
                memoryStream.Close();
                bytes = memoryStream.ToArray();
            }
            return bytes;
        }

        public void limparDoc()
        {
            textos.Clear();
        }

        public void addChunk(string texto)
        {
            Chunk chunk = new Chunk(texto)
            {
                Font = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12f, BaseColor.BLACK)
            };
            textos.Add(chunk);
        }

        public void addPhrase(string texto)
        {
            Phrase phrase = new Phrase(texto)
            {
                Font = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12f, BaseColor.BLACK)
            };
            textos.Add(phrase);
        }

        public void addParagraph(string texto)
        {
            Paragraph paragraph = new Paragraph(texto)
            {
                SpacingBefore = 10,
                SpacingAfter = 10,
                Alignment = Element.ALIGN_LEFT,
                Font = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12f, BaseColor.BLACK)
            };
            textos.Add(paragraph);
        }

    }
}
