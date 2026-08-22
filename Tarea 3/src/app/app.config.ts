import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { JsPdfDocumentAdapter } from './reports/jspdf-document.adapter';
import { PDF_DOCUMENT_FACTORY } from './reports/pdf-document.token';
import { REPORT_RENDERER } from './reports/report-renderer.model';
import { TableReportRenderer } from './reports/table-report-renderer';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    // Composition root: único lugar que ata las abstracciones de reportes
    // (PdfDocument, ReportRenderer) a sus implementaciones concretas.
    { provide: PDF_DOCUMENT_FACTORY, useValue: () => new JsPdfDocumentAdapter() },
    { provide: REPORT_RENDERER, useClass: TableReportRenderer },
  ]
};
