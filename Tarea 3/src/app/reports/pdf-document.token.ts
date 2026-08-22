import { InjectionToken } from '@angular/core';
import { PdfDocument } from './pdf-document.model';

/**
 * Cada reporte necesita una instancia NUEVA de documento (no un singleton),
 * por eso se inyecta una fábrica en vez del documento directamente. Es el
 * único punto donde el resto de la app "sabe" cómo se crea un PdfDocument,
 * sin acoplarse a la librería concreta (DIP).
 */
export type PdfDocumentFactory = () => PdfDocument;

export const PDF_DOCUMENT_FACTORY = new InjectionToken<PdfDocumentFactory>('PDF_DOCUMENT_FACTORY');
