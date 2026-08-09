<?php

class HomeController extends Controller
{
    public function index(): void
    {
        if (isset($_SESSION['user_id'])) {
            header('Location: ' . BASE_URL . '/projects');
        } else {
            header('Location: ' . BASE_URL . '/auth/login');
        }
        exit;
    }
}
