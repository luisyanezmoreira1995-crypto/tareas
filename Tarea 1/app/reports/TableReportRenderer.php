<?php

/**
 * SRP: su única responsabilidad es dibujar cualquier ReportDataProviderInterface
 * como una tabla. No sabe de dónde vienen los datos ni qué librería PDF hay
 * detrás de PdfDocumentInterface (OCP: sirve para tareas, proyectos, etc.
 * sin modificarse; DIP: depende de abstracciones, no de FPDF ni de Task).
 */
class TableReportRenderer implements ReportRendererInterface
{
    private const ROW_HEIGHT = 8.0;
    private const PAGE_CONTENT_WIDTH = 190.0;

    public function render(PdfDocumentInterface $pdf, ReportDataProviderInterface $data): void
    {
        $pdf->setTitle($data->getTitle());
        $pdf->addPage();

        $this->renderHeading($pdf, $data->getTitle());

        $columns = $data->getColumns();
        $columnWidth = self::PAGE_CONTENT_WIDTH / max(count($columns), 1);

        $this->renderTableHeader($pdf, $columns, $columnWidth);
        $this->renderTableRows($pdf, $columns, $data->getRows(), $columnWidth);
    }

    private function renderHeading(PdfDocumentInterface $pdf, string $title): void
    {
        $pdf->setFont('Arial', 'B', 14);
        $pdf->cell(0, 10, $title, 0, 1);
        $pdf->ln(4);
    }

    private function renderTableHeader(PdfDocumentInterface $pdf, array $columns, float $columnWidth): void
    {
        $pdf->setFont('Arial', 'B', 10);
        $pdf->setFillColor(52, 73, 94);
        $pdf->setTextColor(255, 255, 255);

        foreach ($columns as $label) {
            $pdf->cell($columnWidth, self::ROW_HEIGHT, $label, 1, 0, 'C', true);
        }

        $pdf->ln();
    }

    private function renderTableRows(PdfDocumentInterface $pdf, array $columns, array $rows, float $columnWidth): void
    {
        $pdf->setFont('Arial', '', 9);
        $pdf->setTextColor(0, 0, 0);

        if (!$rows) {
            $pdf->cell(self::PAGE_CONTENT_WIDTH, self::ROW_HEIGHT, 'No hay registros para mostrar.', 1, 1, 'C');
            return;
        }

        $fill = false;

        foreach ($rows as $row) {
            $pdf->setFillColor(245, 245, 245);

            foreach (array_keys($columns) as $key) {
                $pdf->cell($columnWidth, self::ROW_HEIGHT, (string) ($row[$key] ?? ''), 1, 0, 'L', $fill);
            }

            $pdf->ln();
            $fill = !$fill;
        }
    }
}
