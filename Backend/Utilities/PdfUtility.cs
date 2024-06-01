using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp;

namespace Backend.Utilities;

public static class PdfUtility
{
    public static Document CreateBlankDocument()
    {
        // Create a new PDF document
        var document = new Document();
        var section = document.AddSection();
        section.PageSetup.PageFormat = PageFormat.A4;

        SetDefaultStyles(document);

        return document;
    }

    private static void SetDefaultStyles(Document document)
    {
        var style = document.Styles["Normal"] ?? throw new InvalidOperationException("Style Normal not found.");
        style.Font.Name = "Times New Roman";
        style.Font.Size = 12;
        style.ParagraphFormat.SpaceBefore = 3;
        style.ParagraphFormat.SpaceAfter = 3;

        style = document.Styles["Heading1"]!;
        style.Font.Size = 16;
        style.Font.Bold = true;
        style.ParagraphFormat.PageBreakBefore = false;
        style.ParagraphFormat.SpaceAfter = 6;
        style.ParagraphFormat.KeepWithNext = true;
        style.ParagraphFormat.Alignment = ParagraphAlignment.Center;

        style = document.Styles["Heading2"]!;
        style.Font.Size = 14;
        style.Font.Bold = true;
        style.ParagraphFormat.PageBreakBefore = false;
        style.ParagraphFormat.SpaceBefore = 6;
        style.ParagraphFormat.SpaceAfter = 6;
        style.ParagraphFormat.Alignment = ParagraphAlignment.Left;
    }

    public static byte[] ConvertPdfToBytes(Document document)
    {
        using MemoryStream stream = new MemoryStream();
        var renderer = new PdfDocumentRenderer
        {
            Document = document
        };
        renderer.RenderDocument();
        renderer.PdfDocument.Save(stream, false);
        return stream.ToArray();
    }
}