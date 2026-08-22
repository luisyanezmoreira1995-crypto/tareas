<?php

require_once APP_PATH . '/vendor/fpdf/fpdf.php';

/**
 * Adaptador (SRP: su única responsabilidad es traducir PdfDocumentInterface
 * a llamadas de la librería FPDF). Es el único archivo del proyecto que
 * conoce la clase concreta FPDF; si mañana se cambia de librería, solo
 * se reescribe este adaptador.
 */
class FpdfDocument implements PdfDocumentInterface
{
    private FPDF $fpdf;

    public function __construct(string $orientation = 'P', string $unit = 'mm', string $size = 'A4')
    {
        $this->fpdf = new FPDF($orientation, $unit, $size);
    }

    public function setTitle(string $title): void
    {
        $this->fpdf->SetTitle($this->toLatin1($title));
    }

    public function addPage(): void
    {
        $this->fpdf->AddPage();
    }

    public function setFont(string $family, string $style = '', float $size = 0): void
    {
        $this->fpdf->SetFont($family, $style, $size);
    }

    public function setFillColor(int $red, int $green, int $blue): void
    {
        $this->fpdf->SetFillColor($red, $green, $blue);
    }

    public function setTextColor(int $red, int $green, int $blue): void
    {
        $this->fpdf->SetTextColor($red, $green, $blue);
    }

    public function cell(
        float $width,
        float $height,
        string $text = '',
        int $border = 0,
        int $ln = 0,
        string $align = '',
        bool $fill = false
    ): void {
        $this->fpdf->Cell($width, $height, $this->toLatin1($text), $border, $ln, $align, $fill);
    }

    public function ln(?float $height = null): void
    {
        if ($height === null) {
            $this->fpdf->Ln();
            return;
        }

        $this->fpdf->Ln($height);
    }

    public function output(string $destination = 'S', string $name = ''): string
    {
        return $this->fpdf->Output($destination, $name);
    }

    private function toLatin1(string $text): string
    {
        return mb_convert_encoding($text, 'ISO-8859-1', 'UTF-8');
    }
}
