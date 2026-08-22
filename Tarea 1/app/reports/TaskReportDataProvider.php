<?php

/**
 * SRP: su única responsabilidad es transformar tareas + proyecto (datos de
 * dominio ya obtenidos por el controlador) en la forma tabular que espera
 * un ReportRendererInterface. No conoce PDO ni FPDF.
 */
class TaskReportDataProvider implements ReportDataProviderInterface
{
    private array $project;
    private array $tasks;

    private const STATUS_LABELS = [
        'pendiente' => 'Pendiente',
        'en_progreso' => 'En progreso',
        'completada' => 'Completada',
    ];

    public function __construct(array $project, array $tasks)
    {
        $this->project = $project;
        $this->tasks = $tasks;
    }

    public function getTitle(): string
    {
        return 'Reporte de tareas - ' . $this->project['name'];
    }

    public function getColumns(): array
    {
        return [
            'title' => 'Titulo',
            'description' => 'Descripcion',
            'due_date' => 'Vence',
            'status' => 'Estado',
        ];
    }

    public function getRows(): array
    {
        $rows = [];

        foreach ($this->tasks as $task) {
            $rows[] = [
                'title' => $task['title'],
                'description' => $task['description'] ?: '-',
                'due_date' => $task['due_date'] ?: 'Sin fecha',
                'status' => self::STATUS_LABELS[$task['status']] ?? $task['status'],
            ];
        }

        return $rows;
    }
}
