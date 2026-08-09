<?php require APP_PATH . '/views/layout/header.php'; ?>

<div class="card auth-card">
    <h1>Crear cuenta</h1>

    <?php if (!empty($errors)): ?>
        <div class="alert alert-error">
            <ul>
                <?php foreach ($errors as $error): ?>
                    <li><?= htmlspecialchars($error) ?></li>
                <?php endforeach; ?>
            </ul>
        </div>
    <?php endif; ?>

    <form method="post" action="<?= BASE_URL ?>/auth/register">
        <label for="name">Nombre</label>
        <input type="text" id="name" name="name" required value="<?= htmlspecialchars($_POST['name'] ?? '') ?>">

        <label for="email">Correo</label>
        <input type="email" id="email" name="email" required value="<?= htmlspecialchars($_POST['email'] ?? '') ?>">

        <label for="password">Contraseña</label>
        <input type="password" id="password" name="password" required minlength="6">

        <label for="password_confirm">Confirmar contraseña</label>
        <input type="password" id="password_confirm" name="password_confirm" required minlength="6">

        <button type="submit" class="btn">Registrarme</button>
    </form>

    <p class="muted">¿Ya tienes cuenta? <a href="<?= BASE_URL ?>/auth/login">Inicia sesión</a></p>
</div>

<?php require APP_PATH . '/views/layout/footer.php'; ?>
