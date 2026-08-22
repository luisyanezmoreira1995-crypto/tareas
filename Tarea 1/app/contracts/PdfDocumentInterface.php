<?php

/**
 * Abstracción del documento PDF. El resto de la aplicación depende de esta
 * interfaz, nunca de FPDF directamente (Principio de Inversión de Dependencias).
 * Solo expone lo que un renderer de reportes necesita (Segregación de Interfaces).
 */
interface PdfDocumentInterface
{
    public function setTitle(string $title): void;

    public function addPage(): void;

    public function setFont(string $family, string $style = '', float $size = 0): void;

    public function setFillColor(int $red, int $green, int $blue): void;

    public function setTextColor(int $red, int $green, int $blue): void;

    public function cell(
        float $width,
        float $height,
        string $text = '',
        int $border = 0,
        int $ln = 0,
        string $align = '',
        bool $fill = false
    ): void;

    public function ln(?float $height = null): void;

    public function output(string $destination = 'S', string $name = ''): string;
}
