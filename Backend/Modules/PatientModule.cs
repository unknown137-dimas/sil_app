using AutoMapper;
using Backend.DTOs;
using Backend.Utilities;
using Database.Models;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;

namespace Backend.Modules;

public class PatientModule : Module<PatientDTO, Patient>
{
    public PatientModule(
        IRepository<Patient> repository,
        IMapper mapper) : base(repository, mapper)
    {
    }

    public byte[] GetPatientLabel(string patientId)
    {
        var patientData = Repository.GetEntities().First(p => p.Id == patientId);

        var document = PdfUtility.CreateBlankDocument();
        var section = document.LastSection;
        var style = document.Styles["Normal"] ?? throw new InvalidOperationException("Style Normal not found.");
        style.Font.Size = 10;
        style.ParagraphFormat.SpaceBefore = 1;
        style.ParagraphFormat.SpaceAfter = 1;
        section.PageSetup.PageWidth = Unit.FromCentimeter(5);
        section.PageSetup.PageHeight = Unit.FromCentimeter(2);
        section.PageSetup.TopMargin = Unit.FromCentimeter(0);
        section.PageSetup.BottomMargin = Unit.FromCentimeter(0);
        section.PageSetup.LeftMargin = Unit.FromCentimeter(0.1);
        section.PageSetup.RightMargin = Unit.FromCentimeter(0.1);

        #region Patient Info

        var patientDataTable = new Table();
        patientDataTable.AddColumn(Unit.FromCentimeter(5));

        var patipatientDataRow = patientDataTable.AddRow();
        patipatientDataRow.Cells[0].AddParagraph($"{patientData.Name}");

        patipatientDataRow = patientDataTable.AddRow();
        patipatientDataRow.Cells[0].AddParagraph($"{patientData.DateOfBirth:dd MMMM yyyy} ({DateTime.Now.Year - patientData.DateOfBirth.Year})");

        patipatientDataRow = patientDataTable.AddRow();
        patipatientDataRow.Cells[0].AddParagraph($"{patientData.Gender}");

        patipatientDataRow = patientDataTable.AddRow();
        patipatientDataRow.Cells[0].AddParagraph($"No RM : {patientData.MedicalRecordNumber}");

        section.Add(patientDataTable);

        #endregion

        return PdfUtility.ConvertPdfToBytes(document);
    }
}