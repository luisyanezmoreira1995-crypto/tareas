<?php

class Controller
{
    protected function model(string $model)
    {
        require_once APP_PATH . '/models/' . $model . '.php';
        return new $model();
    }

    protected function view(string $view, array $data = []): void
    {
        extract($data);
        $viewFile = APP_PATH . '/views/' . $view . '.php';

        if (file_exists($viewFile)) {
            require $viewFile;
        } else {
            http_response_code(404);
            die('La vista "' . htmlspecialchars($view) . '" no existe.');
        }
    }

    protected function requireAuth(): void
    {
        if (!isset($_SESSION['user_id'])) {
            header('Location: ' . BASE_URL . '/auth/login');
            exit;
        }
    }
}
