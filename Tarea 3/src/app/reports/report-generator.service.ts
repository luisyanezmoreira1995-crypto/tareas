import { Inject, Injectable } from '@angular/core';
import { PDF_DOCUMENT_FACTORY, PdfDocumentFactory } from './pdf-document.token';
import { ReportDataProvider } from './report-data-provider.model';
import { REPORT_RENDERER, ReportRenderer } from './report-renderer.model';

/**
 * Orquestador del reporte (DIP): solo conoce abstracciones, inyectadas por
 * constructor. No instancia jsPDF ni sabe qué renderer concreto se está
 * usando; eso se decide en app.config.ts (composition root).
 */
@Injectable({ providedIn: 'root' })
export class ReportGeneratorService {
  constructor(
    @Inject(PDF_DOCUMENT_FACTORY) private readonly createPdfDocument: PdfDocumentFactory,
    @Inject(REPORT_RENDERER) private readonly renderer: ReportRenderer,
  ) {}

  generate<TSource>(dataProvider: ReportDataProvider<TSource>, source: TSource, fileName: string): void {
    const data = dataProvider.build(source);
    const pdf = this.createPdfDocument();
    this.renderer.render(pdf, data);
    pdf.save(fileName);
  }
}
