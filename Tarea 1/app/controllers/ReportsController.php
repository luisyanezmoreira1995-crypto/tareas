<?php

/**
 * Composition root de los reportes: aquí, y solo aquí, se decide qué
 * implementaciones concretas (FpdfDocument, TableReportRenderer,
 * TaskReportDataProvider) se inyectan en ReportGenerator. El resto del
 * sistema de reportes nunca se entera de estas clases concretas.
 */
class ReportsController extends Controller
{
    private Project $projectModel;
    private Task $taskModel;

    public function __construct()
    {
        $this->requireAuth();
        $this->projectModel = $this->model('Project');
        $this->taskModel = $this->model('Task');
    }

    public function tasks(int $projectId): void
    {
        $project = $this->projectModel->find($projectId);

        if (!$project || (int) $project['user_id'] !== (int) $_SESSION['user_id']) {
            header('Location: ' . BASE_URL . '/projects');
            exit;
        }

        $tasks = $this->taskModel->getAllByProject($projectId);

        $dataProvider = new TaskReportDataProvider($project, $tasks);
        $generator = new ReportGenerator(new FpdfDocument(), new TableReportRenderer());
        $pdfContent = $generator->generate($dataProvider);

        $this->streamPdf($pdfContent, 'reporte_tareas_' . $projectId . '.pdf');
    }

    private function streamPdf(string $content, string $fileName): void
    {
        header('Content-Type: application/pdf');
        header('Content-Disposition: attachment; filename="' . $fileName . '"');
        header('Content-Length: ' . strlen($content));
        echo $content;
        exit;
    }
}
