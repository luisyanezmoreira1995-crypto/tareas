import { InjectionToken } from '@angular/core';
import { PdfDocument } from './pdf-document.model';
import { ReportData } from './report-data-provider.model';

/**
 * Contrato para dibujar un ReportData sobre un PdfDocument. Nuevas
 * presentaciones se agregan implementando esta interfaz, sin modificar el
 * código existente (OCP).
 */
export interface ReportRenderer {
  render(pdf: PdfDocument, data: ReportData): void;
}

export const REPORT_RENDERER = new InjectionToken<ReportRenderer>('REPORT_RENDERER');
