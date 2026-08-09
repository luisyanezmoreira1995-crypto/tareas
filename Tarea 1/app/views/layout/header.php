<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Gestión de Tareas</title>
    <link rel="stylesheet" href="<?= BASE_URL ?>/css/style.css">
</head>
<body>
<header class="navbar">
    <a href="<?= BASE_URL ?>/" class="brand">Gestión de Tareas</a>
    <nav>
        <?php if (isset($_SESSION['user_id'])): ?>
            <span class="nav-user">Hola, <?= htmlspecialchars($_SESSION['user_name']) ?></span>
            <a href="<?= BASE_URL ?>/projects">Proyectos</a>
            <a href="<?= BASE_URL ?>/auth/logout">Cerrar sesión</a>
        <?php else: ?>
            <a href="<?= BASE_URL ?>/auth/login">Iniciar sesión</a>
            <a href="<?= BASE_URL ?>/auth/register">Registrarse</a>
        <?php endif; ?>
    </nav>
</header>
<main class="container">
    <?php if (!empty($_SESSION['flash'])): ?>
        <div class="alert alert-<?= htmlspecialchars($_SESSION['flash']['type']) ?>">
            <?= htmlspecialchars($_SESSION['flash']['message']) ?>
        </div>
        <?php unset($_SESSION['flash']); ?>
    <?php endif; ?>
