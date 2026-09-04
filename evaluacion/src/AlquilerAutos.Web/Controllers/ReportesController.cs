using AlquilerAutos.Shared.Dtos;
using AlquilerAutos.Web.Models;
using AlquilerAutos.Web.Services;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace AlquilerAutos.Web.Controllers;

public class ReportesController : Controller
{
    private readonly ReporteApiClient _reportesApi;
    private readonly ClienteApiClient _clientesApi;

    public ReportesController(ReporteApiClient reportesApi, ClienteApiClient clientesApi)
    {
        _reportesApi = reportesApi;
        _clientesApi = clientesApi;
    }

    public async Task<IActionResult> ContratosPorCliente(int? clienteId)
    {
        var vm = new ReporteViewModel
        {
            Clientes = await _clientesApi.GetAllAsync(),
            ClienteSeleccionadoId = clienteId
        };

        if (clienteId.HasValue)
        {
            vm.Reporte = await _reportesApi.ContratosPorClienteAsync(clienteId.Value);
        }

        return View(vm);
    }

    public async Task<IActionResult> ExportarPdf(int clienteId)
    {
        var reporte = await _reportesApi.ContratosPorClienteAsync(clienteId);
        if (reporte is null) return NotFound();

        var bytes = GenerarPdf(reporte);
        return File(bytes, "application/pdf", $"contratos-{reporte.ClienteId}.pdf");
    }

    public async Task<IActionResult> ExportarExcel(int clienteId)
    {
        var reporte = await _reportesApi.ContratosPorClienteAsync(clienteId);
        if (reporte is null) return NotFound();

        var bytes = GenerarExcel(reporte);
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"contratos-{reporte.ClienteId}.xlsx");
    }

    private static byte[] GenerarPdf(ReporteContratosClienteDto reporte)
    {
        var documento = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Column(col =>
                {
                    col.Item().Text("Reporte de contratos por cliente").FontSize(16).Bold();
                    col.Item().Text($"Cliente: {reporte.ClienteNombre} ({reporte.DocumentoIdentidad})");
                    col.Item().Text($"Generado: {DateTime.Now:yyyy-MM-dd HH:mm}");
                });

                page.Content().PaddingTop(10).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(30);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("ID").Bold();
                        header.Cell().Text("Vehículo").Bold();
                        header.Cell().Text("Inicio").Bold();
                        header.Cell().Text("Fin").Bold();
                        header.Cell().Text("Estado").Bold();
                        header.Cell().Text("Total").Bold();
                        header.Cell().Text("Saldo").Bold();
                    });

                    foreach (var c in reporte.Contratos)
                    {
                        table.Cell().Text(c.ContratoId.ToString());
                        table.Cell().Text(c.VehiculoDescripcion);
                        table.Cell().Text(c.FechaInicio.ToString("yyyy-MM-dd"));
                        table.Cell().Text(c.FechaFin.ToString("yyyy-MM-dd"));
                        table.Cell().Text(c.Estado.ToString());
                        table.Cell().Text(c.Total.ToString("C"));
                        table.Cell().Text(c.SaldoPendiente.ToString("C"));
                    }
                });

                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Página ");
                    x.CurrentPageNumber();
                    x.Span(" de ");
                    x.TotalPages();
                });
            });
        });

        return documento.GeneratePdf();
    }

    private static byte[] GenerarExcel(ReporteContratosClienteDto reporte)
    {
        using var workbook = new XLWorkbook();
        var hoja = workbook.Worksheets.Add("Contratos");

        hoja.Cell(1, 1).Value = "Cliente:";
        hoja.Cell(1, 2).Value = reporte.ClienteNombre;
        hoja.Cell(2, 1).Value = "Documento:";
        hoja.Cell(2, 2).Value = reporte.DocumentoIdentidad;

        var filaEncabezado = 4;
        string[] encabezados = { "ID Contrato", "Vehículo", "Fecha inicio", "Fecha fin", "Estado", "Total", "Total pagado", "Saldo pendiente" };
        for (var i = 0; i < encabezados.Length; i++)
        {
            hoja.Cell(filaEncabezado, i + 1).Value = encabezados[i];
            hoja.Cell(filaEncabezado, i + 1).Style.Font.Bold = true;
        }

        var fila = filaEncabezado + 1;
        foreach (var c in reporte.Contratos)
        {
            hoja.Cell(fila, 1).Value = c.ContratoId;
            hoja.Cell(fila, 2).Value = c.VehiculoDescripcion;
            hoja.Cell(fila, 3).Value = c.FechaInicio;
            hoja.Cell(fila, 4).Value = c.FechaFin;
            hoja.Cell(fila, 5).Value = c.Estado.ToString();
            hoja.Cell(fila, 6).Value = c.Total;
            hoja.Cell(fila, 7).Value = c.TotalPagado;
            hoja.Cell(fila, 8).Value = c.SaldoPendiente;
            fila++;
        }

        hoja.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
