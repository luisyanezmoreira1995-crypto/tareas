<?php

/**
 * Contrato para dibujar un ReportDataProviderInterface sobre un PdfDocumentInterface.
 * Nuevas presentaciones (tabla, tarjetas, etc.) se agregan implementando esta
 * interfaz sin modificar el código existente (Principio Abierto/Cerrado).
 */
interface ReportRendererInterface
{
    public function render(PdfDocumentInterface $pdf, ReportDataProviderInterface $data): void;
}
