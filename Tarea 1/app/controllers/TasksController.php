<?php

class TasksController extends Controller
{
    private Task $taskModel;
    private Project $projectModel;

    public function __construct()
    {
        $this->requireAuth();
        $this->taskModel = $this->model('Task');
        $this->projectModel = $this->model('Project');
    }

    private function authorizeProject(int $projectId): array
    {
        $project = $this->projectModel->find($projectId);

        if (!$project || (int) $project['user_id'] !== (int) $_SESSION['user_id']) {
            header('Location: ' . BASE_URL . '/projects');
            exit;
        }

        return $project;
    }

    private function authorizeTask(int $taskId): array
    {
        $task = $this->taskModel->find($taskId);

        if (!$task) {
            header('Location: ' . BASE_URL . '/projects');
            exit;
        }

        $this->authorizeProject((int) $task['project_id']);

        return $task;
    }

    public function index(int $projectId): void
    {
        $project = $this->authorizeProject($projectId);
        $tasks = $this->taskModel->getAllByProject($projectId);
        $this->view('tasks/index', ['tasks' => $tasks, 'project' => $project]);
    }

    public function create(int $projectId): void
    {
        $project = $this->authorizeProject($projectId);
        $errors = [];

        if ($_SERVER['REQUEST_METHOD'] === 'POST') {
            $title = trim($_POST['title'] ?? '');
            $description = trim($_POST['description'] ?? '');
            $dueDate = !empty($_POST['due_date']) ? $_POST['due_date'] : null;

            if ($title === '') {
                $errors[] = 'El título es obligatorio.';
            }

            if (!$errors) {
                $this->taskModel->create($projectId, $title, $description, $dueDate);
                $_SESSION['flash'] = ['type' => 'success', 'message' => 'Tarea creada.'];
                header('Location: ' . BASE_URL . '/tasks/index/' . $projectId);
                exit;
            }
        }

        $this->view('tasks/form', ['errors' => $errors, 'task' => null, 'project' => $project]);
    }

    public function edit(int $id): void
    {
        $task = $this->authorizeTask($id);
        $project = $this->projectModel->find((int) $task['project_id']);
        $errors = [];

        if ($_SERVER['REQUEST_METHOD'] === 'POST') {
            $title = trim($_POST['title'] ?? '');
            $description = trim($_POST['description'] ?? '');
            $dueDate = !empty($_POST['due_date']) ? $_POST['due_date'] : null;
            $status = $_POST['status'] ?? 'pendiente';

            if ($title === '') {
                $errors[] = 'El título es obligatorio.';
            }

            if (!$errors) {
                $this->taskModel->update($id, $title, $description, $dueDate, $status);
                $_SESSION['flash'] = ['type' => 'success', 'message' => 'Tarea actualizada.'];
                header('Location: ' . BASE_URL . '/tasks/index/' . $task['project_id']);
                exit;
            }

            $task = array_merge($task, [
                'title' => $title,
                'description' => $description,
                'due_date' => $dueDate,
                'status' => $status,
            ]);
        }

        $this->view('tasks/form', ['errors' => $errors, 'task' => $task, 'project' => $project]);
    }

    public function delete(int $id): void
    {
        if ($_SERVER['REQUEST_METHOD'] === 'POST') {
            $task = $this->authorizeTask($id);
            $this->taskModel->delete($id);
            header('Location: ' . BASE_URL . '/tasks/index/' . $task['project_id']);
            exit;
        }

        header('Location: ' . BASE_URL . '/projects');
        exit;
    }

    public function toggle(int $id): void
    {
        if ($_SERVER['REQUEST_METHOD'] === 'POST') {
            $task = $this->authorizeTask($id);
            $next = $task['status'] === 'completada' ? 'pendiente' : 'completada';
            $this->taskModel->updateStatus($id, $next);
            header('Location: ' . BASE_URL . '/tasks/index/' . $task['project_id']);
            exit;
        }

        header('Location: ' . BASE_URL . '/projects');
        exit;
    }
}
