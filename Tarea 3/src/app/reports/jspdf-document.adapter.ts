import jsPDF from 'jspdf';
import { CellAlign, FontStyle, PdfDocument } from './pdf-document.model';

/**
 * Adaptador (SRP): traduce PdfDocument a llamadas de jsPDF. Es el único
 * archivo del proyecto que importa "jspdf"; si mañana se cambia de
 * librería, solo se reescribe este adaptador.
 */
export class JsPdfDocumentAdapter implements PdfDocument {
  private static readonly MARGIN_X = 10;
  private static readonly CELL_PADDING = 2;

  private readonly doc = new jsPDF({ unit: 'mm', format: 'a4' });
  private cursorX = JsPdfDocumentAdapter.MARGIN_X;
  private cursorY = 15;

  // jsPDF reutiliza el mismo color de relleno del PDF para el texto y para
  // las figuras: dibujar texto después de un rect() "pisa" el color de
  // relleno que se había fijado. Por eso guardamos ambos colores aquí y los
  // reaplicamos justo antes de cada operación de dibujo, en vez de confiar
  // en que el estado interno de jsPDF se mantenga entre llamadas.
  private fillColor: [number, number, number] = [255, 255, 255];
  private textColor: [number, number, number] = [0, 0, 0];

  setTitle(title: string): void {
    this.doc.setProperties({ title });
  }

  setFont(style: FontStyle, size: number): void {
    this.doc.setFont('helvetica', style);
    this.doc.setFontSize(size);
  }

  setFillColor(red: number, green: number, blue: number): void {
    this.fillColor = [red, green, blue];
  }

  setTextColor(red: number, green: number, blue: number): void {
    this.textColor = [red, green, blue];
  }

  cell(
    width: number,
    height: number,
    text: string,
    align: CellAlign = 'left',
    filled = false,
    border = true,
  ): void {
    if (filled) {
      this.doc.setFillColor(...this.fillColor);
      this.doc.rect(this.cursorX, this.cursorY, width, height, 'F');
    }

    if (border) {
      this.doc.setDrawColor(0, 0, 0);
      this.doc.rect(this.cursorX, this.cursorY, width, height);
    }

    const padding = JsPdfDocumentAdapter.CELL_PADDING;
    let textX = this.cursorX + padding;
    if (align === 'center') textX = this.cursorX + width / 2;
    if (align === 'right') textX = this.cursorX + width - padding;

    this.doc.setTextColor(...this.textColor);
    this.doc.text(text, textX, this.cursorY + height / 2, { align, baseline: 'middle' });
    this.cursorX += width;
  }

  newLine(height = 8): void {
    this.cursorX = JsPdfDocumentAdapter.MARGIN_X;
    this.cursorY += height;
  }

  save(fileName: string): void {
    this.doc.save(fileName);
  }
}
