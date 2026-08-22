<?php

/**
 * Orquestador del reporte. Solo conoce abstracciones (DIP): recibe por
 * constructor un PdfDocumentInterface y un ReportRendererInterface ya
 * construidos (inyección de dependencias), y no instancia nada él mismo.
 */
class ReportGenerator
{
    private PdfDocumentInterface $pdf;
    private ReportRendererInterface $renderer;

    public function __construct(PdfDocumentInterface $pdf, ReportRendererInterface $renderer)
    {
        $this->pdf = $pdf;
        $this->renderer = $renderer;
    }

    public function generate(ReportDataProviderInterface $data): string
    {
        $this->renderer->render($this->pdf, $data);

        return $this->pdf->output('S');
    }
}
