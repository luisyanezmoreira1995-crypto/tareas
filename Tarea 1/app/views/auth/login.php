<?php require APP_PATH . '/views/layout/header.php'; ?>

<div class="card auth-card">
    <h1>Iniciar sesión</h1>

    <?php if (!empty($errors)): ?>
        <div class="alert alert-error">
            <ul>
                <?php foreach ($errors as $error): ?>
                    <li><?= htmlspecialchars($error) ?></li>
                <?php endforeach; ?>
            </ul>
        </div>
    <?php endif; ?>

    <form method="post" action="<?= BASE_URL ?>/auth/login">
        <label for="email">Correo</label>
        <input type="email" id="email" name="email" required value="<?= htmlspecialchars($_POST['email'] ?? '') ?>">

        <label for="password">Contraseña</label>
        <input type="password" id="password" name="password" required>

        <button type="submit" class="btn">Entrar</button>
    </form>

    <p class="muted">¿No tienes cuenta? <a href="<?= BASE_URL ?>/auth/register">Regístrate</a></p>
</div>

<?php require APP_PATH . '/views/layout/footer.php'; ?>
