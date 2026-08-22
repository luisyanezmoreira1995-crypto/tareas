/**
 * Abstracción del documento PDF (ISP): expone solo lo que un renderer de
 * reportes necesita. Ningún otro archivo, salvo su adaptador concreto,
 * conoce la librería real usada para generar el PDF (DIP).
 */
export type CellAlign = 'left' | 'center' | 'right';
export type FontStyle = 'normal' | 'bold';

export interface PdfDocument {
  setTitle(title: string): void;
  setFont(style: FontStyle, size: number): void;
  setFillColor(red: number, green: number, blue: number): void;
  setTextColor(red: number, green: number, blue: number): void;
  cell(
    width: number,
    height: number,
    text: string,
    align?: CellAlign,
    filled?: boolean,
    border?: boolean,
  ): void;
  newLine(height?: number): void;
  save(fileName: string): void;
}
