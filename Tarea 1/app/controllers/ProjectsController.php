<?php

class ProjectsController extends Controller
{
    private Project $projectModel;

    public function __construct()
    {
        $this->requireAuth();
        $this->projectModel = $this->model('Project');
    }

    public function index(): void
    {
        $projects = $this->projectModel->getAllByUser((int) $_SESSION['user_id']);
        $this->view('projects/index', ['projects' => $projects]);
    }

    public function create(): void
    {
        $errors = [];

        if ($_SERVER['REQUEST_METHOD'] === 'POST') {
            $name = trim($_POST['name'] ?? '');
            $description = trim($_POST['description'] ?? '');

            if ($name === '') {
                $errors[] = 'El nombre es obligatorio.';
            }

            if (!$errors) {
                $this->projectModel->create((int) $_SESSION['user_id'], $name, $description);
                $_SESSION['flash'] = ['type' => 'success', 'message' => 'Proyecto creado.'];
                header('Location: ' . BASE_URL . '/projects');
                exit;
            }
        }

        $this->view('projects/form', ['errors' => $errors, 'project' => null]);
    }

    public function edit(int $id): void
    {
        $project = $this->projectModel->find($id);

        if (!$project || (int) $project['user_id'] !== (int) $_SESSION['user_id']) {
            header('Location: ' . BASE_URL . '/projects');
            exit;
        }

        $errors = [];

        if ($_SERVER['REQUEST_METHOD'] === 'POST') {
            $name = trim($_POST['name'] ?? '');
            $description = trim($_POST['description'] ?? '');

            if ($name === '') {
                $errors[] = 'El nombre es obligatorio.';
            }

            if (!$errors) {
                $this->projectModel->update($id, $name, $description);
                $_SESSION['flash'] = ['type' => 'success', 'message' => 'Proyecto actualizado.'];
                header('Location: ' . BASE_URL . '/projects');
                exit;
            }

            $project['name'] = $name;
            $project['description'] = $description;
        }

        $this->view('projects/form', ['errors' => $errors, 'project' => $project]);
    }

    public function delete(int $id): void
    {
        if ($_SERVER['REQUEST_METHOD'] === 'POST') {
            $project = $this->projectModel->find($id);

            if ($project && (int) $project['user_id'] === (int) $_SESSION['user_id']) {
                $this->projectModel->delete($id);
                $_SESSION['flash'] = ['type' => 'success', 'message' => 'Proyecto eliminado.'];
            }
        }

        header('Location: ' . BASE_URL . '/projects');
        exit;
    }
}
