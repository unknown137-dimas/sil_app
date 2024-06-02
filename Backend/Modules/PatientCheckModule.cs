using AutoMapper;
using Backend.DTOs;
using Backend.Utilities;
using Database.Enums;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;

namespace Backend.Modules;

public class PatientCheckModule : Module<PatientCheckDTO, PatientCheck>
{
    private readonly IRelationCheckerModule _relationCheckerModule;

    public PatientCheckModule(
        IRepository<PatientCheck> repository,
        IMapper mapper,
        IRelationCheckerModule relationCheckerModule) : base(repository, mapper)
    {
        _relationCheckerModule = relationCheckerModule;
    }

    public override async Task<PatientCheckDTO?> DeleteAsync(string id)
    {
        var patientCheck = await Repository.GetAsync(id);
        if(patientCheck is not null && _relationCheckerModule.Check(patientCheck) is not null)
        {
            throw new Exception("Can't delete because linked to other data");
        }
        return await base.DeleteAsync(id);
    }

    public byte[] ExportPdf(PatientCheckRequestDTO dto)
    {
        var patientResult = Repository.GetEntities()
        .Include(pc => pc.Patient)
        .Include(pc => pc.CheckService)
        .Where(c => c.PatientId == dto.PatientId && c.CheckSchedule == dto.CheckSchedule).ToList();

        // Create a new PDF document
        var document = PdfUtility.CreateBlankDocument();

        document.LastSection.AddParagraph("Patient Result", "Heading1");

        #region Patient Info

        document.LastSection.AddParagraph("Patient Info", "Heading2");
        var patientData = patientResult.Select(p => p.Patient).First();

        var patientDataTable = new Table();
        patientDataTable.AddColumn(Unit.FromCentimeter(2));
        patientDataTable.AddColumn(Unit.FromCentimeter(0.5));
        patientDataTable.AddColumn(Unit.FromCentimeter(7));

        var patipatientDataRow = patientDataTable.AddRow();
        patipatientDataRow.Cells[0].AddParagraph("Name");
        patipatientDataRow.Cells[1].AddParagraph(":");
        patipatientDataRow.Cells[2].AddParagraph($"{patientData.Name}");

        patipatientDataRow = patientDataTable.AddRow();
        patipatientDataRow.Cells[0].AddParagraph("Age");
        patipatientDataRow.Cells[1].AddParagraph(":");
        patipatientDataRow.Cells[2].AddParagraph($"{DateTime.Now.Year - patientData.DateOfBirth.Year} Years");

        patipatientDataRow = patientDataTable.AddRow();
        patipatientDataRow.Cells[0].AddParagraph("Gender");
        patipatientDataRow.Cells[1].AddParagraph(":");
        patipatientDataRow.Cells[2].AddParagraph($"{patientData.Gender}");

        patipatientDataRow = patientDataTable.AddRow();
        patipatientDataRow.Cells[0].AddParagraph("Address");
        patipatientDataRow.Cells[1].AddParagraph(":");
        patipatientDataRow.Cells[2].AddParagraph($"{patientData.Address}");

        document.LastSection.Add(patientDataTable);

        #endregion

        document.LastSection.AddParagraph(); // Add space between section

        #region Patient Result Table

        document.LastSection.AddParagraph("Check Result", "Heading2");
        document.LastSection.AddParagraph($"Check Schedule : {dto.CheckSchedule:dd MMMM yyyy}");
        var table = new Table
        {
            Borders =
            {
                Width = 0.75
            },
            Rows = {
                Alignment = RowAlignment.Center
            }
        };

        var column = table.AddColumn(Unit.FromCentimeter(1));
        column.Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn(Unit.FromCentimeter(7));
        column = table.AddColumn(Unit.FromCentimeter(4));
        column.Format.Alignment = ParagraphAlignment.Center;

        var row = table.AddRow();
        row.Format.Alignment = ParagraphAlignment.Center;
        row.Shading.Color = Color.Parse("0xFF40A654");
        row.Cells[0].AddParagraph("No");
        row.Cells[1].AddParagraph("Check Item");
        row.Cells[2].AddParagraph("Result");

        var index = 1;
        foreach (var patient in patientResult)
        {
            row = table.AddRow();
            row.Cells[0].AddParagraph(index.ToString());
            row.Cells[1].AddParagraph(patient.CheckService.Name);
            string result = "";
            if(patient.CheckService.NormalValueType == CheckType.Numeric)
            {
                if(patient.NumericResult != null)
                {
                    result = $"{patient.NumericResult} {patient.CheckService.CheckUnit}";
                }
            }
            else
            {
                if(patient.StringResult != null)
                {
                    result = patient.StringResult;
                }
            }
            row.Cells[2].AddParagraph(result);

            index++;
        }

        table.SetEdge(0, 0, table.Columns.Count, table.Rows.Count, Edge.Box, BorderStyle.Single, 1.5, Colors.Black);
        document.LastSection.Add(table);

        #endregion

        return PdfUtility.ConvertPdfToBytes(document);
    }

}
