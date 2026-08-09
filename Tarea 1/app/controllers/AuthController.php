<?php

class AuthController extends Controller
{
    private User $userModel;

    public function __construct()
    {
        $this->userModel = $this->model('User');
    }

    public function register(): void
    {
        if (isset($_SESSION['user_id'])) {
            header('Location: ' . BASE_URL . '/projects');
            exit;
        }

        $errors = [];

        if ($_SERVER['REQUEST_METHOD'] === 'POST') {
            $name = trim($_POST['name'] ?? '');
            $email = trim($_POST['email'] ?? '');
            $password = $_POST['password'] ?? '';
            $passwordConfirm = $_POST['password_confirm'] ?? '';

            if ($name === '') {
                $errors[] = 'El nombre es obligatorio.';
            }
            if (!filter_var($email, FILTER_VALIDATE_EMAIL)) {
                $errors[] = 'El correo no es válido.';
            }
            if (strlen($password) < 6) {
                $errors[] = 'La contraseña debe tener al menos 6 caracteres.';
            }
            if ($password !== $passwordConfirm) {
                $errors[] = 'Las contraseñas no coinciden.';
            }
            if (!$errors && $this->userModel->findByEmail($email)) {
                $errors[] = 'Ese correo ya está registrado.';
            }

            if (!$errors) {
                $this->userModel->register($name, $email, $password);
                $_SESSION['flash'] = ['type' => 'success', 'message' => 'Cuenta creada correctamente. Ahora inicia sesión.'];
                header('Location: ' . BASE_URL . '/auth/login');
                exit;
            }
        }

        $this->view('auth/register', ['errors' => $errors]);
    }

    public function login(): void
    {
        if (isset($_SESSION['user_id'])) {
            header('Location: ' . BASE_URL . '/projects');
            exit;
        }

        $errors = [];

        if ($_SERVER['REQUEST_METHOD'] === 'POST') {
            $email = trim($_POST['email'] ?? '');
            $password = $_POST['password'] ?? '';

            $user = $this->userModel->findByEmail($email);

            if ($user && password_verify($password, $user['password'])) {
                $_SESSION['user_id'] = $user['id'];
                $_SESSION['user_name'] = $user['name'];
                header('Location: ' . BASE_URL . '/projects');
                exit;
            }

            $errors[] = 'Correo o contraseña incorrectos.';
        }

        $this->view('auth/login', ['errors' => $errors]);
    }

    public function logout(): void
    {
        session_unset();
        session_destroy();
        header('Location: ' . BASE_URL . '/auth/login');
        exit;
    }
}
