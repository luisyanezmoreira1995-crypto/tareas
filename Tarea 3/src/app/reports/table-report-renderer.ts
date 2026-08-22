import { PdfDocument } from './pdf-document.model';
import { ReportColumn, ReportData } from './report-data-provider.model';
import { ReportRenderer } from './report-renderer.model';

/**
 * SRP: su única responsabilidad es dibujar cualquier ReportData como tabla.
 * No sabe de dónde vienen los datos ni qué librería hay detrás de
 * PdfDocument (OCP: sirve para tareas, proyectos, etc. sin modificarse;
 * DIP: depende de abstracciones, no de jsPDF ni de TaskService).
 */
export class TableReportRenderer implements ReportRenderer {
  private static readonly ROW_HEIGHT = 8;
  private static readonly PAGE_CONTENT_WIDTH = 190;

  render(pdf: PdfDocument, data: ReportData): void {
    pdf.setTitle(data.title);

    this.renderHeading(pdf, data.title);

    const columnWidth = TableReportRenderer.PAGE_CONTENT_WIDTH / Math.max(data.columns.length, 1);
    this.renderTableHeader(pdf, data.columns, columnWidth);
    this.renderTableRows(pdf, data.columns, data.rows, columnWidth);
  }

  private renderHeading(pdf: PdfDocument, title: string): void {
    pdf.setFont('bold', 16);
    pdf.setTextColor(0, 0, 0);
    pdf.cell(TableReportRenderer.PAGE_CONTENT_WIDTH, 12, title, 'left', false, false);
    pdf.newLine(16);
  }

  private renderTableHeader(pdf: PdfDocument, columns: ReportColumn[], columnWidth: number): void {
    pdf.setFont('bold', 10);
    pdf.setFillColor(52, 73, 94);
    pdf.setTextColor(255, 255, 255);

    for (const column of columns) {
      pdf.cell(columnWidth, TableReportRenderer.ROW_HEIGHT, column.label, 'center', true);
    }

    pdf.newLine(TableReportRenderer.ROW_HEIGHT);
  }

  private renderTableRows(
    pdf: PdfDocument,
    columns: ReportColumn[],
    rows: Record<string, string>[],
    columnWidth: number,
  ): void {
    pdf.setFont('normal', 9);
    pdf.setTextColor(0, 0, 0);

    if (rows.length === 0) {
      pdf.cell(
        TableReportRenderer.PAGE_CONTENT_WIDTH,
        TableReportRenderer.ROW_HEIGHT,
        'No hay tareas para mostrar.',
        'center',
      );
      pdf.newLine(TableReportRenderer.ROW_HEIGHT);
      return;
    }

    let shaded = false;

    for (const row of rows) {
      pdf.setFillColor(245, 245, 245);

      for (const column of columns) {
        pdf.cell(columnWidth, TableReportRenderer.ROW_HEIGHT, row[column.key] ?? '', 'left', shaded);
      }

      pdf.newLine(TableReportRenderer.ROW_HEIGHT);
      shaded = !shaded;
    }
  }
}
