<?php require APP_PATH . '/views/layout/header.php'; ?>

<div class="card form-card">
    <h1><?= $project ? 'Editar proyecto' : 'Nuevo proyecto' ?></h1>

    <?php if (!empty($errors)): ?>
        <div class="alert alert-error">
            <ul>
                <?php foreach ($errors as $error): ?>
                    <li><?= htmlspecialchars($error) ?></li>
                <?php endforeach; ?>
            </ul>
        </div>
    <?php endif; ?>

    <form method="post" action="<?= $project ? BASE_URL . '/projects/edit/' . $project['id'] : BASE_URL . '/projects/create' ?>">
        <label for="name">Nombre</label>
        <input type="text" id="name" name="name" required value="<?= htmlspecialchars($project['name'] ?? '') ?>">

        <label for="description">Descripción</label>
        <textarea id="description" name="description" rows="4"><?= htmlspecialchars($project['description'] ?? '') ?></textarea>

        <div class="form-actions">
            <button type="submit" class="btn"><?= $project ? 'Guardar cambios' : 'Crear proyecto' ?></button>
            <a href="<?= BASE_URL ?>/projects" class="btn btn-secondary">Cancelar</a>
        </div>
    </form>
</div>

<?php require APP_PATH . '/views/layout/footer.php'; ?>
